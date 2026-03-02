using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace CustomLogic
{
    /// <summary>
    /// Represents a VideoPlayer component for playing video clips.
    /// </summary>
    [CLType(Name = "VideoPlayer", Abstract = true, IsComponent = true)]
    partial class CustomLogicVideoPlayerBuiltin : BuiltinComponentInstance
    {
        public VideoPlayer Value;
        public BuiltinClassInstance OwnerBuiltin;

        [CLConstructor]
        public CustomLogicVideoPlayerBuiltin() : base(null) { }

        public CustomLogicVideoPlayerBuiltin(BuiltinClassInstance owner, VideoPlayer videoPlayer) : base(videoPlayer)
        {
            OwnerBuiltin = owner;
            Value = (VideoPlayer)Component;
        }

        /// <summary>
        /// The frame index. 0 for the first frame, 1 for the second frame, and so on.
        /// </summary>
        [CLProperty]
        public int Frame
        {
            set => Value.frame = (int)value;
            get => (int)Value.frame;
        }

        /// <summary>
        /// Number of frames in the current video content.
        /// </summary>
        [CLProperty]
        public ulong FrameCount => Value.frameCount;

        /// <summary>
        /// The frame rate of the video content in frames/second.
        /// </summary>
        [CLProperty]
        public float FrameRate => Value.frameRate;

        /// <summary>
        /// Whether the VideoPlayer restarts when reaches the end.
        /// </summary>
        [CLProperty]
        public bool IsLooping
        {
            set => Value.isLooping = value; 
            get => Value.isLooping;
        }

        /// <summary>
        /// Is the video currently paused.
        /// </summary>
        [CLProperty]
        public bool IsPaused => Value.isPaused;

        /// <summary>
        /// Is the video currently playing.
        /// </summary>
        [CLProperty]
        public bool IsPlaying => Value.isPlaying;

        /// <summary>
        /// Has the video been prepared to play.
        /// </summary>
        [CLProperty]
        public bool IsPrepared => Value.isPrepared;

        /// <summary>
        /// The length of the video in seconds. 
        /// </summary>
        [CLProperty]
        public double Length => Value.length;

        /// <summary>
        /// Factor by which the basic playback rate will be multiplied.
        /// </summary>
        [CLProperty]
        public float PlaybackSpeed
        {
            set => Value.playbackSpeed = value;
            get => Value.playbackSpeed;
        }

        /// <summary>
        /// Video playback position in seconds.
        /// </summary>
        [CLProperty]
        public float Time
        {
            set => Value.time = (float)value;
            get => (float)Value.time;
        }

        /// <summary>
        /// Pauses the video.
        /// </summary>
        [CLMethod]
        public void Pause() => Value.Pause();

        /// <summary>
        /// Plays or resumes the video.
        /// </summary>
        [CLMethod]
        public void Play() => Value.Play();

        /// <summary>
        /// Prepares the video so that it's ready for playback.
        /// </summary>
        [CLMethod]
        public void Prepare() => Value.Prepare();

        /// <summary>
        /// Advance the current time by one frame.
        /// </summary>
        [CLMethod]
        public void StepForward() => Value.StepForward();

        /// <summary>
        /// Stops the video and sets the current time to 0.
        /// </summary>
        [CLMethod]
        public void Stop() => Value.Stop();
    }
}