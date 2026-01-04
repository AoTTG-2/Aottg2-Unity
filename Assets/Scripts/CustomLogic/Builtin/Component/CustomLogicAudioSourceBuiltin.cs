using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents an AudioSource component for playing audio clips.
    /// </summary>
    [CLType(Name = "AudioSource", Abstract = true, IsComponent = true)]
    partial class CustomLogicAudioSourceBuiltin : BuiltinComponentInstance
    {
        public AudioSource Value;
        public BuiltinClassInstance OwnerBuiltin;

        [CLConstructor]
        public CustomLogicAudioSourceBuiltin() : base(null) { }

        public CustomLogicAudioSourceBuiltin(BuiltinClassInstance owner, AudioSource audioSource) : base(audioSource)
        {
            OwnerBuiltin = owner;
            Value = (AudioSource)Component;
        }

        /// <summary>
        /// Volume of the sound.
        /// </summary>
        [CLProperty]
        public float Volume
        {
            get => Value.volume;
            set => Value.volume = value;
        }

        /// <summary>
        /// Sound playback position.
        /// </summary>
        [CLProperty]
        public float Time
        {
            get => Value.time;
            set => Value.time = value;
        }

        /// <summary>
        /// Pitch of the sound.
        /// </summary>
        [CLProperty]
        public float Pitch
        {
            get => Value.pitch;
            set => Value.pitch = value;
        }

        /// <summary>
        /// Is the sound currently playing.
        /// </summary>
        [CLProperty]
        public bool IsPlaying => Value.isPlaying;

        /// <summary>
        /// Plays the sound.
        /// </summary>
        [CLMethod]
        public void Play() => Value.Play();

        /// <summary>
        /// Plays the sound after n seconds.
        /// </summary>
        /// <param name="seconds">The delay in seconds before playing the sound.</param>
        [CLMethod]
        public void PlayDelayed(float seconds)
            => Value.PlayDelayed(seconds);

        /// <summary>
        /// Stops the sound.
        /// </summary>
        [CLMethod]
        public void Stop() => Value.Stop();

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        [CLMethod]
        public void Pause() => Value.Pause();

        /// <summary>
        /// Unpauses the sound.
        /// </summary>
        [CLMethod]
        public void Unpause() => Value.UnPause();
    }
}
