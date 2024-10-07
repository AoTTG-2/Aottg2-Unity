// -----------------------------------------------------------------------
// <copyright file="Voice.cs" company="Exit Games GmbH">
//   Photon Voice API Framework for Photon - Copyright (C) 2017 Exit Games GmbH
// </copyright>
// <summary>
//   Photon data streaming support.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Photon.Voice
{
    /// <summary>
    /// Interface for pulling data, in case this is more appropriate than pushing it.
    /// </summary>
    public interface IDataReader<T> : IDisposable
    {
        /// <summary>Fill full given frame buffer with source uncompressed data or return false if not enough such data.</summary>
        /// <param name="buffer">Buffer to fill.</param>
        /// <returns>True if buffer was filled successfully, false otherwise.</returns>
        bool Read(T[] buffer);
    }

    /// <summary>
    /// Interface for classes that want their Service() function to be called regularly in the context of a LocalVoice.
    /// </summary>
    public interface IServiceable
    {
        /// <summary>Service function that should be called regularly.</summary>
        void Service(LocalVoice localVoice);
    }

    public class FrameOut<T>
    {
        public FrameOut(T[] buf, bool endOfStream)
        {
            Set(buf, endOfStream);
        }
        public FrameOut<T> Set(T[] buf, bool endOfStream)
        {
            Buf = buf;
            EndOfStream = endOfStream;
            return this;
        }
        public T[] Buf { get; private set; }
        public bool EndOfStream { get; private set; } // stream interrupted but may be resumed, flush the output
    }

    /// <summary>
    /// Used to initialize optional properties of the LocalVoice instance at creation time.
    /// </summary>
    public struct VoiceCreateOptions
    {
        /// <summary>Encoder.</summary>
        public IEncoder Encoder;
        /// <summary>See <see cref="LocalVoice.InterestGroup"/>.</summary>
        public byte InterestGroup;
        /// <summary>See <see cref="LocalVoice.TargetPlayers"/>Set to [] to make sure that LocalVoice does not create Remote voices during creation</summary>
        public int[] TargetPlayers;
        /// <summary>See <see cref="LocalVoice.DebugEchoMode"/>.</summary>
        public bool DebugEchoMode;
        /// <summary>See <see cref="LocalVoice.Reliable"/>.</summary>
        public bool Reliable;
        /// <summary>See <see cref="LocalVoice.Encrypt"/>.</summary>
        public bool Encrypt;
        /// <summary>See <see cref="LocalVoice.Fragment"/>.</summary>
        public bool Fragment;
        /// <summary>See <see cref="LocalVoice.FEC"/>.</summary>
        public int FEC;

    }

    /// <summary>
    /// Represents outgoing data stream.
    /// </summary>
    public class LocalVoice : IDisposable
    {
        public const int DATA_POOL_CAPACITY = 50; // TODO: may depend on data type and properties, set for average audio stream

        /// <summary>Returns Info structure assigned on local voice cration.</summary>
        public VoiceInfo Info { get { return info; } }
        /// <summary>If true, stream data broadcasted.</summary>
        public bool TransmitEnabled
        {
            get
            {
                return transmitEnabled;
            }
            set
            {
                if (transmitEnabled != value)
                {
                    if (transmitEnabled)
                    {
                        if (encoder != null && this.voiceClient.transport.IsChannelJoined(this.channelId))
                        {
                            encoder.EndOfStream();
                        }
                    }
                    transmitEnabled = value;
                }
            }
        }
        private bool transmitEnabled = true;

        /// <summary>Returns true if stream broadcasts.</summary>
        public bool IsCurrentlyTransmitting
        {
            get { return Environment.TickCount - lastTransmitTime < NO_TRANSMIT_TIMEOUT_MS; }
        }

        /// <summary>Sent frames counter.</summary>
        public int FramesSent { get; private set; }

        /// <summary>Sent fragmented frames counter.</summary>
        public int FramesSentFragmented { get; private set; }

        /// <summary>Sent frames fragments counter.</summary>
        public int FramesSentFragments { get; private set; }

        /// <summary>Sent frames bytes counter.</summary>
        public int FramesSentBytes { get; private set; }

        /// <summary>Send data reliable. See also <see cref="VoiceCreateOptions.Reliable"/>.</summary>
        public bool Reliable { get; set; }

        /// <summary>Send data encrypted. See also <see cref="VoiceCreateOptions.Encrypt"/>.</summary>
        public bool Encrypt { get; set; }

        /// <summary>Split frames into fragments according to the size provided by the Transport. See also <see cref="VoiceCreateOptions.Fragment"/>.</summary>
        public bool Fragment { get; set; }

        /// <summary>Forward Error Correction control. See also <see cref="VoiceCreateOptions.FEC"/>.</summary>
        public int FEC { get; set; }

        /// <summary>Optional user object attached to LocalVoice. its Service() will be called at each VoiceClient.Service() call.</summary>
        public IServiceable LocalUserServiceable { get; set; }

        [Obsolete("Use InterestGroup.")]
        public byte Group { get { return InterestGroup; } set { InterestGroup = value; } }

        /// <summary>If InterestGroup != 0, streaming only to the players subscribed to this group (if supported by the transport). See also <see cref="VoiceCreateOptions.InterestGroup"/>.</summary>
        /// <remarks>A remote voice is created even if the remote player is not subscribed to the InterestGroup. Use <see cref="VoiceCreateOptions.TargetPlayers"/> and <see cref="TargetPlayers"/> to completely hide the existence of LocalVoice from the remote player./></remarks>
        public byte InterestGroup { get; set; }

        /// <summary>
        /// If true, outgoing stream routed back to client via server same way as for remote client's streams. See also <see cref="VoiceCreateOptions.DebugEchoMode"/>.</summary>
        /// <remarks>This functionality availability depends on transport.</remarks>
        /// </summary>
        public bool DebugEchoMode
        {
            get { return debugEchoMode; }
            set
            {
                if (debugEchoMode != value)
                {
                    debugEchoMode = value;
                    if (isJoined)
                    {
                        if (debugEchoMode)
                        {
                            sendVoiceInfoAndConfigFrame(true, new int[0]);
                        }
                        else
                        {
                            sendVoiceRemove(true, new int[0]);
                        }
                    }

                }
            }
        }
        bool debugEchoMode;

        /// <summary>If TargetPlayers is not null, sending voice info and streaming only to clients having player numbers specified in the array (if supported by transport). See also <see cref="VoiceCreateOptions.TargetPlayers"/>.</summary>
        /// <remarks>
        /// TargetPlayers update triggers the sending of voice info to added players and voice remove to removed.
        /// Depending on the transport, TargetPlayers may disregard <see cref="InterestGroup"/>: the remote player whos number is in TargetPlayers, receives the stream even if not subscribed to the interest group.
        /// If the local player number is in TargetPlayers, it works like <see cref="DebugEchoMode"/>.
        /// Using both DebugEchoMode and TargetPlayers at the same time to route the stream back to the sender leads to an inconsistent state after one of the options is switched off: the voice is removed but the frames are still delivered.
        /// </remarks>
        public int[] TargetPlayers
        {
            get => targetPlayers_ == null ? null : (int[])targetPlayers_.Clone();

            set
            {
                int[] tpNew = value == null ? null : (int[])value.Clone();

                if (isJoined)
                {
                    if (targetPlayers_ != null && tpNew != null)
                    {
                        sendVoiceRemove(false, targetPlayers_.Except(tpNew).ToArray());
                        sendVoiceInfoAndConfigFrame(false, tpNew.Except(targetPlayers_).ToArray());
                    }
                    else if (targetPlayers_ != null || tpNew != null)
                    {
                        sendVoiceRemove(false, targetPlayers_);
                        sendVoiceInfoAndConfigFrame(false, tpNew);
                    }
                    // else both are null, no action required
                }

                targetPlayers_ = tpNew;
            }
        }
        protected int[] targetPlayers_;

        public void SendSpacingProfileStart()
        {
            sendSpacingProfile.Start();
        }

        public string SendSpacingProfileDump { get { return sendSpacingProfile.Dump; } }

        /// <summary>
        /// Logs input frames time spacing profiling results. Do not call frequently.
        /// </summary>
        public int SendSpacingProfileMax { get { return sendSpacingProfile.Max; } }

        public byte ID { get { return id; } }
        public byte EvNumber { get { return evNumber; } } // ignored by remote client, see RemoteVoice()

        #region nonpublic

        protected VoiceInfo info;
        protected IEncoder encoder;
        internal byte id;
        internal int channelId;
        internal byte evNumber = 0; // sequence used by receivers to detect loss. will overflow.
        protected VoiceClient voiceClient;
        protected bool threadingEnabled;
        protected ArraySegment<byte> configFrame;

        volatile protected bool disposed;
        protected object disposeLock = new object();
        internal LocalVoice() // for dummy voices
        {
        }

        internal LocalVoice(VoiceClient voiceClient, byte id, VoiceInfo voiceInfo, int channelId, VoiceCreateOptions opt)
        {
            this.info = voiceInfo;

            this.channelId = channelId;
            this.InterestGroup = opt.InterestGroup;
            this.TargetPlayers = opt.TargetPlayers;
            this.DebugEchoMode = opt.DebugEchoMode;
            this.Reliable = opt.Reliable;
            this.Encrypt = opt.Encrypt;
            this.Fragment = opt.Fragment;
            this.FEC = opt.FEC;

            this.voiceClient = voiceClient;
            this.threadingEnabled = voiceClient.ThreadingEnabled;
            this.id = id;

            if (opt.Encoder == null)
            {
                var m = LogPrefix + ": encoder is null";
                voiceClient.logger.LogError(m);
                throw new ArgumentNullException("encoder");
            }
            this.encoder = opt.Encoder;
            this.encoder.Output = sendFrame;
        }

        protected string shortName { get { return "v#" + id + "ch#" + voiceClient.channelStr(channelId); } }

        public string Name { get { return "Local " + info.Codec + " v#" + id + " ch#" + voiceClient.channelStr(channelId); } }
        public string LogPrefix { get { return "[PV] " + Name; } }

        private const int NO_TRANSMIT_TIMEOUT_MS = 100; // should be greater than SendFrame() call interval
        private int lastTransmitTime = Environment.TickCount - NO_TRANSMIT_TIMEOUT_MS;

        internal virtual void service()
        {
            while (true)
            {
                FrameFlags f;
                var x = encoder.DequeueOutput(out f);
                if (x.Count == 0)
                {
                    break;
                }
                else
                {
                    sendFrame(x, f);
                }
            }

            if (LocalUserServiceable != null)
            {
                LocalUserServiceable.Service(this);
            }
        }

        // voiceClient is null for dummy voices
        protected bool isJoined => voiceClient != null && voiceClient.transport.IsChannelJoined(this.channelId);

        // prevents calling ITransport event method on empty targets (targetMe is false and targetPlayers is [])
        protected bool targetExits(bool targetMe, int[] targetPlayers)
        {
            return targetMe || targetPlayers == null || targetPlayers.Length > 0;
        }

        internal void onJoinChannel()
        {
            sendVoiceInfoAndConfigFrame(DebugEchoMode, targetPlayers_);
        }

        internal void onLeaveChannel()
        {
            sendVoiceRemove(DebugEchoMode, targetPlayers_);
        }

        internal void onPlayerJoin(int playerId)
        {
            if (targetPlayers_ == null || targetPlayers_.Contains(playerId))
            {
                sendVoiceInfoAndConfigFrame(false, new int[] { playerId });
            }
            else
            {
                this.voiceClient.logger.LogInfo(LogPrefix + " player " + playerId + " join is ignored becuase it's not in target players");
            }
        }

        internal void sendVoiceInfoAndConfigFrame()
        {
            sendVoiceInfoAndConfigFrame(DebugEchoMode, targetPlayers_);
        }

        private string getTargetStr(bool targetMe, int[] targetPlayers)
        {
            string targetStr;
            if (targetPlayers != null)
            {
                targetStr = string.Join(", ", targetPlayers);
            }
            else
            {
                targetStr = "others";
            }
            if (targetMe)
            {
                targetStr += (targetStr.Length > 0 ? " and " : "") + "me";
            }

            return targetStr;
        }

        protected void sendVoiceInfoAndConfigFrame(bool targetMe, int[] targetPlayers)
        {
            if (targetExits(targetMe, targetPlayers))
            {
                string targetStr = getTargetStr(targetMe, targetPlayers);

                this.voiceClient.logger.LogInfo(LogPrefix + " Sending voice info to " + targetStr + ": " + info.ToString() + " ev=" + evNumber);
                voiceClient.transport.SendVoiceInfo(this, channelId, targetMe, targetPlayers);

                if (configFrame.Count != 0)
                {
                    this.voiceClient.logger.LogInfo(LogPrefix + " Sending config frame to " + targetStr);
                    sendFrame0(configFrame, FrameFlags.Config, targetMe, targetPlayers, 0, true);
                }
            }
        }

        protected void sendVoiceRemove(bool targetMe, int[] targetPlayers)
        {
            if (targetExits(targetMe, targetPlayers))
            {
                this.voiceClient.logger.LogInfo(LogPrefix + " Sending voice remove to " + getTargetStr(targetMe, targetPlayers));
                voiceClient.transport.SendVoiceRemove(this, channelId, targetMe, targetPlayers);
            }
        }

        internal void sendFrame(ArraySegment<byte> compressed, FrameFlags flags)
        {
            if ((flags & FrameFlags.Config) != 0)
            {
                byte[] a = configFrame.Array != null && configFrame.Array.Length >= compressed.Count ? configFrame.Array : new byte[compressed.Count];
                Buffer.BlockCopy(compressed.Array, compressed.Offset, a, 0, compressed.Count);
                configFrame = new ArraySegment<byte>(a, 0, compressed.Count);

                this.voiceClient.logger.LogInfo(LogPrefix + " Got config frame from encoder, " + configFrame.Count + " bytes");
            }
            if (this.voiceClient.transport.IsChannelJoined(this.channelId) && this.TransmitEnabled)
            {
                // test
                //compressed = new ArraySegment<byte>(new byte[FramesSent + 2000].Select(x => (byte)rnd.Next()).ToArray());
                sendFrame0(compressed, flags, DebugEchoMode, targetPlayers_, InterestGroup, Reliable);
            }
        }

        // Optionally fragments into multiple events
        internal void sendFrame0(ArraySegment<byte> compressed, FrameFlags flags, bool targetMe, int[] targetPlayers, byte interestGroup, bool reliable)
        {
            if (!targetExits(targetMe, targetPlayers))
            {

return;
            }

            bool fragment = Fragment && (flags & FrameFlags.Config) == 0; // fragmentation of config frames is not supported (see RemoteVoice.configFrameQueue)

            // sending reliably breaks timing
            // consider sending multiple EndOfStream packets for reliability
            if ((flags & FrameFlags.EndOfStream) != 0)
            {
                //                reliable = true;
            }

            var sendFramePar = new SendFrameParams(targetMe, targetPlayers, interestGroup, reliable, this.Encrypt);

            int maxFragSize = fragment ? voiceClient.transport.GetPayloadFragmentSize(sendFramePar) : 0;

            if (maxFragSize <= 0 || compressed.Count <= maxFragSize)
            {
                sendFrameEvent(compressed, flags, sendFramePar);
            }
            else
            {
                // We add 1 byte with fragments count to the end of the 1st fragement buffer.
                // For non-last fragments, we can always safely borrow the 1st byte of the next fragment to quickly make a +1 byte buffer.
                int totCount = compressed.Count + 1; // +1 byte
                byte fragCount = (byte)((totCount + maxFragSize - 1) / maxFragSize);
                for (byte i = 0; i < fragCount; i++)
                {
                    bool last = i == fragCount - 1;

                    var flagsFrag = flags;
                    if (i > 0) // not 1st
                    {
                        flagsFrag |= FrameFlags.FragNotBeg;
                    }

                    if (!last)
                    {
                        flagsFrag |= FrameFlags.FragNotEnd;
                    }

                    int fragSize;
                    byte borrowedByte = 0;
                    if (i == 0)
                    {
                        borrowedByte = compressed.Array[compressed.Offset + maxFragSize];
                        compressed.Array[compressed.Offset + maxFragSize] = fragCount;
                        fragSize = maxFragSize + 1;
                    }
                    else if (last)
                    {
                        fragSize = compressed.Count % maxFragSize;
                    }
                    else
                    {
                        fragSize = maxFragSize;
                    }

                    sendFrameEvent(new ArraySegment<byte>(compressed.Array, compressed.Offset + i * maxFragSize, fragSize), flagsFrag, sendFramePar);

                    if (i == 0)
                    {
                        compressed.Array[compressed.Offset + maxFragSize] = borrowedByte;
                    }

                    this.FramesSentFragments++;
                }

                this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + evNumber + " fr#" + FramesSent + " c#" + fragCount + " Fragmented sent from events " + (byte)(evNumber - fragCount) + "-" + evNumber + ", size: " + compressed.Count + ", flags: " + flags);
                this.FramesSentFragmented++;
            }

            this.FramesSent++;
            this.FramesSentBytes += compressed.Count;

            if (compressed.Count > 0 && (flags & FrameFlags.Config) == 0) // otherwise the frame is config or control (EOS)
            {
                lastTransmitTime = Environment.TickCount;
            }
        }

        const int FEC_INFO_SIZE = 5; // write additional data required for recovery at the buffer end
        byte[] fecBuffer = new byte[0];
        FrameFlags fecFlags;
        byte fecFrameNumber;
        int fecTotSize;
        int fecMaxSize; // should be always updated synchronously with FEC data because resetFEC() clears exactly fecMaxSize + FEC_INFO_SIZE bytes
        byte fecCnt;

        void resetFEC()
        {
            Array.Clear(fecBuffer, 0, fecMaxSize + FEC_INFO_SIZE);
            fecFlags = 0;
            fecFrameNumber = 0;
            fecTotSize = 0;
            fecMaxSize = 0;
            fecCnt = 0;
        }

        // Optionally injects FEC events
        void sendFrameEvent(ArraySegment<byte> data, FrameFlags flags, SendFrameParams sendFramePar)
        {
            int fec = FEC;

//            if (this.evNumber % 7 != 0)
            this.voiceClient.transport.SendFrame(data, flags, this.evNumber, (byte)this.FramesSent, id, this.channelId, sendFramePar);

            this.sendSpacingProfile.Update(false, false);
            if (this.DebugEchoMode)
            {
                this.eventTimestamps[this.evNumber] = Environment.TickCount;
            }

            this.evNumber++;

            if (fec > 0)
            {
                if (fecBuffer.Length < data.Count + FEC_INFO_SIZE)
                {
                    var tmp = fecBuffer;
                    fecBuffer = new byte[data.Count + FEC_INFO_SIZE];
                    Array.Copy(tmp, fecBuffer, fecMaxSize);
                }

                for (int i = 0; i < data.Count; i++)
                {
                    fecBuffer[i] ^= data.Array[data.Offset + i];
                }
                fecMaxSize = fecMaxSize < data.Count ? data.Count : fecMaxSize;
                fecFlags ^= flags;
                fecFrameNumber ^= (byte)this.FramesSent;
                fecTotSize += data.Count;
                fecCnt++;
                if (fecCnt >= fec)
                {
                    fecBuffer[fecMaxSize + 0] = (byte)fecFrameNumber;
                    fecBuffer[fecMaxSize + 1] = (byte)fecFlags;
                    fecBuffer[fecMaxSize + 2] = (byte)fecTotSize;
                    fecBuffer[fecMaxSize + 3] = (byte)(fecTotSize >> 8);
                    fecBuffer[fecMaxSize + 4] = (byte)(this.evNumber - fecCnt);
                    // assign evNumber to the FEC event but do not increment it to avoid timing and decoding issues (lost FEC event cannot be distinguished from regular lost event)
                    // FEC events processed in a separate queue, so numbers do not clash
                    // it's easier to process FEC event if its number is 1 more than the last xored event number
                    // frame number is not relevant, passing event number instead
                    this.voiceClient.transport.SendFrame(new ArraySegment<byte>(fecBuffer, 0, fecMaxSize + FEC_INFO_SIZE), FrameFlags.FEC, this.evNumber, this.evNumber, id, this.channelId, sendFramePar);

                    resetFEC();
                }
            }
        }

        internal Dictionary<byte, int> eventTimestamps = new Dictionary<byte, int>();

        SpacingProfile sendSpacingProfile = new SpacingProfile(1000);
        #endregion

        /// <summary>Remove this voice from it's VoiceClient (using VoiceClient.RemoveLocalVoice</summary>
        public void RemoveSelf()
        {
            if (this.voiceClient != null) // dummy voice can try to remove self
            {
                this.voiceClient.RemoveLocalVoice(this);
            }
        }

        public virtual void Dispose()
        {
            if (!disposed)
            {
                if (this.encoder != null)
                {
                    this.encoder.Dispose();
                }
                disposed = true;
            }
        }
    }

    /// <summary>Event Actions and other options for a remote voice (incoming stream).</summary>
    public struct RemoteVoiceOptions
    {
        public RemoteVoiceOptions(ILogger logger, string logPrefix, VoiceInfo voiceInfo)
        {
            this.logger = logger;
            this.logPrefix = logPrefix;
            this.voiceInfo = voiceInfo;
            this.Decoder = null;
            this.OnRemoteVoiceRemoveAction = null;
        }

        /// <summary>
        /// Create default audio decoder and register a method to be called when a data frame is decoded.
        /// </summary>
        public void SetOutput(Action<FrameOut<float>> output)
        {
            if (voiceInfo.Codec == Codec.Raw) // Debug only. Assumes that original data is short[].
            {
                this.Decoder = new RawCodec.Decoder<short>(new RawCodec.ShortToFloat(output as Action<FrameOut<float>>).Output);
                return;
            }
            setOutput<float>(output);
        }

        /// <summary>
        /// Create default audio decoder and register a method to be called when a data frame is decoded.
        /// </summary>
        public void SetOutput(Action<FrameOut<short>> output)
        {
            if (voiceInfo.Codec == Codec.Raw) // Debug only. Assumes that original data is short[].
            {
                this.Decoder = new RawCodec.Decoder<short>(output);
                return;
            }
            setOutput<short>(output);
        }

        private void setOutput<T>(Action<FrameOut<T>> output)
        {
            logger.LogInfo(logPrefix + ": Creating default decoder " + voiceInfo.Codec + " for output FrameOut<" + typeof(T) + ">");
            if (voiceInfo.Codec == Codec.AudioOpus)
            {
                this.Decoder = new OpusCodec.Decoder<T>(output, logger);
            }
            else
            {
                logger.LogError(logPrefix + ": FrameOut<" + typeof(T) + "> output set for non-audio decoder " + voiceInfo.Codec);
            }
        }

        /// <summary>
        /// Register a method to be called when the remote voice is removed.
        /// </summary>
        public Action OnRemoteVoiceRemoveAction { get; set; }

        /// <summary>Remote voice data decoder. Use to set decoder options or override it with user decoder.</summary>
        public IDecoder Decoder { get; set; }

        private readonly ILogger logger;
        private readonly VoiceInfo voiceInfo;
        internal string logPrefix { get; }

    }

    internal class RemoteVoice : IDisposable
    {
        // Hides FrameBuffer and lock arrays behind a nice interface but slows down operations by 3 times in C# and Unity IL2CPP apps.
        // The performance impact is negligible in a real app but we still prefer to lock and access without calls.
        /*
        class RingBuffer
        {
            FrameBuffer[] buf = new FrameBuffer[256];
            // buf per element lock
            // A thread tries to lock a frameQueue element by writing 1 to the correspondent bufLock element. If the previous value was already 1, lock fails and the thread starts over.
            // To release the lock, the thread writes 0.
            int[] bufLock = new int[256];

            public ref FrameBuffer Lock(int i)
            {
                while (Interlocked.Exchange(ref bufLock[i], 1) == 1) ; // lock single slot for writing
                return ref buf[i];
            }

            public void Unlock(int i)
            {
                Interlocked.Exchange(ref bufLock[i], 0);               // unlock single slot
            }

            public void Swap(int i, FrameBuffer f)
            {
                Lock(i);
                buf[i].Release(); // unprocessed frame may be in the slot
                buf[i] = f;
                Unlock(i);
            }

            public ref FrameBuffer this[int i]
            {
                get => ref buf[i];
            }

            public void UnloclAll()
            {
                Array.Clear(bufLock, 0, bufLock.Length);
            }

            public void Clear()
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    buf[i].Release();
                    buf[i] = nullFrame;
                }
            }
        }
        */

        // Client.RemoteVoiceInfos support
        internal VoiceInfo Info { get; private set; }
        internal RemoteVoiceOptions options;
        internal int channelId;

        // The delay between frameQueue writer and reader.
        // Originally designed for simple streams synchronization.
        // In DeliveryMode.UnreliableUnsequenced Photon transport mode, helps to save late events (although some out-of-order events are managed to process in order even with 0 delay)
        // For FEC, should be >= FEC injection interval to fully utilize a FEC event belonging to different frames. A fragmented frame may have FEC events belonging to this frame only.
        // As soon as the receiver finds a fragmented frame in the stream, it sets the actual delay to be at least 1 to ensure that all fragments have time to arrive.
        internal int DelayFrames { get; set; }
        private int playerId;
        private byte voiceId;
        protected bool threadingEnabled;
        volatile private bool disposed;
        object disposeLock = new object();
        // incremented while receiveBytes() is running
        volatile private int receiving;
        // > 0 while decode thread is running
        volatile private bool decoding;

        internal RemoteVoice(VoiceClient client, RemoteVoiceOptions options, int channelId, int playerId, byte voiceId, VoiceInfo info, byte lastEventNumber) // 1st received event instead of 'lastEventNumber' parameter is used to init numbers (see 'started' field)
        {
            this.options = options;
            this.LogPrefix = options.logPrefix;
            this.voiceClient = client;
            this.threadingEnabled = voiceClient.ThreadingEnabled;
            this.channelId = channelId;
            this.playerId = playerId;
            this.voiceId = voiceId;
            this.Info = info;

            if (this.options.Decoder == null)
            {
                var m = LogPrefix + ": decoder is null (set it with options Decoder property or SetOutput method in OnRemoteVoiceInfoAction)";
                voiceClient.logger.LogError(m);
                disposed = true;
                return;
            }

            if (!threadingEnabled)
            {
                voiceClient.logger.LogInfo(LogPrefix + ": Starting decode singlethreaded");
                options.Decoder.Open(Info);
            }
            else
            {
#if NETFX_CORE
                Windows.System.Threading.ThreadPool.RunAsync((x) =>
                {
                    decodeThread();
                });
#else
                var t = new Thread(decodeThread);
                Util.SetThreadName(t, "[PV] Dec" + shortName);
                t.Start();
#endif
            }
        }

        private string shortName { get { return "v#" + voiceId + "ch#" + voiceClient.channelStr(channelId) + "p#" + playerId; } }
        public string LogPrefix { get; private set; }

        SpacingProfile receiveSpacingProfile = new SpacingProfile(1000);

        /// <summary>
        /// Starts input frames time spacing profiling. Once started, it can't be stopped.
        /// </summary>
        public void ReceiveSpacingProfileStart()
        {
            receiveSpacingProfile.Start();
        }

        public string ReceiveSpacingProfileDump { get { return receiveSpacingProfile.Dump; } }

        /// <summary>
        /// Logs input frames time spacing profiling results. Do not call frequently.
        /// </summary>
        public int ReceiveSpacingProfileMax { get { return receiveSpacingProfile.Max; } }

        private VoiceClient voiceClient;

        internal void receiveBytes(ref FrameBuffer receivedBytes, byte evNumber)
        {
            if (receivedBytes.IsConfig)
            {
                if ((receivedBytes.Flags & FrameFlags.MaskFrag) != 0)
                {
                    this.voiceClient.logger.LogError(LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + ", flags: " + receivedBytes.Flags + ": config frame can't be fragmented");
                }
                else
                {
                    // Prevents the very unlikely infinite growth.
                    // IsEmpty seems to be faster than Count and the queue is mostly empty.
                    while (!configFrameQueue.IsEmpty && configFrameQueue.Count > 10)
                    {
                        if (configFrameQueue.TryDequeue(out FrameBuffer confFrame))
                        {
                            confFrame.Release();
                        }
                    }
                    configFrameQueue.Enqueue(receivedBytes);
                    receivedBytes.Retain();
                }

                return;
            }
            // to avoid multiple empty frames injeciton to the decoder at startup when the current frame number is unknown.
            if (!started && !receivedBytes.IsFEC)
            {
                started = true;
                frameReadPos = (byte)(receivedBytes.FrameNum - 1);
                frameWritePos = receivedBytes.FrameNum;
                eventReadPos = evNumber;
            }

            if (receivedBytes.IsFEC)
            {
                // store the event in FEC events queue
                while (Interlocked.Exchange(ref fecQueueLock[evNumber], 1) == 1) ; // lock single slot for writing
                fecQueue[evNumber].Release(); // unprocessed frame may be in the slot
                fecQueue[evNumber] = receivedBytes;
                Interlocked.Exchange(ref fecQueueLock[evNumber], 0);               // unlock single slot

                // prevent disposing in Release() after return, frame copy in frameQueue still has refCnt = 1
                receivedBytes.Retain();

                // fill xored events array at indexes from xor_start_ev to evNumber - 1 (see sendFrameEvent)
                // [..., flags, size_lsb, size_msb, xor_start_ev]
                for (byte i = receivedBytes.Array[receivedBytes.Offset + receivedBytes.Length - 1]; i != evNumber; i++)
                {
                    fecXoredEvents[i] = evNumber;
                }

                fecEventTimeout = 0;
            }
            else
            {
                // store the event in the main events queue
                while (Interlocked.Exchange(ref eventQueueLock[evNumber], 1) == 1) ; // lock single slot for writing
                eventQueue[evNumber].Release(); // unprocessed frame may be in the slot
                eventQueue[evNumber] = receivedBytes;
                Interlocked.Exchange(ref eventQueueLock[evNumber], 0);               // unlock single slot

                // prevent disposing in Release() after return, frame copy in frameQueue still has refCnt = 1
                receivedBytes.Retain();

                if ((receivedBytes.Flags & FrameFlags.EndOfStream) != 0)
                {
                    flushingFrameNum = evNumber;
                }

                // there is no synchronization between multiple receiving threads, so the value can be > FEC_EVENT_TIMEOUT_INF, but with a reasonable number of threads this is not a problem
                if (fecEventTimeout < FEC_EVENT_TIMEOUT_INF)
                {
                    fecEventTimeout++;
                }

                // receive gap
                int miss = (byte)(receivedBytes.FrameNum - (frameWritePos + 1));

                if (miss > 127)
                {
                    // late (out of order) frame
                    // this frame is already counted in FramesMiss
                    this.voiceClient.FramesLate++;
                    this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + " late: " + (255 - miss) + " r/b " + receivedBytes.Length + ", flags: " + receivedBytes.Flags);
                }
                else
                {
                    frameWritePos = receivedBytes.FrameNum;
                    if (frameQueueReady != null)
                    {
                        frameQueueReady.Set();
                    }

                    if (miss != 0)
                    {
                        this.voiceClient.FramesMiss += miss;
                        this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + " miss: " + miss + " r/b " + receivedBytes.Length + ", flags: " + receivedBytes.Flags);
                    }
                }

                if (!threadingEnabled)
                {
                    try
                    {
                        decodeQueue();
                    }
                    catch (Exception e)
                    {
                        voiceClient.logger.LogError(LogPrefix + ": Exception in receiveBytes: " + e);
                        Interlocked.Decrement(ref receiving);
                        Dispose();
                    }
                }
                receiveSpacingProfile.Update(false, (receivedBytes.Flags & FrameFlags.EndOfStream) != 0);
            }

            // decrementing after Dispose() does not hurt
            Interlocked.Decrement(ref receiving);
        }

        FrameBuffer[] eventQueue = new FrameBuffer[256];
        // frameQueue per element lock
        // A thread tries to lock a frameQueue element by writing 1 to the correspondent frameQueueLock element. If the previous value was already 1, lock fails and the thread starts over.
        // To release the lock, the thread writes 0.
        int[] eventQueueLock = new int[256];
        byte frameWritePos;
        byte frameReadPos;
        byte eventReadPos;
        AutoResetEvent frameQueueReady;
        int flushingFrameNum = -1; // if >= 0, we are flushing since the frame with this number: process the queue w/o delays until this frame encountered
        static FrameBuffer nullFrame = new FrameBuffer();
        // The queue of frames guaranteed to be processed.
        // These are currently only video config frames sent reliably w/o fragmentation.
        // Event queue processor can drop a config frame if it's delivered later than its neighbours.
        // Config frames are rare (usually 1 in decoder lifetime), we can use a dynamic queue for them.
        ConcurrentQueue<FrameBuffer> configFrameQueue = new ConcurrentQueue<FrameBuffer>();
        bool started = false;

        // buffers for fragmented frames assembly
        FragmentedPoolSlot[] fragmentedPool = new FragmentedPoolSlot[10];

        // FEC events are processed in a sepearate queue to avod timing and decoding issues (lost FEC event cannot be distinguished from regular lost event)
        FrameBuffer[] fecQueue = new FrameBuffer[256];
        int[] fecQueueLock = new int[256];
        // every FEC event writes its event number at indexes of events it's xored from
        // it's not cleared from no more in use FEC events, so it can point to the wrong FEC event but in the worst case decoder processes corrupted frame instead missing
        byte[] fecXoredEvents = new byte[256];
        const int FEC_EVENT_TIMEOUT_INF = 127;
        // number of events since last FEC event, used to optimize FEC events presence check
        byte fecEventTimeout = FEC_EVENT_TIMEOUT_INF;

        // Keep already processed frames for FEC until the read pointer is ahead by that many slots.
        // Should be > FEC events injection period
        const int QUEUE_CLEAR_LAG = 64;

        // A simple way to ensure that the delay is at least 1 frame if the stream has fragmented frames: the flag set once and never reset even if the sender stops sending fragmented frames.
        // The first fragmented frame still may be processed too early with partial loss if DelayFrames is 0.
        bool fragmentDetected;

        private void decodeQueue()
        {
            int df = 0; // the delay between frame writer and reader.
            if (flushingFrameNum < 0) // the delay is always 0 if flushing
            {
                if (DelayFrames > 0)
                {
                    df = DelayFrames > 127 ? 127 : DelayFrames; // leave half of the buffer for write/read jitter, 127 is ~1,5 sec. of video (kfi = 30, 30 fragments per kf) or 2.5-7.6 sec. of audio (20-60ms)
                }
                else
                {
                    df = fragmentDetected ? 1 : 0; // at least 1 frame delay required to ensure that all fragments have time to arrive
                }
            }

            int nullFramesCnt = 0; // to avoid infinite loop when read frame position does not advance for some reason
            while (!disposed && nullFramesCnt++ < 10 && (byte)(frameWritePos - frameReadPos) > df)
            {
                while (configFrameQueue.TryDequeue(out FrameBuffer confFrame))
                {
                    options.Decoder.Input(ref confFrame);
                    confFrame.Release();
                }

                if (flushingFrameNum == frameReadPos)
                {
                    // the frame is flushing, the next frame will be processed with delay
                    flushingFrameNum = -1;
                }

                byte eventReadPosPrev = eventReadPos;
                byte frameReadPosPrev = frameReadPos;
                eventReadPos += processFrame(eventReadPos);

                if (frameReadPosPrev != frameReadPos)
                {
                    nullFramesCnt = 0;
                }

                for (byte i = eventReadPosPrev; i != eventReadPos; i++)
                {
                    // clear the main event queue
                    byte clearSlot = (byte)(i - QUEUE_CLEAR_LAG);
                    while (Interlocked.Exchange(ref eventQueueLock[clearSlot], 1) == 1) ;          // lock single slot for cleaning
                    eventQueue[clearSlot].Release();
                    eventQueue[clearSlot] = nullFrame;
                    Interlocked.Exchange(ref eventQueueLock[clearSlot], 0);                        // unlock single slot

                    // clear FEC event queue
                    while (Interlocked.Exchange(ref fecQueueLock[clearSlot], 1) == 1) ;       // lock single slot for cleaning
                    fecQueue[clearSlot].Release();
                    fecQueue[clearSlot] = nullFrame;
                    Interlocked.Exchange(ref fecQueueLock[clearSlot], 0);                     // unlock single slot
                }
            }
        }

        void processLostEvent(byte lostEvNum, ref FrameBuffer lostEv)
        {
            var fecEvNum = fecXoredEvents[lostEvNum];
            while (Interlocked.Exchange(ref fecQueueLock[fecEvNum], 1) == 1) ; // lock single FEC event slot
            ref FrameBuffer fecEv = ref fecQueue[fecEvNum];
            if (fecEv.IsFEC) // FEC event exists
            {
                if (recoverLostEvent(lostEvNum, ref lostEv, fecEvNum, ref fecEv)) // puts recovered event in lost event's slot via ref f
                {
                    this.voiceClient.FramesRecovered++;
                }
            }
            else
            {
                this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " FEC failed to recover because of non-FEC event in FEC events lookup array at index " + fecEvNum + " (" + (fecEv.Array == null ? "empty" : "flags: " + fecEv.Flags) + ")");
            }
            Interlocked.Exchange(ref fecQueueLock[fecEvNum], 0);               // unlock single FEC event slot
        }

        bool recoverLostEvent(byte lostEvNum, ref FrameBuffer lostEv, byte fecEvNum, ref FrameBuffer fecEv)
        {
            this.voiceClient.FramesTryFEC++;
            // see sendFrameEvent():
            // [..., flags, size_lsb, size_msb, xor_start_ev]
            var last = fecEv.Offset + fecEv.Length;
            byte frNumber = fecEv.Array[last - 5];
            FrameFlags flags = (FrameFlags)fecEv.Array[last - 4];
            int size = fecEv.Array[last - 3] + (fecEv.Array[last - 2] << 8);
            byte from = fecEv.Array[last - 1];

            // lock all events required for xor
            // end event number = fecEvNum - 1 (see sendFrameEvent)
            for (byte i = from; i != fecEvNum; i++)
            {
                if (i != lostEvNum) // all but lost
                {
                    while (Interlocked.Exchange(ref eventQueueLock[i], 1) == 1) ; // lock single slot for reading

                    if (eventQueue[i].Array == null) // another lost, can't recover: unlock all and abort
                    {
                        for (byte j = from; j != (byte)(i + 1); j++)
                        {
                            if (j != lostEvNum) // all but lost
                            {
                                Interlocked.Exchange(ref eventQueueLock[j], 0);       // unlock single slot
                            }
                        }
                        this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " FEC failed to recover from events " + from + "-" + fecEvNum + " because at least 2 events are lost");

return false;
                    }
                }
            }

            // xor directly into FEC event buffer and unlock xored frames
            for (byte i = from; i != fecEvNum; i++)
            {
                if (i != lostEvNum) // all but lost
                {
                    var xf = eventQueue[i];
                    for (int j = 0; j < xf.Length; j++)
                    {
                        fecEv.Array[fecEv.Offset + j] ^= xf.Array[xf.Offset + j];
                    }
                    flags ^= xf.Flags;
                    frNumber ^= xf.FrameNum;
                    size -= xf.Length;
                    Interlocked.Exchange(ref eventQueueLock[i], 0);                   // unlock single slot
                }
            }

            if (size >= 0 && size <= fecEv.Length)
            {
                // move FEC event with recovered data to the lost event's slot...
                lostEv = new FrameBuffer(fecEv, fecEv.Offset, size, flags, frNumber);
                // ... from FEC event slot in FEC queue
                fecEv = nullFrame;
                this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " fr#" + lostEv.FrameNum + " FEC recovered from events " + from + "-" + fecEvNum + ", size: " + +size);
                return true;
            }
            else
            {
                this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " FEC failed to recover from FEC event of size " + fecEv.Length + " because of wrong resulting size " + size);
                return false;
            }
        }

        // returns the number of events we have advanced
        // the caller passes 'eventReadPos' field to this method and updates it with returned value for clarity
        byte processFrame(byte begEvNum)
        {
            while (Interlocked.Exchange(ref eventQueueLock[begEvNum], 1) == 1) ; // lock frame 1st event
            ref FrameBuffer begEv = ref eventQueue[begEvNum];

            // try to recover lost event if we had FEC events recenty
            if (begEv.Array == null && fecEventTimeout < FEC_EVENT_TIMEOUT_INF)
            {
                processLostEvent(begEvNum, ref begEv);
            }

            if (begEv.Array == null)
            {
                this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + begEv.FrameNum + " wr#" + frameWritePos + " rd#" + frameReadPos + " lost event");
                Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);           // unlock frame 1st event
                this.voiceClient.EventsLost++;

return 1;
            }

            // send null buffer to the decoder for missing frames
            if (frameReadPos != begEv.FrameNum) // frameReadPos may be updated already by an earlier event with the same frame number
            {
                for (frameReadPos++; frameReadPos != begEv.FrameNum; frameReadPos++)
                {
                    this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + begEv.FrameNum + " wr#" + frameWritePos + " rd#" + frameReadPos + " missing frame");
                    options.Decoder.Input(ref nullFrame);
                    this.voiceClient.FramesLost++;
                }
            }

            FrameFlags fragMask = (begEv.Flags & FrameFlags.MaskFrag);

            if (fragMask == FrameFlags.FragNotEnd)
            {
                // assemble fragmented
                // scan and lock fragments, move read pointer at the last read slot
                fragmentDetected = true;
                bool partial = false; // some fragments lost
                byte fragCount = begEv.Array[begEv.Offset + begEv.Length - 1]; // the count of fragments is in the last byte
                if (fragCount == 0)
                {
                    this.voiceClient.logger.LogWarning(LogPrefix + " ev#" + begEvNum + " fr#" + begEv.FrameNum + " c#" + fragCount + " 1st event corrupted: 0 fragments count");
                    Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);           // unlock frame 1st event

return 1;
                }

                int begEvPayloadSize = begEv.Length - 1; // - last byte with count, all fragments but the last are of this size
                int maxPayloadSize = begEvPayloadSize * fragCount;

                byte[] fragmented; // assemble to this buffer
                int poolIdx = 0;
                // if decoder retains the buffer, we take another one from the pool
                for (; poolIdx < fragmentedPool.Length; poolIdx++)
                {
                    if (fragmentedPool[poolIdx] == null || fragmentedPool[poolIdx].IsFree)
                    {
                        break;
                    }
                }
                if (poolIdx == fragmentedPool.Length) // not found, the decoder retained too many frames, that's strange
                {
                    voiceClient.logger.LogError(LogPrefix + " Fragmented pool is full, allocating " + maxPayloadSize + " bytes directly");
                    fragmented = new byte[maxPayloadSize];
                }
                else  if (fragmentedPool[poolIdx] == null || fragmentedPool[poolIdx].Buf.Length < maxPayloadSize)  // reallocate buffer if needed
                {
                    fragmented = new byte[maxPayloadSize];
                }
                else // reuse
                {
                    fragmented = fragmentedPool[poolIdx].Buf;
                }

                // read 1st fragment
                Array.Copy(begEv.Array, begEv.Offset, fragmented, 0, begEvPayloadSize);
                Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);           // unlock frame 1st event

                int payloadSize = begEvPayloadSize;
                // read all fragments, fill the buffer with 0s if a lost or unfragmented event is encountered, in the hope that the decoder can still get something useful from the partial frame rather than crashing on it
                for (byte fragEvNum = (byte)(begEvNum + 1), i = 1; i != fragCount; fragEvNum++, i++)
                {
                    this.voiceClient.FramesReceivedFragments++;

                    while (Interlocked.Exchange(ref eventQueueLock[fragEvNum], 1) == 1) ; // lock fragment slot
                    ref FrameBuffer fragEv = ref eventQueue[fragEvNum];

                    // try to recover lost event if we had FEC events recenty
                    if (fragEv.Array == null && fecEventTimeout < FEC_EVENT_TIMEOUT_INF)
                    {
                        processLostEvent(fragEvNum, ref fragEv);
                    }

                    if (fragEv.FrameNum == begEv.FrameNum && (fragEv.Flags & FrameFlags.FragNotBeg) != 0) // intermediate or last fragment
                    {
                        int fragEvLength = fragEv.Length < begEvPayloadSize ? fragEv.Length : begEvPayloadSize; // normally all lenghts are 'begEvPayloadSize' except for the last which can be smaller
                        Array.Copy(fragEv.Array, fragEv.Offset, fragmented, payloadSize, fragEvLength);
                        payloadSize += fragEvLength;
                    }
                    else
                    {
                        // either lost event or the event not from this frame, fill the buffer with 0s
                        // note: if we are here with the last fragment, the size of the frame will be wrong
                        partial = true;
                        Array.Clear(fragmented, payloadSize, begEvPayloadSize);
                        payloadSize += begEvPayloadSize;
                        this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + begEv.FrameNum + " c#" + fragCount + " Fragmented segment zeroed due to invalid fragment ev#" + fragEvNum + " fr#" + fragEv.FrameNum + ", flags:" + fragEv.Flags + (fragEv.Array == null ? " NULL" : ""));
                    }

                    Interlocked.Exchange(ref eventQueueLock[fragEvNum], 0);       // unlock fragment slot
                }

                IDisposable disposer = null;
                if (poolIdx != fragmentedPool.Length) // store in the pool
                {
                    if (fragmentedPool[poolIdx] == null)
                    {
                        fragmentedPool[poolIdx] = new FragmentedPoolSlot();
                    }
                    fragmentedPool[poolIdx].Buf = fragmented; // if we reuse the buffer, 'Buf' already stores 'fragmented' but the call is still important because it resets IsFree
                    disposer = fragmentedPool[poolIdx];
                }

                FrameBuffer fragFrame = new FrameBuffer(fragmented, 0, payloadSize, begEv.Flags, begEv.FrameNum, disposer);

                this.voiceClient.FramesReceivedFragmented++;
                if (partial)
                {
                    this.voiceClient.FramesFragPart++;
                }
                this.voiceClient.logger.LogDebug(LogPrefix + " DEC ev#" + begEvNum + " fr#" + fragFrame.FrameNum + " c#" + fragCount + " Fragmented assembled from events " + begEvNum + "-" + (byte)(begEvNum + fragCount - 1) + ", size: " + payloadSize + ", flags: " + begEv.Flags);

                options.Decoder.Input(ref fragFrame);
                fragFrame.Release();

return fragCount;
            }
            else if (fragMask == 0) // unfragemented
            {
                options.Decoder.Input(ref begEv);
            }
            else // unexected fragment
            {
                // this.voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + begEv.FrameNum + " wr#" + frameWritePos + " Unexpected Fragment" + ", flags: " + begEv.Flags);
                // we get here when the 1st fragment is lost
                // TODO: we could skip to the last event of continuous fragments segment (end of the frame in the best case) to avoid repeatedly calling processFrame() on each fragment
                this.voiceClient.EventsLost++;
            }

            Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);           // unlock frame 1st event

return 1;
        }

        void decodeThread()
        {
            lock (disposeLock)
            {
                if (disposed)
                {
                    return;
                }
                decoding = true;
            }

            voiceClient.logger.LogInfo(LogPrefix + ": Starting decode thread");
            frameQueueReady = new AutoResetEvent(false);
            try
            {
#if UNITY_ANDROID
                UnityEngine.AndroidJNI.AttachCurrentThread();
#endif
                this.options.Decoder.Open(Info);

                while (!disposed)
                {
                    decodeQueue();
                    frameQueueReady.WaitOne(); // Wait until data is pushed to the queue or Dispose signals.
                }
            }
            catch (Exception e)
            {
                voiceClient.logger.LogError(LogPrefix + ": Exception in decode thread: " + e);
                decoding = false;
                Dispose();
            }
            finally
            {
#if UNITY_ANDROID
                UnityEngine.AndroidJNI.DetachCurrentThread();
#endif
                voiceClient.logger.LogInfo(LogPrefix + ": Exiting decode thread");
            }

            decoding = false;
        }

        internal void removeAndDispose()
        {
            if (options.OnRemoteVoiceRemoveAction != null)
            {
                options.OnRemoteVoiceRemoveAction();
            }
            Dispose();
        }

        public void Dispose()
        {
            lock (disposeLock)
            {
                if (disposed)
                {
                    return;
                }
                disposed = true;
            }

            if (frameQueueReady != null)
            {
                frameQueueReady.Set(); // let decodeThread exit
            }

            while (receiving > 0 || decoding)
            {
                // we may need this in case of exception in decoder because current event slot is locked while decoding
                Array.Clear(eventQueueLock, 0, eventQueueLock.Length);
                Array.Clear(fecQueueLock, 0, fecQueueLock.Length);
            }

            // no concurrent threads below this line

            // Closing requires synchronization with frameQueueReady.Set() to avoid 'ObjectDisposedException: Safe handle has been closed'.
            // On the other hand, we can simply drop all references to the wait handle and allow the garbage collector to do the job for you sometime later (wait handles implement the disposal pattern whereby the finalizer calls Close).
            // This is one of the few scenarios where relying on this backup is (arguably)acceptable, because wait handles have a light OS burden (asynchronous delegates rely on exactly this mechanism to release their IAsyncResult’s wait handle):
            // https://www.cnblogs.com/malaikuangren/archive/2012/06/02/2532239.html
//            if (frameQueueReady != null)
//            {
//#if NETFX_CORE
//                frameQueueReady.Dispose();
//#else
//                frameQueueReady.Close();
//#endif
//            }

            for (int i = 0; i < eventQueue.Length; i++)
            {
                eventQueue[i].Release();
                eventQueue[i] = nullFrame;
            }

            for (int i = 0; i < fecQueue.Length; i++)
            {
                fecQueue[i].Release();
                fecQueue[i] = nullFrame;
            }

            this.options.Decoder.Dispose();
        }

        // Serves as a FrameBuffer disposer to detect when its buffer is freed
        class FragmentedPoolSlot : IDisposable
        {
            public bool IsFree { get; private set; }
            public byte[] Buf
            {
                get => buf;
                set
                {
                    buf = value;
                    IsFree = false;
                }
            }
            private byte[] buf;
            public void Dispose()
            {
                IsFree = true;
            }
        }
    }
}