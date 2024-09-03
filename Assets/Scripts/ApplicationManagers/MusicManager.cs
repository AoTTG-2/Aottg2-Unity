using UnityEngine;
using System.Collections;
using Utility;
using System.Collections.Generic;
using Events;
using SimpleJSONFixed;
using Settings;
using System.Runtime.CompilerServices;

namespace ApplicationManagers
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager _instance;
        private static JSONNode _musicInfo;
        private string _currentPlaylist;
        private AudioSource _audio;
        private GameObject _soundEffectObject;
        private AudioSource _soundEffect;
        private float _songTimeLeft;
        private float _songVolume;
        private bool _autoPlay;
        private bool _isFading;
        private const float FadeTime = 1.5f;
        private int _currentSong;
        public float _deathSongTimeLeft;
        private List<string> _customPlaylist = new List<string>();
        private string _currentSongName;
        public static bool _muted = false;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnLoadScene += OnLoadScene;
            _instance._audio = _instance.gameObject.AddComponent<AudioSource>();
            _instance._audio.ignoreListenerVolume = true;

            // Add sound effects obj and audiosource
            _instance._soundEffectObject = new GameObject("SoundEffect");
            _instance._soundEffectObject.transform.SetParent(_instance.transform);
            _instance._soundEffect = _instance._soundEffectObject.AddComponent<AudioSource>();


            _musicInfo = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.Info, "MusicInfo"));
        }

        private static void OnLoadScene(SceneName sceneName)
        {
            if (sceneName == SceneName.MainMenu || sceneName == SceneName.MapEditor || 
                sceneName == SceneName.CharacterEditor || sceneName == SceneName.SnapshotViewer || sceneName == SceneName.Gallery || 
                sceneName == SceneName.Credits)
                SetPlaylist(MusicPlaylist.Menu);
        }

        public static void ApplySoundSettings()
        {
            if (_instance == null)
                return;
            if (!_instance._isFading)
                _instance._audio.volume = _instance._songVolume * GetMusicVolume();
            if (SettingsManager.SoundSettings.ForcePlaylist.Value != "Default")
                SetPlaylist(SettingsManager.SoundSettings.ForcePlaylist.Value);
        }

        public static void PlayDeathSong()
        {
            if (!SettingsManager.SoundSettings.TitanGrabMusic.Value)
                return;
            var songInfo = _musicInfo["Death"][0];
            PlayImmediateTransition(songInfo);
        }

        public static void PlayEffect()
        {
            var songInfo = _musicInfo["Effect"];
            PlaySoundEffect(songInfo[Random.Range(0, songInfo.Count)]);
        }

        public static void PlayTransition()
        {
            var songInfo = _musicInfo["Transition"];
            PlayImmediateTransition(songInfo[Random.Range(0, songInfo.Count)]);
        }

        public static void StopPlaylist()
        {
            _instance._currentPlaylist = string.Empty;
        }

        public static void StopSong()
        {
            SetSong(string.Empty);
        }

        public static void SetPlaylist(string playlist, bool forceNext = false)
        {
            if (SettingsManager.SoundSettings.ForcePlaylist.Value != "Default")
            {
                playlist = SettingsManager.SoundSettings.ForcePlaylist.Value;
                _instance._customPlaylist = new List<string>(SettingsManager.SoundSettings.CustomPlaylist.Value.Split(','));
            }
            bool change = _instance._currentPlaylist != playlist;
            _instance._currentPlaylist = playlist;
            if (forceNext || change)
            {
                _instance._currentSong = 0;
                NextSong();
            }
        }

        public static void SetSong(string song)
        {
            var songInfo = FindSong(song);
            SetSong(songInfo);
        }

        private static void PlaySoundEffect(JSONNode songInfo)
        {
            AudioClip clip = null;
            float volume = 0f;
            if (songInfo != null)
            {
                if (songInfo.HasKey("Name"))
                {
                    clip = (AudioClip)ResourceManager.LoadAsset("Music", songInfo["Name"]);
                    volume = songInfo["Volume"];

                    _instance.StartCoroutine(_instance.StartSoundEffect(clip, volume));
                }
            }            
        }

        private static void PlayImmediateTransition(JSONNode songInfo)
        {
            AudioClip clip = null;
            float volume = 0f;
            if (songInfo != null)
            {
                if (songInfo.HasKey("Name"))
                {
                    clip = (AudioClip)ResourceManager.LoadAsset("Music", songInfo["Name"]);
                    volume = songInfo["Volume"];
                    _instance._audio.clip = clip;
                    _instance._audio.volume = volume * GetMusicVolume();
                    _instance._audio.Play();
                    _instance._songTimeLeft = clip.length;
                }
            }
        }

        private IEnumerator StartSoundEffect(AudioClip clip, float volume)
        {
            _soundEffect.volume = volume;
            _soundEffect.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }

        public static void SetSong(JSONNode songInfo)
        {
            _instance._autoPlay = false;
            AudioClip clip = null;
            float volume = 0f;
            _instance._currentSongName = "";
            if (songInfo != null)
            {
                if (songInfo.HasKey("Name"))
                {
                    clip = (AudioClip)ResourceManager.LoadAsset("Music", songInfo["Name"]);
                    volume = songInfo["Volume"];
                    _instance._currentSongName = songInfo["Name"];
                }
                else
                {
                    var playlist = _musicInfo[(string)songInfo["Playlist"]];
                    songInfo = playlist[Random.Range(0, playlist.Count)];
                    clip = (AudioClip)ResourceManager.LoadAsset("Music", songInfo["Name"]);
                    volume = songInfo["Volume"];
                    _instance._currentSongName = songInfo["Name"];
                }
            }
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.FadeNextSong(clip, volume));
        }

        public static void NextSong()
        {
            if (_instance._currentPlaylist == string.Empty)
                return;
            if (_instance._currentPlaylist == "Custom")
            {
                if (_instance._customPlaylist.Count == 0)
                    return;
                _instance._currentSong++;
                if (_instance._currentSong >= _instance._customPlaylist.Count)
                    _instance._currentSong = 0;
                SetSong(FindSong(_instance._customPlaylist[_instance._currentSong]));
            }
            else
            {
                JSONNode playlist = _musicInfo[_instance._currentPlaylist];
                JSONNode songInfo;
                if (_instance._currentPlaylist.EndsWith("Ordered"))
                {
                    songInfo = playlist[_instance._currentSong];
                    _instance._currentSong += 1;
                    if (_instance._currentSong >= playlist.Count)
                        _instance._currentSong = 0;
                }
                else
                    songInfo = playlist[Random.Range(0, playlist.Count)];
                SetSong(songInfo);
            }
        }

        private static JSONNode FindSong(string name)
        {
            if (name == string.Empty)
                return null;
            foreach (JSONNode playlist in _musicInfo.Values)
            {
                foreach (JSONNode song in playlist)
                {
                    if (song.HasKey("Name") && song["Name"] == name)
                        return song;
                }
            }
            return null;
        }

        private IEnumerator FadeNextSong(AudioClip nextClip, float volume)
        {
            // fade out
            _isFading = true;
            float fadeTimeLeft = FadeTime;
            if (_audio.isPlaying)
            {
                _songVolume = _audio.volume;
                while (fadeTimeLeft > 0f)
                {
                    float lerp = fadeTimeLeft / FadeTime;
                    _audio.volume = lerp * _songVolume;
                    fadeTimeLeft -= 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }
                _audio.Stop();
            }
            if (nextClip == null)
            {
                _songTimeLeft = 0f;
                _autoPlay = true;
                yield break;
            }

            // set song
            _audio.clip = nextClip;
            _audio.volume = 0f;
            _audio.Play();
            _songTimeLeft = nextClip.length - FadeTime;
            _songVolume = volume;
            _autoPlay = true;

            // fade in
            fadeTimeLeft = FadeTime;
            while (fadeTimeLeft > 0f)
            {
                float lerp = 1f - fadeTimeLeft / FadeTime;
                _audio.volume = lerp * _songVolume * GetMusicVolume();
                fadeTimeLeft -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            _audio.volume = _songVolume * GetMusicVolume();
            _isFading = false;
        }

        private void Update()
        {
            _songTimeLeft -= Time.deltaTime;
            _deathSongTimeLeft -= Time.deltaTime;
            if (_songTimeLeft <= 0f && _autoPlay && _deathSongTimeLeft <= 0f)
                NextSong();
        }

        private static float GetMusicVolume()
        {
            return !_muted ? SettingsManager.SoundSettings.Music.Value * 0.4f : 0;
        }

        public static string GetCurrentSong()
        {
            if (_instance._currentSongName == "")
                return "None";
            return _instance._currentSongName;
        }

        public static List<string> GetAllSongs()
        {
            HashSet<string> songs = new HashSet<string>();
            foreach (JSONNode playlist in _musicInfo.Values)
            {
                foreach (JSONNode song in playlist)
                {
                    if (song.HasKey("Name"))
                        songs.Add(song["Name"]);
                }
            }
            return new List<string>(songs);
        }

        public static void PlaySoundOneShot(AudioSource source)
        {
            if (source == null)
                return;

            // This gets played when loading into a new scene, mark the source as DoNotDestroy
            DontDestroyOnLoad(source.gameObject);
            source.PlayOneShot(source.clip);

            // Destroy the source after the clip has finished playing
            //Destroy(source.gameObject, source.clip.length);

        }
    }

    public class MusicPlaylist
    {
        public static string Menu = "Menu";
        public static string Default = "Default.Ordered";
        public static string Peaceful = "Peaceful";
        public static string Battle = "Battle";
        public static string Boss = "Boss";
        public static string Racing = "Racing";
    }
}
