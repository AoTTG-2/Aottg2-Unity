using UnityEngine;
using System.Collections;
using Utility;
using System.Collections.Generic;
using Events;
using SimpleJSONFixed;
using Settings;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using GameManagers;
using Cameras;
using Characters;

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
        private const float BattleFadeInTime = 1f;
        private const float DefaultFadeOutTime = 1f;
        private const float DefaultFadeInTime = 5f;
        private int _currentSong;
        private List<string> _customPlaylist = new List<string>();
        private string _currentSongName;
        public static bool _muted = false;
        public bool _isDefaultPlaylist = false;
        private float _transitionTimeLeft = 0f;
        private bool _isMenuTransition = false;
        private const float BattleTitanAnyDistance = 200f;
        private const float BattleTitanActiveDistance = 1000f;
        private const float BattleOtherAnyDistance = 500f;

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
            else if (sceneName == SceneName.InGame)
                SetPlaylist(MusicPlaylist.Default);
        }

        public static void ApplySoundSettings()
        {
            if (_instance == null)
                return;
            if (!_instance._isFading)
                _instance._audio.volume = _instance._songVolume * GetMusicVolume();
            if (SettingsManager.SoundSettings.ForcePlaylist.Value != MusicPlaylist.Default)
                SetPlaylist(SettingsManager.SoundSettings.ForcePlaylist.Value);
        }

        public static void PlayDeathSong()
        {
            if (!_instance._isDefaultPlaylist)
                return;
            var songInfo = _musicInfo["Death"][0];
            _instance._isMenuTransition = false;
            PlayImmediateTransition(songInfo);
        }

        public static void PlayGrabbedSong()
        {
            if (!SettingsManager.SoundSettings.TitanGrabMusic.Value)
                return;
            var songInfo = _musicInfo["Grabbed"][0];
            _instance._isMenuTransition = false;
            PlayImmediateTransition(songInfo);
        }

        public static void OnEscapeGrab()
        {
            if (_instance._transitionTimeLeft > 0f)
            {
                _instance._transitionTimeLeft = 0f;
                _instance._songTimeLeft = 0f;
            }
        }

        public static void PlayEffect()
        {
            var songInfo = _musicInfo["Effect"];
            PlaySoundEffect(songInfo[Random.Range(0, songInfo.Count)]);
        }

        public static void PlayTransition()
        {
            var songInfo = _musicInfo["Transition"];
            _instance._isMenuTransition = true;
            PlayImmediateTransition(songInfo[Random.Range(0, songInfo.Count)]);
        }

        public static void SetPlaylist(string playlist)
        {
            if (SettingsManager.SoundSettings.ForcePlaylist.Value != MusicPlaylist.Default)
            {
                playlist = SettingsManager.SoundSettings.ForcePlaylist.Value;
                _instance._customPlaylist = new List<string>(SettingsManager.SoundSettings.CustomPlaylist.Value.Split(','));
            }
            if (playlist == MusicPlaylist.Default)
            {
                _instance._isDefaultPlaylist = true;
                playlist = MusicPlaylist.Ambient;
            }
            else
                _instance._isDefaultPlaylist = false;
            FinishSetPlaylist(playlist);
        }

        private static void FinishSetPlaylist(string playlist, bool forceNext = false)
        {
            bool change = _instance._currentPlaylist != playlist;
            _instance._currentPlaylist = playlist;
            if (_instance._isDefaultPlaylist || (!_instance._isMenuTransition && _instance._transitionTimeLeft > 0f))
                _instance._songTimeLeft = 0f;
            if (!_instance._isMenuTransition || playlist == MusicPlaylist.Battle)
                _instance._transitionTimeLeft = 0f;
            if (_instance._transitionTimeLeft <= 0f && (change || forceNext))
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
                    _instance._transitionTimeLeft = clip.length;
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
                    var asset = ResourceManager.LoadAsset("Music", songInfo["Name"]);
                    if (asset != null)
                        clip = (AudioClip)asset;
                    volume = songInfo["Volume"];
                    _instance._currentSongName = songInfo["Name"];
                }
                else
                {
                    var playlist = _musicInfo[(string)songInfo["Playlist"]];
                    songInfo = playlist[Random.Range(0, playlist.Count)];
                    var asset = ResourceManager.LoadAsset("Music", songInfo["Name"]);
                    if (asset != null)
                        clip = (AudioClip)asset;
                    volume = songInfo["Volume"];
                    _instance._currentSongName = songInfo["Name"];
                }
            }
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.FadeNextSong(clip, volume));
        }

        public static void ChatNextSong()
        {
            _instance._songTimeLeft = 0f;
            _instance._transitionTimeLeft = 0f;
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
            float fadeInTime = DefaultFadeInTime;
            float fadeOutTime = DefaultFadeOutTime;
            if (_instance._currentPlaylist == MusicPlaylist.Battle)
                fadeInTime = BattleFadeInTime;
            // fade out
            _isFading = true;
            float fadeTimeLeft = fadeOutTime;
            if (_audio.isPlaying)
            {
                _songVolume = _audio.volume;
                while (fadeTimeLeft > 0f)
                {
                    float lerp = fadeTimeLeft / fadeOutTime;
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
            _songTimeLeft = nextClip.length - fadeOutTime;
            _songVolume = volume;
            _autoPlay = true;

            // fade in
            fadeTimeLeft = fadeInTime;
            while (fadeTimeLeft > 0f)
            {
                float lerp = 1f - fadeTimeLeft / fadeInTime;
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
            _transitionTimeLeft -= Time.deltaTime;
            if (_isDefaultPlaylist)
            {
                if (_currentPlaylist != MusicPlaylist.Battle)
                {
                    if ((_transitionTimeLeft <= 0f || _instance._isMenuTransition) && ShouldPlayBattleMusic())
                        FinishSetPlaylist(MusicPlaylist.Battle);
                }
                if (_songTimeLeft <= 0f && _autoPlay && _transitionTimeLeft <= 0f)
                {
                    if (ShouldPlayBattleMusic())
                        FinishSetPlaylist(MusicPlaylist.Battle, true);
                    else if (_currentPlaylist == MusicPlaylist.Battle)
                        FinishSetPlaylist(MusicPlaylist.Ambient, true);
                    else
                        FinishSetPlaylist(MusicPlaylist.Peaceful, true);
                }
            }
            else if (_songTimeLeft <= 0f && _autoPlay && _transitionTimeLeft <= 0f)
                NextSong();
        }

        private bool ShouldPlayBattleMusic()
        {
            if (SceneLoader.SceneName == SceneName.InGame && SceneLoader.CurrentCamera != null && SceneLoader.CurrentGameManager != null)
            {
                if (SceneLoader.CurrentCamera is InGameCamera && SceneLoader.CurrentGameManager is InGameManager)
                {
                    var follow = ((InGameCamera)SceneLoader.CurrentCamera)._follow;
                    var inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
                    if (follow == null)
                        return false;
                    foreach (var character in inGameManager.GetAllCharacters())
                    {
                        if (!TeamInfo.SameTeam(character, follow))
                        {
                            var distance = Vector3.Distance(character.Cache.Transform.position, follow.Cache.Transform.position);
                            if (character is BasicTitan)
                            {
                                if (distance < BattleTitanAnyDistance)
                                    return true;
                                if (distance < BattleTitanActiveDistance && ((BasicTitan)character).TargetViewId > 0)
                                    return true;
                            }
                            else
                            {
                                if (distance < BattleOtherAnyDistance)
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
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
        public static string Default = "Default";
        public static string Peaceful = "Peaceful";
        public static string Ambient = "Ambient";
        public static string Battle = "Battle";
        public static string Boss = "Boss";
        public static string Racing = "Racing";
    }
}
