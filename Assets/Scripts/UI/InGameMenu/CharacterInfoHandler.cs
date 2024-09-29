using System.Collections.Generic;
using UnityEngine;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using Utility;
using Cameras;
using Assets.Scripts.Utility;

namespace UI
{
    class CharacterInfoHandler : MonoBehaviour
    {
        protected HashSet<SetItem<BaseCharacter, CharacterInfoPopup>> _characterInfoPopups = new HashSet<SetItem<BaseCharacter, CharacterInfoPopup>>();
        //protected Dictionary<BaseCharacter, CharacterInfoPopup> _characterInfoPopups = new Dictionary<BaseCharacter, CharacterInfoPopup>();
        protected const float HumanRange = 500f;
        protected const float TitanRange = 250f;
        protected const float HumanOffset = 2f;
        protected const float TitanOffset = 20f;
        protected const float CrawlerOffset = 5f;
        protected const float ShifterOffset = 57f;
        protected Color RedColor = new Color(0.455f, 0.094f, 0.094f);
        protected Color GreenColor = new Color(0.106f, 0.368f, 0.086f);
        protected LayerMask CullMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities, PhysicsLayer.TitanMovebox);
        private InGameManager _inGameManager;
        private SetItem<BaseCharacter, CharacterInfoPopup> mockEntry = new SetItem<BaseCharacter, CharacterInfoPopup>(null, null);

        private void Awake()
        {
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
        }

        protected void LateUpdate()
        {
            RefreshDict();
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            bool inMenu = InGameMenu.InMenu() || ((InGameManager)SceneLoader.CurrentGameManager).State == GameState.Loading;
            ShowMode showNameMode = (ShowMode)SettingsManager.UISettings.ShowNames.Value;
            ShowMode showHealthMode = (ShowMode)SettingsManager.UISettings.ShowHealthbars.Value;
            bool highlyVisible = SettingsManager.UISettings.HighVisibilityNames.Value;
            foreach (SetItem<BaseCharacter, CharacterInfoPopup> kv in _characterInfoPopups)
            {
                var character = kv.Key;
                var popup = kv.Value;
                bool showName = (showNameMode == ShowMode.All || (showNameMode == ShowMode.Mine && character.IsMainCharacter()) ||
                    (showNameMode == ShowMode.Others && !character.IsMainCharacter()));
                bool showHealth = (showHealthMode == ShowMode.All || (showHealthMode == ShowMode.Mine && character.IsMainCharacter()) ||
                    (showHealthMode == ShowMode.Others && !character.IsMainCharacter()));
                bool toggleName = showName && !character.AI && !(character is BasicTitan);
                bool toggleHealth = showHealth && character.MaxHealth > 1 && character.CurrentHealth < character.MaxHealth;
                if (_inGameManager.Restarting || SettingsManager.InGameCurrent.Misc.RealismMode.Value || (character.IsMainCharacter() && camera.GetCameraDistance() <= 0f))
                    toggleName = toggleHealth = false;
                if ((!toggleName && !toggleHealth) || inMenu)
                {
                    popup.HideImmediate();
                    continue;
                }
                Vector3 worldPosition = character.Cache.Transform.position + popup.Offset;
                float range = popup.Range;
                float distance = Vector3.Distance(camera.Cache.Transform.position, worldPosition);
                if (distance > range && !highlyVisible)
                {
                    popup.Hide();
                    if (popup.gameObject.activeSelf)
                        popup.transform.position = camera.Camera.WorldToScreenPoint(worldPosition);
                    continue;
                }
                var direction = (worldPosition - camera.Cache.Transform.position).normalized;
                if (Vector3.Angle(camera.Cache.Transform.forward, direction) > 90f)
                {
                    popup.HideImmediate();
                    continue;
                }
                if (!character.IsMainCharacter() && Physics.Raycast(camera.Cache.Transform.position, direction, distance, CullMask))
                {
                    popup.HideImmediate();
                    continue;
                }
                if (toggleHealth)
                {
                    popup.ToggleHealthbar(true);
                    Color color = character.IsMainCharacter() ? GreenColor : RedColor;
                    popup.SetHealthbar(character.CurrentHealth, character.MaxHealth, color);
                }
                else
                    popup.ToggleHealthbar(false);
                if (toggleName)
                {
                    popup.ToggleName(true);
                    string name = character.Name;
                    if (character.Guild != "")
                        name = character.Guild + "\n" + name;
                    if (highlyVisible)
                        name = name.ForceWhiteColorTag();
                    popup.SetName(name);
                }
                else
                    popup.ToggleName(false);
                Vector3 screenPosition = camera.Camera.WorldToScreenPoint(worldPosition);
                popup.transform.position = screenPosition;
                if (highlyVisible)
                    popup.ShowImmediate();
                else
                    popup.Show();
            }
        }

        protected CharacterInfoPopup CreateInfoPopup(BaseCharacter character)
        {
            Vector3 offset = Vector3.zero;
            float range = HumanRange;
            if (character is Human)
            {
                offset = Vector3.up * HumanOffset;
                range = HumanRange;
            }
            else if (character is BasicTitan)
            {
                var titan = (BasicTitan)character;
                if (titan.IsCrawler)
                    offset = Vector3.up * CrawlerOffset * titan.Size;
                else
                    offset = Vector3.up * TitanOffset * titan.Size;
                range = TitanRange;
            }
            else if (character is BaseShifter)
            {
                offset = Vector3.up * ShifterOffset * ((BaseShifter)character).Size;
                range = TitanRange;
            }
            var popup = ElementFactory.InstantiateAndSetupPanel<CharacterInfoPopup>(transform, "Prefabs/InGame/CharacterInfoPopup").GetComponent<CharacterInfoPopup>();
            popup.Load(character, offset, range);
            return popup;
        }

        protected void RefreshDict()
        {
            _characterInfoPopups.RemoveWhere(e =>
            {
                if (!e.Key || e.Key.Dead)
                {
                    Destroy(e.Value.gameObject);
                    return true;
                }
                return false;
            });

            
            foreach (var character in _inGameManager.GetAllCharactersEnumerable())
            {
                mockEntry.Key = character;
                if (!_characterInfoPopups.Contains(mockEntry) && !character == false && character.Dead == false)
                {
                    var popup = CreateInfoPopup(character);
                    _characterInfoPopups.Add(new SetItem<BaseCharacter, CharacterInfoPopup>(character, popup));
                }
            }
        }
    }
}
