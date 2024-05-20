using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using Utility;
using Cameras;

namespace UI
{
    class HUDBottomHandler : MonoBehaviour
    {
        private Color FillLowColor = new Color(1f, 0.5f, 0f, 0.5f);
        private Color BackgroundLowColor = new Color(1f, 0.75f, 0.5f);
        private Color FillVeryLowColor = new Color(1f, 0f, 0f, 0.5f);
        private Color BackgroundVeryLowColor = new Color(1f, 0.5f, 0.5f);
        private Color FillNormalColor = new Color(1f, 1f, 1f, 0.5f);
        private Color BladeFillNormalColor = new Color(1f, 1f, 1f, 0.75f);
        private Color BackgroundNormalColor = Color.white;
        private Color BackgroundEmptyColor = new Color(1f, 0.25f, 0.25f);
        private GameObject _hudBottom;

        private Image _specialFill;
        private Image _specialIconBackground;
        private Image _specialIconFill;

        private Image _specialFill_2; // added by ata 20 May 2024 for Ability Wheel //
        private Image _specialIconBackground_2; // added by ata 20 May 2024 for Ability Wheel //
        private Image _specialIconFill_2; // added by ata 20 May 2024 for Ability Wheel //

        private Image _specialFill_3; // added by ata 20 May 2024 for Ability Wheel //
        private Image _specialIconBackground_3; // added by ata 20 May 2024 for Ability Wheel //
        private Image _specialIconFill_3; // added by ata 20 May 2024 for Ability Wheel //

        private Image _gasFillLeft;
        private Image _gasFillRight;
        private Image _gasBackground;

        private string _currentSpecialIcon = "";
        private string _newSpecialIcon = "";

        private string _currentSpecialIcon_2 = ""; // added by ata 20 May 2024 for Ability Wheel //
        private string _newSpecialIcon_2 = ""; // added by ata 20 May 2024 for Ability Wheel //

        private string _currentSpecialIcon_3 = ""; // added by ata 20 May 2024 for Ability Wheel //
        private string _newSpecialIcon_3 = ""; // added by ata 20 May 2024 for Ability Wheel //

        private Human _human;
        private float _gasAnimationTimeLeft = 0f;
        private float _reloadAnimationTimeLeft = 0f;
        private float _shootAnimationTimeLeft = 0f;

        // blades
        private Image _bladeFillLeft;
        private Image _bladeFillRight;
        private Image _bladeBackground;
        private Image _bladeReload;
        private Image _bladeOut;
        private GameObject _bladeAmmoGroupLeft;
        private GameObject _bladeAmmoGroupRight;
        private List<GameObject> _bladeAmmoLeft;
        private List<GameObject> _bladeAmmoRight;

        // ammo
        private Image _ammoFillLeft;
        private Image _ammoFillRight;
        private Text _ammoLabelLeft;
        private Text _ammoLabelRight;

        // apg
        private Image _apgBackground;
        private Image _apgReload;
        private Image _apgShoot;
        private Image _apgAmmoBackground;

        // ahss
        private Image _ahssBackgroundLeft;
        private Image _ahssBackgroundRight;
        private Image _ahssEmptyLeft;
        private Image _ahssEmptyRight;
        private Image _ahssReloadLeft;
        private Image _ahssReloadRight;
        private Image _ahssShootLeft;
        private Image _ahssShootRight;

        // thunderspear
        private Image _tsBackground;
        private Image _tsReload;
        private Image _tsShoot;
        private Image _tsBombBackground;
        private Image _tsBombReload;
        private Image _bombFillLeft;
        private Image _bombFillRight;
        private bool _bombInCooldown;

        public void SetBottomHUD(Human myHuman = null)
        {
            _human = myHuman;
            if (_hudBottom != null)
                Destroy(_hudBottom);
            if (_human == null)
                return;
            if (_human.Setup.Weapon == HumanWeapon.APG)
            {
                _hudBottom = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/HUDBottomAPG");
                _apgBackground = _hudBottom.transform.Find("GunBackground").GetComponent<Image>();
                _ammoFillLeft = _hudBottom.transform.Find("GunAmmoFillLeft").GetComponent<Image>();
                _ammoFillRight = _hudBottom.transform.Find("GunAmmoFillRight").GetComponent<Image>();
                _apgReload = _hudBottom.transform.Find("GunReload").GetComponent<Image>();
                _apgShoot = _hudBottom.transform.Find("GunShoot").GetComponent<Image>();
                _apgAmmoBackground = _hudBottom.transform.Find("GunAmmoBackground").GetComponent<Image>();
                _ammoLabelLeft = _hudBottom.transform.Find("GunAmmoLabelLeft").GetComponent<Text>();
                _ammoLabelRight = _hudBottom.transform.Find("GunAmmoLabelRight").GetComponent<Text>();
                _apgReload.color = BackgroundVeryLowColor;
            }
            else if (_human.Setup.Weapon == HumanWeapon.AHSS)
            {
                _hudBottom = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/HUDBottomAHSS");
                _ahssBackgroundLeft = _hudBottom.transform.Find("GunBackgroundLeft").GetComponent<Image>();
                _ahssBackgroundRight = _hudBottom.transform.Find("GunBackgroundRight").GetComponent<Image>();
                _ahssEmptyLeft = _hudBottom.transform.Find("GunEmptyLeft").GetComponent<Image>();
                _ahssEmptyRight = _hudBottom.transform.Find("GunEmptyRight").GetComponent<Image>();
                _ahssReloadLeft = _hudBottom.transform.Find("GunReloadLeft").GetComponent<Image>();
                _ahssReloadRight = _hudBottom.transform.Find("GunReloadRight").GetComponent<Image>();
                _ahssShootLeft = _hudBottom.transform.Find("GunShootLeft").GetComponent<Image>();
                _ahssShootRight = _hudBottom.transform.Find("GunShootRight").GetComponent<Image>();
                _ahssReloadLeft = _hudBottom.transform.Find("GunReloadLeft").GetComponent<Image>();
                _ahssReloadRight = _hudBottom.transform.Find("GunReloadRight").GetComponent<Image>();
                _ammoLabelLeft = _hudBottom.transform.Find("GunAmmoLabelLeft").GetComponent<Text>();
                _ammoLabelRight = _hudBottom.transform.Find("GunAmmoLabelRight").GetComponent<Text>();
                _ahssEmptyLeft.color = BackgroundVeryLowColor;
                _ahssEmptyRight.color = BackgroundVeryLowColor;
                _ahssReloadLeft.color = BackgroundVeryLowColor;
                _ahssReloadRight.color = BackgroundVeryLowColor;
                _ahssShootLeft.color = BackgroundLowColor;
                _ahssShootRight.color = BackgroundLowColor;
            }
            else if (_human.Setup.Weapon == HumanWeapon.Thunderspear)
            {
                _hudBottom = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/HUDBottomTS");
                _tsBackground = _hudBottom.transform.Find("TSBackground").GetComponent<Image>();
                _tsBombBackground = _hudBottom.transform.Find("TSBombBackground").GetComponent<Image>();
                _ammoFillLeft = _hudBottom.transform.Find("AmmoFillLeft").GetComponent<Image>();
                _ammoFillRight = _hudBottom.transform.Find("AmmoFillRight").GetComponent<Image>();
                _bombFillLeft = _hudBottom.transform.Find("BombFillLeft").GetComponent<Image>();
                _bombFillRight = _hudBottom.transform.Find("BombFillRight").GetComponent<Image>();
                _tsReload = _hudBottom.transform.Find("TSReload").GetComponent<Image>();
                _tsBombReload = _hudBottom.transform.Find("TSBombReload").GetComponent<Image>();
                _tsShoot = _hudBottom.transform.Find("TSShoot").GetComponent<Image>();
                _ammoLabelLeft = _hudBottom.transform.Find("AmmoLabelLeft").GetComponent<Text>();
                _ammoLabelRight = _hudBottom.transform.Find("AmmoLabelRight").GetComponent<Text>();
                _tsReload.color = BackgroundVeryLowColor;
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                {
                    _tsBackground.gameObject.SetActive(false);
                    _tsBombBackground.gameObject.SetActive(true);
                    _ammoFillLeft.gameObject.SetActive(false);
                    _ammoFillRight.gameObject.SetActive(false);
                    _bombFillLeft.gameObject.SetActive(true);
                    _bombFillRight.gameObject.SetActive(true);
                    _tsReload.gameObject.SetActive(false);
                    _tsShoot.gameObject.SetActive(false);
                    _ammoLabelLeft.gameObject.SetActive(false);
                    _ammoLabelRight.gameObject.SetActive(false);
                }
            }
            else
            {
                _hudBottom = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/HUDBottomBlade");
                _bladeBackground = _hudBottom.transform.Find("BladeBackground").GetComponent<Image>();
                _bladeReload = _hudBottom.transform.Find("BladeReload").GetComponent<Image>();
                _bladeOut = _hudBottom.transform.Find("BladeOut").GetComponent<Image>();
                _bladeFillLeft = _hudBottom.transform.Find("BladeFillLeft").GetComponent<Image>();
                _bladeFillRight = _hudBottom.transform.Find("BladeFillRight").GetComponent<Image>();
                _bladeAmmoLeft = new List<GameObject>();
                _bladeAmmoRight = new List<GameObject>();
                _bladeAmmoGroupLeft = _hudBottom.transform.Find("BladeAmmoGroupLeft").gameObject;
                _bladeAmmoGroupRight = _hudBottom.transform.Find("BladeAmmoGroupRight").gameObject;
                _bladeOut.color = BackgroundVeryLowColor;
            }
            _hudBottom.AddComponent<HUDScaler>();
            ElementFactory.SetAnchor(_hudBottom, TextAnchor.LowerCenter, TextAnchor.LowerCenter, Vector3.up * 10f);

            _specialFill = _hudBottom.transform.Find("SpecialFill").GetComponent<Image>();
            _specialIconBackground = _hudBottom.transform.Find("SpecialIconBackground").GetComponent<Image>();
            _specialIconFill = _hudBottom.transform.Find("SpecialIconFill").GetComponent<Image>();

            _specialFill_2 = _hudBottom.transform.Find("SpecialFill_2").GetComponent<Image>(); // added by ata 20 May 2024 for Ability Wheel //
            _specialIconBackground_2 = _hudBottom.transform.Find("SpecialIconBackground_2").GetComponent<Image>(); // added by ata 20 May 2024 for Ability Wheel //
            _specialIconFill_2 = _hudBottom.transform.Find("SpecialIconFill_2").GetComponent<Image>(); // added by ata 20 May 2024 for Ability Wheel //
 
            _specialFill_3 = _hudBottom.transform.Find("SpecialFill_3").GetComponent<Image>(); // added by ata 20 May 2024 for Ability Wheel //
            _specialIconBackground_3 = _hudBottom.transform.Find("SpecialIconBackground_3").GetComponent<Image>(); // added by ata 20 May 2024 for Ability Wheel //
            _specialIconFill_3 = _hudBottom.transform.Find("SpecialIconFill_3").GetComponent<Image>(); // added by ata 20 May 2024 for Ability Wheel //

            _gasFillLeft = _hudBottom.transform.Find("GasFillLeft").GetComponent<Image>();
            _gasFillRight = _hudBottom.transform.Find("GasFillRight").GetComponent<Image>();
            _gasBackground = _hudBottom.transform.Find("GasBackground").GetComponent<Image>();
            
            _currentSpecialIcon = "";
            _currentSpecialIcon_2 = ""; // added by ata 20 May 2024 for Ability Wheel //
            _currentSpecialIcon_3 = ""; // added by ata 20 May 2024 for Ability Wheel //
        }

        public void SetSpecialIcon(string icon)
        {
            _newSpecialIcon = icon;
        }
        public void SetSpecialIcon_2(string icon) // added by ata 20 May 2024 for Ability Wheel //
        {
            _newSpecialIcon_2 = icon;
        }
        public void SetSpecialIcon_3(string icon) // added by ata 20 May 2024 for Ability Wheel //
        {
            _newSpecialIcon_3 = icon;
        }

        public void ShakeGas()
        {
            if (_human != null && _gasBackground != null)
            {
                StartAnimator(_gasBackground);
                StartAnimator(_gasFillLeft);
                StartAnimator(_gasFillRight);
                _gasAnimationTimeLeft = 0.4f;
            }
        }

        public void Reload()
        {
            if (_human == null || _hudBottom == null)
                return;
            if (_bladeBackground != null)
            {
                _bladeOut.gameObject.SetActive(false);
                _bladeBackground.gameObject.SetActive(false);
                _bladeFillLeft.gameObject.SetActive(false);
                _bladeFillRight.gameObject.SetActive(false);
                _bladeReload.gameObject.SetActive(true);
                _bladeReload.GetComponent<Animator>().Update(0f);
                _reloadAnimationTimeLeft = 1f;
            }
            else if (_apgBackground != null)
            {
                _apgShoot.gameObject.SetActive(false);
                _apgAmmoBackground.gameObject.SetActive(false);
                _apgBackground.gameObject.SetActive(false);
                _apgReload.gameObject.SetActive(true);
                _apgReload.GetComponent<Animator>().Update(0f);
                _reloadAnimationTimeLeft = 0.8f;
                _shootAnimationTimeLeft = 0f;
            }
            else if (_ahssBackgroundLeft != null)
            {
                _ahssShootLeft.gameObject.SetActive(false);
                _ahssShootRight.gameObject.SetActive(false);
                _ahssEmptyLeft.gameObject.SetActive(false);
                _ahssEmptyRight.gameObject.SetActive(false);
                _ahssBackgroundLeft.gameObject.SetActive(false);
                _ahssBackgroundRight.gameObject.SetActive(false);
                _ahssReloadLeft.gameObject.SetActive(true);
                _ahssReloadRight.gameObject.SetActive(true);
                _ahssReloadLeft.GetComponent<Animator>().Update(0f);
                _ahssReloadRight.GetComponent<Animator>().Update(0f);
                _reloadAnimationTimeLeft = 1.333f;
                _shootAnimationTimeLeft = 0f;
            }
            else if (_tsBackground != null && !SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
            {
                _tsBackground.gameObject.SetActive(false);
                _tsShoot.gameObject.SetActive(false);
                _tsReload.gameObject.SetActive(true);
                _tsReload.GetComponent<Animator>().Update(0f);
                _ammoFillLeft.gameObject.SetActive(false);
                _ammoFillRight.gameObject.SetActive(false);
                _reloadAnimationTimeLeft = 1f;
                _shootAnimationTimeLeft = 0f;
            }
        }

        public void ShootAPG()
        {
            if (_human == null || _hudBottom == null || _apgBackground == null)
                return;
            _apgShoot.gameObject.SetActive(true);
            _apgShoot.GetComponent<Animator>().Update(0f);
            _apgAmmoBackground.gameObject.SetActive(true);
            _apgBackground.gameObject.SetActive(false);
            _apgReload.gameObject.SetActive(false);
            _shootAnimationTimeLeft = 0.5f;
            _reloadAnimationTimeLeft = 0f;
        }

        public void ShootAHSS(bool left, bool right)
        {
            if (_human == null || _hudBottom == null || _ahssBackgroundLeft == null)
                return;
            if (left)
            {
                _ahssShootLeft.gameObject.SetActive(true);
                _ahssShootLeft.GetComponent<Animator>().Update(0f);
                _ahssBackgroundLeft.gameObject.SetActive(false);
                _ahssEmptyLeft.gameObject.SetActive(false);
                _ahssReloadLeft.gameObject.SetActive(false);
            }
            if (right)
            {
                _ahssShootRight.gameObject.SetActive(true);
                _ahssShootRight.GetComponent<Animator>().Update(0f);
                _ahssBackgroundRight.gameObject.SetActive(false);
                _ahssEmptyRight.gameObject.SetActive(false);
                _ahssReloadRight.gameObject.SetActive(false);
            }
            _shootAnimationTimeLeft = 0.667f;
            _reloadAnimationTimeLeft = 0f;
        }

        public void ShootTS()
        {
            if (_human == null || _hudBottom == null || _tsBackground == null || SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                return;
            _tsShoot.gameObject.SetActive(true);
            _tsShoot.GetComponent<Animator>().Update(0f);
            _tsBackground.gameObject.SetActive(false);
            _tsReload.gameObject.SetActive(false);
            _ammoFillLeft.gameObject.SetActive(true);
            _ammoFillRight.gameObject.SetActive(true);
            _shootAnimationTimeLeft = 1f;
            _reloadAnimationTimeLeft = 0f;
        }

        private void Update()
        {
            if (_human == null)
                return;
            _reloadAnimationTimeLeft -= Time.deltaTime;
            _gasAnimationTimeLeft -= Time.deltaTime;
            _shootAnimationTimeLeft -= Time.deltaTime;
            UpdateHumanSpecial();
            UpdateHumanSpecial_2(); // added by ata 20 May 2024 for Ability Wheel //
            UpdateHumanSpecial_3(); // added by ata 20 May 2024 for Ability Wheel //
            UpdateGas();
            if (_human.Weapon is BladeWeapon)
                UpdateBlade();
            else if (_human.Weapon is APGWeapon)
                UpdateAPG();
            else if (_human.Weapon is AHSSWeapon)
                UpdateAHSS();
            else if (_human.Weapon is ThunderspearWeapon)
                UpdateTS();
        }

        private void StopAnimator(Image obj)
        {
            var animator = obj.GetComponent<Animator>();
            animator.Update(0f);
            animator.speed = 0f;
        }

        private void StartAnimator(Image obj)
        {
            var animator = obj.GetComponent<Animator>();
            if (animator.speed == 0f)
            {
                animator.Update(0f);
                animator.speed = 1f;
            }
        }

        private void UpdateHumanSpecial()
        {
            if (_human.Special == null)
            {
                _specialFill.fillAmount = 0f;
                _specialIconFill.fillAmount = 0f;
            }
            else
            {
                var ratio = _human.Special.GetCooldownRatio();
                _specialFill.fillAmount = ratio;
                _specialIconFill.fillAmount = ratio;
                if (_currentSpecialIcon != _newSpecialIcon)
                {
                    _currentSpecialIcon = _newSpecialIcon;
                    if (_currentSpecialIcon != "")
                    {
                        var icon = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Specials/" + _currentSpecialIcon, true);
                        var sprite = UnityEngine.Sprite.Create(icon, new Rect(0f, 0f, icon.width, icon.height), new Vector2(0.5f, 0.5f));
                        _specialIconBackground.sprite = sprite;
                        _specialIconFill.sprite = sprite;
                    }
                }
            }
            _specialIconBackground.gameObject.SetActive(_human.Special != null && _currentSpecialIcon != "");
            _specialIconFill.gameObject.SetActive(_human.Special != null && _currentSpecialIcon != "");
        }

        private void UpdateHumanSpecial_2() // added by ata 20 May 2024 for Ability Wheel // 
        {
            if (_human.Special_2 == null)
            {
                _specialFill_2.fillAmount = 0f;
                _specialIconFill_2.fillAmount = 0f;
            }
            else
            {
                var ratio = _human.Special_2.GetCooldownRatio();
                _specialFill_2.fillAmount = ratio;
                _specialIconFill_2.fillAmount = ratio;
                if (_currentSpecialIcon_2 != _newSpecialIcon_2)
                {
                    _currentSpecialIcon_2 = _newSpecialIcon_2;
                    if (_currentSpecialIcon_2 != "")
                    {
                        var icon = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Specials/" + _currentSpecialIcon_2, true);
                        var sprite = UnityEngine.Sprite.Create(icon, new Rect(0f, 0f, icon.width, icon.height), new Vector2(0.5f, 0.5f));
                        _specialIconBackground_2.sprite = sprite;
                        _specialIconFill_2.sprite = sprite;
                    }
                }
            }
            _specialIconBackground_2.gameObject.SetActive(_human.Special_2 != null && _currentSpecialIcon_2 != "");
            _specialIconFill_2.gameObject.SetActive(_human.Special_2 != null && _currentSpecialIcon_2 != "");
        }
        private void UpdateHumanSpecial_3() // added by ata 20 May 2024 for Ability Wheel //
        {
            if (_human.Special_3 == null)
            {
                _specialFill_3.fillAmount = 0f;
                _specialIconFill_3.fillAmount = 0f;
            }
            else
            {
                var ratio = _human.Special_3.GetCooldownRatio();
                _specialFill_3.fillAmount = ratio;
                _specialIconFill_3.fillAmount = ratio;
                if (_currentSpecialIcon_3 != _newSpecialIcon_3)
                {
                    _currentSpecialIcon_3 = _newSpecialIcon_3;
                    if (_currentSpecialIcon_3 != "")
                    {
                        var icon = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Specials/" + _currentSpecialIcon_3, true);
                        var sprite = UnityEngine.Sprite.Create(icon, new Rect(0f, 0f, icon.width, icon.height), new Vector2(0.5f, 0.5f));
                        _specialIconBackground_3.sprite = sprite;
                        _specialIconFill_3.sprite = sprite;
                    }
                }
            }
            _specialIconBackground_3.gameObject.SetActive(_human.Special_3 != null && _currentSpecialIcon_3 != "");
            _specialIconFill_3.gameObject.SetActive(_human.Special_3 != null && _currentSpecialIcon_3 != "");
        }

        private void UpdateGas()
        {
            float gasRatio;
            if (_human.MaxGas <= 0f)
                gasRatio = 0f;
            else
                gasRatio = _human.CurrentGas / _human.MaxGas;
            _gasFillLeft.fillAmount = gasRatio;
            _gasFillRight.fillAmount = gasRatio;
            bool animate = false;
            if (gasRatio <= 0f)
                _gasBackground.color = BackgroundEmptyColor;
            else if (gasRatio <= 0.15f)
            {
                _gasFillLeft.color = FillVeryLowColor;
                _gasFillRight.color = FillVeryLowColor;
                _gasBackground.color = BackgroundVeryLowColor;
                animate = true;
            }
            else if (gasRatio <= 0.3f)
            {
                _gasFillLeft.color = FillLowColor;
                _gasFillRight.color = FillLowColor;
                _gasBackground.color = BackgroundLowColor;
            }
            else
            {
                _gasFillLeft.color = FillNormalColor;
                _gasFillRight.color = FillNormalColor;
                _gasBackground.color = BackgroundNormalColor;
            }
            if (animate)
            {
                StartAnimator(_gasBackground);
                StartAnimator(_gasFillLeft);
                StartAnimator(_gasFillRight);
            }
            else if (_gasAnimationTimeLeft <= 0f)
            {
                StopAnimator(_gasBackground);
                StopAnimator(_gasFillLeft);
                StopAnimator(_gasFillRight);
            }
        }

        private void UpdateBlade()
        {
            var weapon = (BladeWeapon)_human.Weapon;
            float ratio = weapon.CurrentDurability / weapon.MaxDurability;
            if (_bladeFillLeft.gameObject.activeSelf)
            {
                _bladeFillLeft.fillAmount = _bladeFillRight.fillAmount = ratio;
                if (ratio <= 0.25f)
                {
                    _bladeFillLeft.color = FillVeryLowColor;
                    _bladeFillRight.color = FillVeryLowColor;
                    _bladeBackground.color = BackgroundVeryLowColor;
                }
                else
                {
                    _bladeFillLeft.color = BladeFillNormalColor;
                    _bladeFillRight.color = BladeFillNormalColor;
                    _bladeBackground.color = BackgroundNormalColor;
                }
            }
            if (ratio <= 0f && !_bladeOut.gameObject.activeSelf && !_bladeReload.gameObject.activeSelf)
            {
                _bladeOut.gameObject.SetActive(true);
                _bladeOut.GetComponent<Animator>().Update(0f);
                _bladeBackground.gameObject.SetActive(false);
            }
            else if (ratio > 0f && !_bladeBackground.gameObject.activeSelf && _reloadAnimationTimeLeft <= 0f)
            {
                _bladeBackground.gameObject.SetActive(true);
                _bladeReload.gameObject.SetActive(false);
                _bladeOut.gameObject.SetActive(false);
                _bladeFillLeft.gameObject.SetActive(true);
                _bladeFillRight.gameObject.SetActive(true);
                _bladeFillLeft.fillAmount = _bladeFillRight.fillAmount = ratio;
            }
            int currBladeCount = _bladeAmmoLeft.Count;
            if (currBladeCount > weapon.BladesLeft)
            {
                for (int i = 0; i < currBladeCount - weapon.BladesLeft; i++)
                {
                    Destroy(_bladeAmmoLeft[0]);
                    Destroy(_bladeAmmoRight[0]);
                    _bladeAmmoLeft.RemoveAt(0);
                    _bladeAmmoRight.RemoveAt(0);
                }
            }
            else if (currBladeCount < weapon.BladesLeft)
            {
                for (int i = 0; i < weapon.BladesLeft - currBladeCount; i++)
                {
                    {
                        _bladeAmmoLeft.Add(ElementFactory.InstantiateAndBind(_bladeAmmoGroupLeft.transform, "Prefabs/InGame/HUDBladeAmmo"));
                        var right = ElementFactory.InstantiateAndBind(_bladeAmmoGroupRight.transform, "Prefabs/InGame/HUDBladeAmmo");
                        right.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                        _bladeAmmoRight.Add(right);
                    }
                }
            }
        }

        private void UpdateAPG()
        {
            var weapon = (AmmoWeapon)_human.Weapon;
            float ratio = 0f;
            if (weapon.MaxRound > 0)
                ratio = (float)weapon.RoundLeft / (float)weapon.MaxRound;
            else if (weapon.RoundLeft == -1)
                ratio = 1f;
            _ammoFillLeft.fillAmount = _ammoFillRight.fillAmount = ratio;
            if (ratio <= 0f)
            {
                _apgBackground.color = BackgroundVeryLowColor;
                _apgAmmoBackground.color = BackgroundVeryLowColor;
                _apgShoot.color = BackgroundVeryLowColor;
            }
            else if (ratio <= 0.5f)
            {
                _apgBackground.color = BackgroundLowColor;
                _apgAmmoBackground.color = BackgroundLowColor;
                _apgShoot.color = BackgroundLowColor;
            }
            else
            {
                _apgBackground.color = BackgroundNormalColor;
                _apgAmmoBackground.color = BackgroundNormalColor;
                _apgShoot.color = BackgroundNormalColor;
            }
            _ammoLabelLeft.text = _ammoLabelRight.text = weapon.AmmoLeft.ToString();
            if (weapon.AmmoLeft == 0)
            {
                _ammoLabelLeft.color = Color.red;
                _ammoLabelRight.color = Color.red;
            }
            else
            {
                _ammoLabelLeft.color = Color.green;
                _ammoLabelRight.color = Color.green;
            }
            if (!_apgBackground.gameObject.activeSelf)
            {
                if (_reloadAnimationTimeLeft <= 0f && _shootAnimationTimeLeft <= 0f)
                {
                    _apgBackground.gameObject.SetActive(true);
                    _apgAmmoBackground.gameObject.SetActive(false);
                    _apgReload.gameObject.SetActive(false);
                    _apgShoot.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateAHSS()
        {
            var weapon = (AmmoWeapon)_human.Weapon;
            _ammoLabelLeft.text = _ammoLabelRight.text = weapon.AmmoLeft.ToString();
            if (weapon.AmmoLeft == 0)
            {
                _ammoLabelLeft.color = Color.red;
                _ammoLabelRight.color = Color.red;
            }
            else
            {
                _ammoLabelLeft.color = Color.green;
                _ammoLabelRight.color = Color.green;
            }
            if (weapon.RoundLeft == 0)
            {
                _ahssEmptyLeft.color = BackgroundVeryLowColor;
                _ahssEmptyRight.color = BackgroundVeryLowColor;
                _ahssShootLeft.color = BackgroundVeryLowColor;
                _ahssShootRight.color = BackgroundVeryLowColor;
            }
            else if (weapon.RoundLeft == 1)
            {
                _ahssEmptyLeft.color = BackgroundLowColor;
                _ahssEmptyRight.color = BackgroundLowColor;
                _ahssBackgroundLeft.color = BackgroundLowColor;
                _ahssBackgroundRight.color = BackgroundLowColor;
                _ahssShootLeft.color = BackgroundLowColor;
                _ahssShootRight.color = BackgroundLowColor;
            }
            else
            {
                _ahssBackgroundLeft.color = BackgroundNormalColor;
                _ahssBackgroundRight.color = BackgroundNormalColor;
            }
            if (_ahssEmptyLeft.gameObject.activeSelf && weapon.RoundLeft >= 2)
            {
                _ahssEmptyLeft.gameObject.SetActive(false);
                _ahssBackgroundLeft.gameObject.SetActive(true);
            }
            if (_ahssEmptyRight.gameObject.activeSelf && weapon.RoundLeft >= 1)
            {
                _ahssEmptyRight.gameObject.SetActive(false);
                _ahssBackgroundRight.gameObject.SetActive(true);
            }
            if (_ahssShootLeft.gameObject.activeSelf)
            {
                if (_shootAnimationTimeLeft <= 0f)
                {
                    _ahssShootLeft.gameObject.SetActive(false);
                    _ahssEmptyLeft.gameObject.SetActive(true);
                }
            }
            if (_ahssShootRight.gameObject.activeSelf)
            {
                if (_shootAnimationTimeLeft <= 0f)
                {
                    _ahssShootRight.gameObject.SetActive(false);
                    _ahssEmptyRight.gameObject.SetActive(true);
                }
            }
            if (_ahssReloadLeft.gameObject.activeSelf || _ahssReloadRight.gameObject.activeSelf)
            {
                if (_reloadAnimationTimeLeft <= 0f)
                {
                    _ahssReloadLeft.gameObject.SetActive(false);
                    _ahssReloadRight.gameObject.SetActive(false);
                    _ahssBackgroundLeft.gameObject.SetActive(true);
                    _ahssBackgroundRight.gameObject.SetActive(true);
                }
            }
        }

        private void UpdateTS()
        {
            var weapon = (ThunderspearWeapon)_human.Weapon;
            float ratio = 0f;
            if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
            {
                ratio = weapon.GetCooldownRatio();
                _bombFillLeft.fillAmount = _bombFillRight.fillAmount = ratio;
                bool inCooldown = ratio < 1f;
                if (_bombInCooldown != inCooldown)
                {
                    _bombInCooldown = inCooldown;
                    if (!_bombInCooldown && !_tsBombReload.gameObject.activeSelf)
                    {
                        _tsBombBackground.gameObject.SetActive(false);
                        _tsBombReload.gameObject.SetActive(true);
                        _tsBombReload.GetComponent<Animator>().Update(0f);
                        _reloadAnimationTimeLeft = 0.5f;
                    }
                }
                if (!_tsBombBackground.gameObject.activeSelf && _reloadAnimationTimeLeft <= 0f)
                {
                    _tsBombReload.gameObject.SetActive(false);
                    _tsBombBackground.gameObject.SetActive(true);
                }
            }
            else
            {
                if (weapon.MaxRound > 0)
                    ratio = (float)weapon.RoundLeft / (float)weapon.MaxRound;
                else if (weapon.RoundLeft == -1)
                    ratio = 1f;
                _ammoFillLeft.fillAmount = _ammoFillRight.fillAmount = ratio;
                if (ratio <= 0f)
                {
                    _tsBackground.color = BackgroundVeryLowColor;
                    _tsShoot.color = BackgroundVeryLowColor;
                }
                else if (ratio <= 0.5f)
                {
                    _tsBackground.color = BackgroundLowColor;
                    _tsShoot.color = BackgroundLowColor;
                }
                else
                {
                    _tsBackground.color = BackgroundNormalColor;
                    _tsShoot.color = BackgroundNormalColor;
                }
                _ammoLabelLeft.text = _ammoLabelRight.text = weapon.AmmoLeft.ToString();
                if (weapon.AmmoLeft == 0)
                {
                    _ammoLabelLeft.color = Color.red;
                    _ammoLabelRight.color = Color.red;
                }
                else
                {
                    _ammoLabelLeft.color = Color.green;
                    _ammoLabelRight.color = Color.green;
                }
                if (!_tsBackground.gameObject.activeSelf)
                {
                    if (_reloadAnimationTimeLeft <= 0f && _shootAnimationTimeLeft <= 0f)
                    {
                        _tsBackground.gameObject.SetActive(true);
                        _tsReload.gameObject.SetActive(false);
                        _tsShoot.gameObject.SetActive(false);
                        _ammoFillLeft.gameObject.SetActive(true);
                        _ammoFillRight.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
