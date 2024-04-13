namespace Photon.Voice.Unity.Demos.DemoVoiceUI
{
    using Realtime;
    using ExitGames.Client.Photon;

    public static partial class PhotonDemoExtensions // todo: USE C.A.S. ALWAYS
    {
        // this demo uses a Custom Property (as explained in the Realtime API), to sync if a player muted her microphone. that value needs a string key.
        internal const string MUTED_KEY      = "mu";
        internal const string PHOTON_VAD_KEY = "pv";
        internal const string WEBRTC_AEC_KEY = "ec";
        internal const string WEBRTC_VAD_KEY = "wv";
        internal const string WEBRTC_AGC_KEY = "gc";
        internal const string MIC_KEY        = "m";

        public static bool Mute(this Player player)
        {
            return player.SetCustomProperties(new Hashtable(1) { { MUTED_KEY, true } });
        }

        public static bool Unmute(this Player player)
        {
            return player.SetCustomProperties(new Hashtable(1) { { MUTED_KEY, false } });
        }

        public static bool IsMuted(this Player player)
        {
            return player.HasBoolProperty(MUTED_KEY);
        }

        public static bool SetPhotonVAD(this Player player, bool value)
        {
            return player.SetCustomProperties(new Hashtable(1) { { PHOTON_VAD_KEY, value } });
        }
        
        public static bool SetWebRTCVAD(this Player player, bool value)
        {
            return player.SetCustomProperties(new Hashtable(1) { { WEBRTC_VAD_KEY, value } });
        }


        public static bool SetAEC(this Player player, bool value)
        {
            return player.SetCustomProperties(new Hashtable(1) { { WEBRTC_AEC_KEY, value } });
        }

        public static bool SetAGC(this Player player, bool agcEnabled, int gain, int level)
        {
            return player.SetCustomProperties(new Hashtable(1) { { WEBRTC_AGC_KEY, new object[] { agcEnabled, gain,level} } });
        }

        public static bool SetMic(this Player player, Recorder.MicType type)
        {
            return player.SetCustomProperties(new Hashtable(1) { { MIC_KEY, type } } );
        }


        public static bool HasPhotonVAD(this Player player)
        {
            return player.HasBoolProperty(PHOTON_VAD_KEY);
        }

        public static bool HasWebRTCVAD(this Player player)
        {
            return player.HasBoolProperty(WEBRTC_VAD_KEY);
        }

        public static bool HasAEC(this Player player)
        {
            return player.HasBoolProperty(WEBRTC_AEC_KEY);
        }

        public static bool HasAGC(this Player player)
        {
            var agc = player.GetObjectProperty(WEBRTC_AGC_KEY) as object[];
            return agc != null && agc.Length > 0 ? (bool)agc[0] : false;
        }

        public static int GetAGCGain(this Player player)
        {
            var agc = player.GetObjectProperty(WEBRTC_AGC_KEY) as object[];
            return agc != null && agc.Length > 1 ? (int)agc[1] : 0;
        }

        public static int GetAGCLevel(this Player player)
        {
            var agc = player.GetObjectProperty(WEBRTC_AGC_KEY) as object[];
            return agc != null && agc.Length > 2 ? (int)agc[2] : 0;
        }

        public static Recorder.MicType? GetMic(this Player player)
        {
            Recorder.MicType? mic = null;
            try
            {
                mic = (Recorder.MicType)player.GetObjectProperty(MIC_KEY);
            }
            catch {
                mic = null;
            }

            return mic;
        }

        private static bool HasBoolProperty(this Player player, string prop)
        {
            object temp;
            return player.CustomProperties.TryGetValue(prop, out temp) && (bool)temp;
        }

        private static int? GetIntProperty(this Player player, string prop)
        {
            object temp;
            if (player.CustomProperties.TryGetValue(prop, out temp))
                return (int)temp;
            return null;
        }

        private static object GetObjectProperty(this Player player, string prop)
        {
            object temp;
            if (player.CustomProperties.TryGetValue(prop, out temp))
                return temp;
            return null;
        }

    }

}