using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "AudioSource", Abstract = true, Description = "Represents an AudioSource component for playing audio clips.", IsComponent = true)]
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

        [CLProperty("Volume of the sound.")]
        public float Volume
        {
            get => Value.volume;
            set => Value.volume = value;
        }

        [CLProperty("Sound playback position.")]
        public float Time
        {
            get => Value.time;
            set => Value.time = value;
        }

        [CLProperty("Pitch of the sound.")]
        public float Pitch
        {
            get => Value.pitch;
            set => Value.pitch = value;
        }

        [CLProperty("Is the sound currently playing.")]
        public bool IsPlaying => Value.isPlaying;

        [CLMethod("Plays the sound.")]
        public void Play() => Value.Play();

        [CLMethod("Plays the sound after n seconds.")]
        public void PlayDelayed(
            [CLParam("The delay in seconds before playing the sound.")]
            float seconds)
            => Value.PlayDelayed(seconds);

        [CLMethod("Stops the sound.")]
        public void Stop() => Value.Stop();

        [CLMethod("Pauses the sound.")]
        public void Pause() => Value.Pause();

        [CLMethod("Unpauses the sound.")]
        public void Unpause() => Value.UnPause();
    }
}
