using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using Utility;
using Photon.Pun;

namespace UI
{
    class EmoteHandler : MonoBehaviour
    {
        public static Dictionary<string, Texture2D> EmojiTextures = new Dictionary<string, Texture2D>();
        public static List<string> AvailableEmojis = new List<string>() { "Smile", "ThumbsUp", "Cool", "Love", "Shocked", "Crying", "Annoyed", "Angry" };
        public static List<string> AvailableText = new List<string>() { "Help", "Thanks", "Sorry", "Titan here", "Good game", "Nice hit", "Oops", "Welcome" };
        private List<EmoteTextPopup> _emoteTextPopups = new List<EmoteTextPopup>();
        private List<EmoteTextPopup> _emoteEmojiPopups = new List<EmoteTextPopup>();
        private BasePopup _emoteWheelPopup;
        private EmoteWheelState _currentEmoteWheelState = EmoteWheelState.Text;
        private float _currentEmoteCooldown;
        public const float EmoteCooldown = 4f;
        public bool IsActive;
        private InGameManager _inGameManager;
        protected const float Range = 500f;
        protected const float HumanOffset = 4f;
        protected const float TitanOffset = 25f;
        protected const float CrawlerOffset = 15f;
        protected const float ShifterOffset = 70f;
        protected const float ShowTime = 3f;
        protected LayerMask CullMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities, PhysicsLayer.TitanMovebox);

        private void Awake()
        {
            for (int i = 0; i < 5; i++)
            {
                EmoteTextPopup emoteTextPopup = ElementFactory.InstantiateAndSetupPanel<EmoteTextPopup>(transform, "Prefabs/InGame/EmoteTextPopup").GetComponent<EmoteTextPopup>();
                _emoteTextPopups.Add(emoteTextPopup);
                EmoteEmojiPopup emoteEmojiPopup = ElementFactory.InstantiateAndSetupPanel<EmoteEmojiPopup>(transform, "Prefabs/InGame/EmoteEmojiPopup").GetComponent<EmoteEmojiPopup>();
                _emoteEmojiPopups.Add(emoteEmojiPopup);
            }
            _emoteWheelPopup = ElementFactory.InstantiateAndSetupPanel<WheelPopup>(transform, "Prefabs/InGame/WheelMenu").GetComponent<BasePopup>();
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
        }

        public static void OnEmoteTextRPC(int viewId, string text, PhotonMessageInfo info)
        {
            if (UIManager.CurrentMenu == null || !SettingsManager.UISettings.ShowEmotes.Value || InGameManager.MuteEmote.Contains(info.Sender.ActorNumber))
                return;
            EmoteHandler handler = UIManager.CurrentMenu.GetComponent<EmoteHandler>();
            BaseCharacter c = Util.FindCharacterByViewId(viewId);
            if (c != null && handler != null)
                handler.ShowEmoteText(text, c);
        }

        public static void OnEmoteEmojiRPC(int viewId, string emoji, PhotonMessageInfo info)
        {
            if (UIManager.CurrentMenu == null || !SettingsManager.UISettings.ShowEmotes.Value || InGameManager.MuteEmote.Contains(info.Sender.ActorNumber))
                return;
            EmoteHandler handler = UIManager.CurrentMenu.GetComponent<EmoteHandler>();
            BaseCharacter c = Util.FindCharacterByViewId(viewId);
            if (c != null && handler != null)
                handler.ShowEmoteEmoji(emoji, c);
        }

        private void ShowEmoteText(string text, BaseCharacter character)
        {
            EmoteTextPopup popup = (EmoteTextPopup)GetAvailablePopup(_emoteTextPopups);
            if (text.Length > 20)
                text = text.Substring(0, 20);
            popup.Load(text, ShowTime, character, GetOffset(character));
        }

        private void ShowEmoteEmoji(string emoji, BaseCharacter character)
        {
            EmoteEmojiPopup popup = (EmoteEmojiPopup)GetAvailablePopup(_emoteEmojiPopups);
            popup.Load(emoji, ShowTime, character, GetOffset(character));
        }

        private Vector3 GetOffset(BaseCharacter character)
        {
            if (character is Human)
                return Vector3.up * HumanOffset;
            else if (character is BasicTitan)
            {
                var titan = (BasicTitan)character;
                if (titan.IsCrawler)
                    return Vector3.up * CrawlerOffset * titan.Size;
                else
                    return Vector3.up * TitanOffset * titan.Size;
            }
            else if (character is BaseShifter)
                return Vector3.up * ShifterOffset * ((BaseShifter)character).Size;
            return Vector3.zero;
        }

        public void ToggleEmoteWheel()
        {
            SetEmoteWheel(!IsActive);
        }

        public void SetEmoteWheel(bool enable)
        {
            if (enable)
            {
                if (!InGameMenu.InMenu())
                {
                    ((WheelPopup)_emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(),
                        GetEmoteWheelOptions(_currentEmoteWheelState), () => OnEmoteWheelSelect());
                    IsActive = true;
                }
            }
            else
            {
                _emoteWheelPopup.Hide();
                IsActive = false;
            }
        }

        public void NextEmoteWheel()
        {
            if (!_emoteWheelPopup.gameObject.activeSelf || !IsActive)
                return;
            _currentEmoteWheelState++;
            if (_currentEmoteWheelState > EmoteWheelState.Action)
                _currentEmoteWheelState = 0;
            ((WheelPopup)_emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(),
                    GetEmoteWheelOptions(_currentEmoteWheelState), () => OnEmoteWheelSelect());
        }

        private void OnEmoteWheelSelect()
        {
            if (_currentEmoteWheelState != EmoteWheelState.Action)
            {
                if (_currentEmoteCooldown > 0f)
                {
                    _emoteWheelPopup.Hide();
                    IsActive = false;
                    ((InGameMenu)UIManager.CurrentMenu).SkipAHSSInput = true;
                    return;
                }
                _currentEmoteCooldown = EmoteCooldown;
            }
            BaseCharacter character = _inGameManager.CurrentCharacter;
            int selected = ((WheelPopup)_emoteWheelPopup).SelectedItem;
            if (character != null)
            {
                if (_currentEmoteWheelState == EmoteWheelState.Text)
                {
                    if (selected < AvailableText.Count)
                    {
                        string text = AvailableText[selected];
                        RPCManager.PhotonView.RPC("EmoteTextRPC", RpcTarget.All, new object[] { character.Cache.PhotonView.ViewID, text });
                    }
                }
                else if (_currentEmoteWheelState == EmoteWheelState.Emoji)
                {
                    if (selected < AvailableEmojis.Count)
                    {
                        string emoji = AvailableEmojis[selected];
                        RPCManager.PhotonView.RPC("EmoteEmojiRPC", RpcTarget.All, new object[] { character.Cache.PhotonView.ViewID, emoji });
                    }
                }
                else if (_currentEmoteWheelState == EmoteWheelState.Action)
                {
                    if (selected < character.EmoteActions.Count)
                    {
                        string action = character.EmoteActions[selected];
                        character.Emote(action);
                    }
                }
            }
            _emoteWheelPopup.Hide();
            IsActive = false;
            ((InGameMenu)UIManager.CurrentMenu).SkipAHSSInput = true;
        }

        private List<string> GetEmoteWheelOptions(EmoteWheelState state)
        {
            if (state == EmoteWheelState.Text)
                return AvailableText;
            else if (state == EmoteWheelState.Emoji)
                return AvailableEmojis;
            else
            {
                if (_inGameManager.CurrentCharacter == null)
                    return new List<string>();
                return _inGameManager.CurrentCharacter.EmoteActions;
            }
        }

        private EmoteTextPopup GetAvailablePopup(List<EmoteTextPopup> popups)
        {
            foreach (EmoteTextPopup popup in popups)
            {
                if (!popup.gameObject.activeSelf)
                    return popup;
            }
            return popups[0];
        }

        protected void UpdatePopup(EmoteTextPopup popup, bool inMenu)
        {
            var camera = SceneLoader.CurrentCamera;
            popup.ShowTimeLeft -= Time.deltaTime;
            if (popup.ShowTimeLeft <= 0f || inMenu || popup.Character == null)
            {
                popup.HideImmediate();
                return;
            }
            Vector3 worldPosition = popup.Character.Cache.Transform.position + popup.Offset;
            float distance = Vector3.Distance(camera.Cache.Transform.position, worldPosition);
            if (distance > Range)
            {
                popup.Hide();
                if (popup.gameObject.activeSelf)
                    popup.transform.position = camera.Camera.WorldToScreenPoint(worldPosition);
                return;
            }
            var direction = (worldPosition - camera.Cache.Transform.position).normalized;
            if (Vector3.Angle(camera.Cache.Transform.forward, direction) > 90f)
            {
                popup.HideImmediate();
                return;
            }
            if (!popup.Character.IsMainCharacter() && Physics.Raycast(camera.Cache.Transform.position, direction, distance, CullMask))
            {
                popup.HideImmediate();
                return;
            }
            Vector3 screenPosition = camera.Camera.WorldToScreenPoint(worldPosition);
            popup.transform.position = screenPosition;
            popup.Show();
        }

        protected void LateUpdate()
        {
            _currentEmoteCooldown -= Time.deltaTime;
            bool inMenu = InGameMenu.InMenu() || ((InGameManager)SceneLoader.CurrentGameManager).State == GameState.Loading;
            foreach (var popup in _emoteTextPopups)
                UpdatePopup(popup, inMenu);
            foreach(var popup in _emoteEmojiPopups)
                UpdatePopup(popup, inMenu);
        }
    }

    public enum EmoteWheelState
    {
        Text,
        Emoji,
        Action
    }
}
