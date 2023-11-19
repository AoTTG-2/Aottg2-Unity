using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using System.Collections;

namespace UI
{
    class ItemHandler : MonoBehaviour
    {
        public static List<string> AvailableItems = new List<string>() { "Flare1", "Flare2", "Flare3" };
        private BasePopup _itemWheelPopup;
        public bool IsActive;
        private InGameManager _inGameManager;


        private void Awake()
        {
            _itemWheelPopup = ElementFactory.InstantiateAndSetupPanel<WheelPopup>(transform, "Prefabs/InGame/WheelMenu").GetComponent<BasePopup>();
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
        }

        private void Start()
        {
            StartCoroutine(UpdateForever(1f));
        }

        public void ToggleItemWheel()
        {
            SetItemWheel(!IsActive);
        }

        public void SetItemWheel(bool enable)
        {
            if (enable)
            {
                if (!InGameMenu.InMenu())
                {
                    ((WheelPopup)_itemWheelPopup).Show(SettingsManager.InputSettings.Interaction.ItemMenu.ToString(),
                        GetItemWheelOptions(), () => OnItemWheelSelect());
                    IsActive = true;
                }
            }
            else
            {
                _itemWheelPopup.Hide();
                IsActive = false;
            }
        }

        private IEnumerator UpdateForever(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                if (IsActive)
                {
                    ((WheelPopup)_itemWheelPopup).Show(SettingsManager.InputSettings.Interaction.ItemMenu.ToString(),
                        GetItemWheelOptions(), () => OnItemWheelSelect());
                }
            }
        }

        private void OnItemWheelSelect()
        {
            BaseCharacter character = _inGameManager.CurrentCharacter;
            int selected = ((WheelPopup)_itemWheelPopup).SelectedItem;
            if (character != null && selected < character.Items.Count)
            {
                character.UseItem(selected);
            }
            _itemWheelPopup.Hide();
            IsActive = false;
        }

        private List<string> GetItemWheelOptions()
        {
            BaseCharacter character = _inGameManager.CurrentCharacter;
            List<string> items = new List<string>();
            if (character != null)
            {
                foreach (var item in character.Items)
                {
                    string name = item.Name;
                    if (item.MaxUses != -1)
                        name += " (" + item.UsesLeft.ToString() + ")";
                    else if (item.GetCooldownLeft() > 0f)
                        name += " (" + ((int)item.GetCooldownLeft()).ToString() + ")";
                    items.Add(name);
                }
            }
            return items;
        }
    }
}
