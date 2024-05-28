using System.Collections;
using UnityEngine;
using Photon.Pun;
using Characters;
using Settings;
using TMPro;
using UnityEngine.UI;
using UI;
using Utility;
using System.Resources;
using GameManagers;
using CustomLogic;

class ZippsUIManager : MonoBehaviour
{
    protected Human _human;
    private void Start()
    {
        _humanInput = SettingsManager.InputSettings.Human;
        previousHorseAutorunState = EmVariables.HorseAutorun;
        horseAutoRunAnimator =  HorseAutoRunObject.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        LogisticianUpdate();
        CannoneerUpdate();
        AbilityWheelUpdate();
        EmHUDUpdate();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            HideAbilityWheel();
            LogisticianMenu.SetActive(false);
            EmVariables.LogisticianOpen = false;
        }
    }

    #region EM Menu

    [SerializeField]
    private GameObject CanvasObj;

    [SerializeField]
    private TMP_InputField CoordsInputField;

    public void CloseEmMenu()
    {
        CanvasObj.SetActive(false);
    }

    public void OpenEmMenu()
    {
        CanvasObj.SetActive(true);
    }

    public void OnTPPlayerButtonClick(int setting)
    {
        GameObject TargetplayerGameObject = PhotonExtensions.GetPlayerFromID(EmVariables.SelectedPlayer.ActorNumber);
        Vector3 Mypos = PhotonExtensions.GetMyPlayer().transform.position;

        switch (setting)
        {
            case 0: //TP all to me 
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    go.GetComponent<Human>().photonView.RPC("moveToRPC", RpcTarget.Others, new object[] { Mypos.x, Mypos.y, Mypos.z });
                }
                break;
            case 1: //TP player to me
                TargetplayerGameObject.GetComponent<Human>().photonView.RPC("moveToRPC", EmVariables.SelectedPlayer, new object[] { Mypos.x, Mypos.y, Mypos.z });
                break;
            case 2: //TP me to player
                GameObject ME = PhotonExtensions.GetMyPlayer();
                ME.transform.position = TargetplayerGameObject.transform.position;
                break;
            case 3: //TP player to coords
                string[] tpCoordsSplit = CoordsInputField.text.Split(' ');
                TargetplayerGameObject.GetComponent<Human>().photonView.RPC("moveToRPC", EmVariables.SelectedPlayer, new object[]
                {
                    float.Parse(tpCoordsSplit[0]),
                    float.Parse(tpCoordsSplit[1]),
                    float.Parse(tpCoordsSplit[2])
                });
                break;
        }
    }

    public void GiveRoles(int Role)
    {
        string RoleName = "";
        ExitGames.Client.Photon.Hashtable playerProps = EmVariables.SelectedPlayer.CustomProperties;
        switch (Role)
        {
            case 0:
                RoleName = "Logistician";
                break;
            case 1:
                RoleName = "Cannoneer";
                break;
            case 2:
                RoleName = "Carpenter";
                break;
            case 3:
                RoleName = "Veteran";
                break;
            case 4:
                RoleName = "Wagon";
                break;
        }

        if (RoleName == string.Empty) return;

        if (playerProps.ContainsKey(RoleName))
            playerProps.Remove(RoleName);
        else
            playerProps.Add(RoleName, true);
        EmVariables.SelectedPlayer.SetCustomProperties(playerProps);
    }

    #endregion

    #region Logistician Menu

    [Header("Logistician")]
    [SerializeField]
    private GameObject LogisticianMenu;
    [SerializeField]
    private GameObject LogisticianCanvas;
    [SerializeField]
    private RawImage BladeImage;
    [SerializeField]
    private RawImage GasImage;
    [SerializeField]
    private TMP_Text BladeSupplyCount;
    [SerializeField]
    private TMP_Text GasSupplyCount;
    [SerializeField]
    private AudioSource MenuAudioSource;

    private bool GasSelected = false;
    private bool BladeSelected = false;
    protected HumanInputSettings _humanInput;

    public void OnHoverGasOption()
    {
        GasSelected = true;
        MenuAudioSource.Play();
        GasImage.color = new Color(0.525f, 0.164f, 0.227f);
    }

    public void OnHoverExitGasOption()
    {
        GasSelected = false;
        GasImage.color = Color.white;
    }

    public void OnHoverBladeOption()
    {
        BladeSelected = true;
        MenuAudioSource.Play();
        BladeImage.color = new Color(0.525f, 0.164f, 0.227f);
    }

    public void OnHoverExitBladeOption()
    {
        BladeSelected = false;
        BladeImage.color = Color.white;
    }

    private void SpawnSelected()
    {
        GasImage.color = Color.white;
        BladeImage.color = Color.white;
        LogisticianMenu.SetActive(false);
        EmVariables.LogisticianOpen = false;

        if (BladeSelected && EmVariables.LogisticianBladeSupply > 0)
        {
            GameObject hero = PhotonExtensions.GetMyHuman();
            Vector3 Pos = hero.transform.position + (hero.transform.forward * 4f) + new Vector3(0,1.5f,0);
            GameObject obj = PhotonNetwork.Instantiate("Momos Folder/Functionality/Logistician/Prefabs/SpinningSupplyBladePrefab", Pos, Quaternion.identity );

            EmVariables.LogisticianBladeSupply--;

            BladeSelected = false;
            GasSelected = false;
            return;
        }
        if (GasSelected && EmVariables.LogisticianGasSupply > 0)
        {
            GameObject hero = PhotonExtensions.GetMyHuman();
            Vector3 Pos = hero.transform.position + (hero.transform.forward * 4f) + new Vector3(0, 1.5f, 0);
            GameObject obj = PhotonNetwork.Instantiate("Momos Folder/Functionality/Logistician/Prefabs/SpinningSupplyGasPrefab", Pos, Quaternion.identity);

            EmVariables.LogisticianGasSupply--;

            GasSelected = false;
            BladeSelected = false;
            return;
        }
    }

    private Color red = new Color(176f / 255f, 43f / 255f, 37f / 255f);
    private Color orange = new Color(208f / 255f, 119f / 255f, 28f / 255f);
    private Color yellow = new Color(227f / 255f, 199f / 255f, 36f / 255f);
    private Color green = new Color(39f / 255f, 116f / 255f, 46f / 255f);
    private void LogisticianUpdate()
    {
        if (PhotonExtensions.GetMyPlayer() == null)
        {
            EmVariables.LogisticianBladeSupply = EmVariables.LogisticianMaxSupply;
            EmVariables.LogisticianGasSupply = EmVariables.LogisticianMaxSupply;
            LogisticianCanvas.SetActive(false);
            LogisticianMenu.SetActive(false);
            EmVariables.LogisticianOpen = false;
            return;
        }

        _human = PhotonExtensions.GetMyHuman().gameObject.GetComponent<Human>();
        if (_human.Dead)
        {
            _human.MaxOutLogisticianSupplies();
            LogisticianCanvas.SetActive(false);
            LogisticianMenu.SetActive(false);
            EmVariables.LogisticianOpen = false;
            return;
        }

        if (_human == null)
        {
            _human.MaxOutLogisticianSupplies();
            LogisticianCanvas.SetActive(false);
            LogisticianMenu.SetActive(false);
            EmVariables.LogisticianOpen = false;
            return;
        }

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Logistician"))
        {
            LogisticianCanvas.SetActive(false);
            return;
        }
        else
            LogisticianCanvas.SetActive(true);

        BladeSupplyCount.text = EmVariables.LogisticianBladeSupply.ToString();
        GasSupplyCount.text = EmVariables.LogisticianGasSupply.ToString();

        switch (EmVariables.LogisticianBladeSupply)
        {
            case 0:
                BladeSupplyCount.color = red;
                break;
            case 1:
                BladeSupplyCount.color = red;
                break; 
            case 2:
                BladeSupplyCount.color = orange;
                break;
            case 3:
                BladeSupplyCount.color = yellow;
                break;
            default:
                BladeSupplyCount.color = green;
                break;
        }
        switch (EmVariables.LogisticianGasSupply)
        {
            case 0:
                GasSupplyCount.color = red;
                break;
            case 1:
                GasSupplyCount.color = red;
                break;
            case 2:
                GasSupplyCount.color = orange;
                break;
            case 3:
                GasSupplyCount.color = yellow;
                break;
            default:
                GasSupplyCount.color = green;
                break;
        }

        bool inMenu = InGameMenu.InMenu() || ChatManager.IsChatActive() || CustomLogicManager.Cutscene;
        if (_humanInput.LogisticianMenu.GetKeyDown() && !inMenu)
        {
            LogisticianMenu.SetActive(true);
            CanvasObj.SetActive(false);
            EmVariables.LogisticianOpen = true;
        }
        if (_humanInput.LogisticianMenu.GetKeyUp())
        {
            SpawnSelected();
            LogisticianMenu.SetActive(false);
            EmVariables.LogisticianOpen = false;
        }
    }

    #endregion

    #region Cannoneer

    GameObject CannonObj = null;
    private void CannoneerUpdate()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Cannoneer"))
            return;

        if (_humanInput.CannoneerSpawn.GetKeyDown())
        {
            GameObject hero = PhotonExtensions.GetMyHuman();
            Vector3 Pos = hero.transform.position + (hero.transform.forward * 5f) + new Vector3(0, 1.5f, 0);
            if (CannonObj != null)
            {
                CannonObj.GetComponent<CannoneerCannon>().UnMount();
            }
            if (CannonObj == null)
                CannonObj = PhotonNetwork.Instantiate("Map/Interact/Prefabs/CannoneerCannon", Pos, hero.transform.rotation);
        }
    }

    #endregion


    #region Ability Wheel
    [Header("Ability Wheel")]
    [SerializeField]
    private GameObject AbilityWheelMenu;
    [SerializeField]
    private GameObject AbilityWheelCanvas;
    [SerializeField]
    private GameObject LoadoutParent;
    [SerializeField]
    private AudioSource AbilityWheelAudio;
    [SerializeField]
    private Image Ability1Image; 
    [SerializeField]
    private Image Ability2Image;
    [SerializeField]
    private Image Ability3Image;
    [SerializeField]
    private Image LoadoutImage;
    [SerializeField]
    private RawImage Ability1Selector;
    [SerializeField]
    private RawImage Ability2Selector;
    [SerializeField]
    private RawImage Ability3Selector;
    [SerializeField]
    private RawImage LoadoutSelector;
    [SerializeField]
    public GameObject AbilitySelectionSound;
    [SerializeField]
    public AudioSource ChangeAbilityAudio;

    public bool LastHoveredLoadout = false;
    public bool Ability1Selected = false; // made public so i can set to red on human spawn //
    public bool Ability2Selected = false; // made public so i can set to white on human spawn //
    public bool Ability3Selected = false; // made public so i can set to white on human spawn //

    public void SetWheelImages()
    {
        if (SettingsManager.InGameCharacterSettings.Special.Value.Length == 0 || SettingsManager.InGameCharacterSettings.Special.Value == "None")
        {
            Ability1Image.sprite = LoadSprite("No");
        }
        else
        {
            Ability1Image.sprite = LoadSprite(SettingsManager.InGameCharacterSettings.Special.Value);
        }

        if (SettingsManager.InGameCharacterSettings.Special_2.Value.Length == 0 || SettingsManager.InGameCharacterSettings.Special_2.Value == "None")
        {
            Ability2Image.sprite = LoadSprite("No");
        }
        else
        {
            Ability2Image.sprite = LoadSprite(SettingsManager.InGameCharacterSettings.Special_2.Value);
        }

        if (SettingsManager.InGameCharacterSettings.Special_3.Value.Length == 0  || SettingsManager.InGameCharacterSettings.Special_3.Value == "None")
        {
            Ability3Image.sprite = LoadSprite("No");
        }
        else
        {
            Ability3Image.sprite = LoadSprite(SettingsManager.InGameCharacterSettings.Special_3.Value);
        }
    }

    private Sprite LoadSprite(string spriteName)
    {
        string path = "UI/Icons/Specials/" + spriteName.Replace(" ", "") + "SpecialIcon";
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
        {
            Debug.LogError("Sprite not found at path: " + path);
        }
        return sprite;
    }

    private Sprite LoadSpriteForLoadout(string spriteName) {
        string path = "UI/Icons/EM Icons/Vet" + spriteName;
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
        {
            Debug.LogError("Sprite not found at path: " + path);
        }
        return sprite;
    }
 
    public void OnHoverAbility1()
    {
        if (SettingsManager.InGameCharacterSettings.Special.Value.Length > 0 && SettingsManager.InGameCharacterSettings.Special.Value != "None")
        {
            Ability1Selected = true;
            Ability2Selected = false;
            Ability3Selected = false;
            LastHoveredLoadout = false;
            AbilityWheelAudio.Play();
            Ability1Selector.color = new Color(0.525f, 0.164f, 0.227f);
        }
    }

    public void OnHoverExitAbility1()
    {
        Ability1Selected = false;
        Ability1Selector.color = Color.white;
    }
    public void OnHoverAbility2()
    {
        if (SettingsManager.InGameCharacterSettings.Special_2.Value.Length > 0 && SettingsManager.InGameCharacterSettings.Special_2.Value != "None")
        {
            Ability1Selected = false;
            Ability2Selected = true;
            Ability3Selected = false;
            LastHoveredLoadout = false;
            AbilityWheelAudio.Play();
            Ability2Selector.color = new Color(0.525f, 0.164f, 0.227f);
        }
    }

    public void OnHoverExitAbility2()
    {
        Ability2Selected = false;
        Ability2Selector.color = Color.white;
    }
    public void OnHoverAbility3()
    {
        if (SettingsManager.InGameCharacterSettings.Special_3.Value.Length > 0 && SettingsManager.InGameCharacterSettings.Special_3.Value != "None")
        {
            Ability1Selected = false;
            Ability2Selected = false;
            Ability3Selected = true;
            LastHoveredLoadout = false;
            AbilityWheelAudio.Play();
            Ability3Selector.color = new Color(0.525f, 0.164f, 0.227f);
            
        }
    }

    public void OnHoverExitAbility3()
    {
        Ability3Selected = false;
        Ability3Selector.color = Color.white;
    }

    public void OnHoverLoadout()
    {
        Ability1Selected = false;
        Ability2Selected = false;
        Ability3Selected = false;
        LastHoveredLoadout = true; 
        LoadoutImage.color = new Color(0.525f, 0.164f, 0.227f);
        AbilityWheelAudio.Play();
    }

    public void OnHoverExitLoadout()
    {
        LastHoveredLoadout = false; 
        LoadoutImage.color = Color.white;
    }


    private void HideAbilityWheel()
    {
        _human = PhotonExtensions.GetMyHuman().gameObject.GetComponent<Human>();
        if (Ability1Selected && _human.CurrentSpecial != SettingsManager.InGameCharacterSettings.Special.Value)
        {
            _human.SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special.Value, 1);
        }
        else if (Ability2Selected && _human.CurrentSpecial != SettingsManager.InGameCharacterSettings.Special_2.Value)
        {
            _human.SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special_2.Value, 2);
        }
        else if (Ability3Selected && _human.CurrentSpecial != SettingsManager.InGameCharacterSettings.Special_3.Value)
        {
            _human.SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special_3.Value, 3);
        }
        else if (LastHoveredLoadout)
        {
            _human.SwitchVeteranLoadout();
            LastHoveredLoadout = false;
        }

        AbilityWheelCanvas.SetActive(true);
        AbilityWheelMenu.SetActive(false);
        EmVariables.AbilityWheelOpen = false;
        Ability1Image.color = Color.white;
        Ability2Image.color = Color.white;
        Ability3Image.color = Color.white;
        Ability1Selector.color = Color.white;
        Ability2Selector.color = Color.white;
        Ability3Selector.color = Color.white;
        LoadoutSelector.color = new Color(184f/255f, 184f/255f, 184f/255f);
        LoadoutImage.color = Color.white;
    }

    public void PlayAbilitySelectSoundFromKeybind()
    {
        AbilitySelectionSound.SetActive(true);
        ChangeAbilityAudio.Play();
        StartCoroutine(WaitForSelectAudioToFinish());
    }

    private IEnumerator WaitForSelectAudioToFinish()
    {
        while (ChangeAbilityAudio.isPlaying)
        {
            yield return null;
        }

        AbilitySelectionSound.SetActive(false);
    }

    public void KeepSelectedAbilityColor()
    {

        _human = PhotonExtensions.GetMyHuman().gameObject.GetComponent<Human>();

        if (_human.CurrentSpecial == SettingsManager.InGameCharacterSettings.Special.Value)
        {
            Ability1Image.color = new Color(0.525f, 0.164f, 0.227f);
        }
        else
        {
            Ability1Image.color = Color.white;
        }

        if (_human.CurrentSpecial == SettingsManager.InGameCharacterSettings.Special_2.Value)
        {
            Ability2Image.color = new Color(0.525f, 0.164f, 0.227f);
        }
        else
        {
            Ability2Image.color = Color.white;
        }

        if (_human.CurrentSpecial == SettingsManager.InGameCharacterSettings.Special_3.Value)
        {
            Ability3Image.color = new Color(0.525f, 0.164f, 0.227f);
        }
        else
        {
            Ability3Image.color = Color.white;
        }
    }

    private void UpdateLoadoutVisibility()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Veteran"))
        {
            if (LoadoutParent.activeInHierarchy == false)
                LoadoutParent.SetActive(true);

            _human = PhotonExtensions.GetMyHuman().gameObject.GetComponent<Human>();

            if(_human.Setup.Weapon_2 == HumanWeapon.Blade)
                LoadoutImage.sprite = LoadSpriteForLoadout("Blades");
            if(_human.Setup.Weapon_2 == HumanWeapon.AHSS)
                LoadoutImage.sprite = LoadSpriteForLoadout("AHSS");
            if(_human.Setup.Weapon_2 == HumanWeapon.APG)
                LoadoutImage.sprite = LoadSpriteForLoadout("APG");
            if(_human.Setup.Weapon_2 == HumanWeapon.Thunderspear)
                LoadoutImage.sprite = LoadSpriteForLoadout("TS");
        }
        else
        {
            LoadoutParent.SetActive(false);
        }
    }

    private void AbilityWheelUpdate()
    {
        bool isAbilityWheelActive = AbilityWheelMenu.activeInHierarchy;
        if (PhotonExtensions.GetMyPlayer() == null)
        {
            if (AbilityWheelMenu.activeInHierarchy || isAbilityWheelActive)
                HideAbilityWheel();

            return;
        }

        _human = PhotonExtensions.GetMyHuman().gameObject.GetComponent<Human>();
        if ((_human == null || _human.Dead) && isAbilityWheelActive)
        {
            HideAbilityWheel();
            return;
        }

        bool inMenu = InGameMenu.InMenu() || ChatManager.IsChatActive() || CustomLogicManager.Cutscene;
        if (_humanInput.AbilityWheelMenu.GetKeyDown() && !inMenu)
        {
            //SetWheelImages(); => moved to the human script for performance concerns by ata
            KeepSelectedAbilityColor();
            AbilityWheelCanvas.SetActive(true);
            AbilityWheelMenu.SetActive(true);
            CanvasObj.SetActive(false);
            EmVariables.AbilityWheelOpen = true;
        }
        if (_humanInput.AbilityWheelMenu.GetKeyUp())
        {
            HideAbilityWheel();
        }

        UpdateLoadoutVisibility();
    }
    #endregion

    #region EM HUD
    [Header("EM HUD")]
    [SerializeField]
    private GameObject EmHUD;
    [SerializeField]
    private RawImage HorseAutoRunImage;
    [SerializeField]
    private GameObject HorseAutoRunObject;
    [SerializeField]
    private GameObject HorseAutoRunAudioObject;
    [SerializeField]
    private AudioSource HorseAutoRunAudioSource;
    [SerializeField]
    private bool previousHorseAutorunState ;
    private Animator horseAutoRunAnimator;

    public void OpenEmHUD()
    {
        if (EmHUD.activeInHierarchy) return;
        EmHUD.SetActive(true);
    }

    public void CloseEmHUD()
    {
        if (!EmHUD.activeInHierarchy) return;
        EmHUD.SetActive(false);
    }

    private void EmHUDUpdate()
    {
        if (PhotonExtensions.GetMyPlayer() == null && EmVariables.EmHUD)
        {
            if (HorseAutoRunAudioObject.activeInHierarchy)
                CloseEmHUD();
        }
        else OpenEmHUD();
        
        Horse _horse = FindFirstObjectByType<Horse>();
        if (_horse == null)
        {
            if (!HorseAutoRunObject.activeInHierarchy) return;
            HorseAutoRunObject.SetActive(false);
        }
        else
        {   
            HorseAutoRunUpdate();
            if (HorseAutoRunObject.activeInHierarchy) return;
            HorseAutoRunObject.SetActive(true);
        }
    }

    private void HorseAutoRunUpdate()
    {
        if (EmVariables.HorseAutorun != previousHorseAutorunState)
        {
            if (EmVariables.HorseAutorun)
            {
                HorseAutoRunImage.color = new Color(0.525f, 0.164f, 0.227f);
                horseAutoRunAnimator.SetBool("HorseSway", true);
            }
            else
            {
                HorseAutoRunImage.color = Color.white;
                horseAutoRunAnimator.SetBool("HorseSway", false);
            }

            previousHorseAutorunState = EmVariables.HorseAutorun;
        }
    }

    public void PlayHorseAutoSwitchSoundFromKeybind()
    {
        HorseAutoRunAudioObject.SetActive(true);
        HorseAutoRunAudioSource.Play();
        StartCoroutine(WaitForSwitchAudioToFinish());
    }

    private IEnumerator WaitForSwitchAudioToFinish()
    {
        while (HorseAutoRunAudioSource.isPlaying)
        {
            yield return null;
        }
        HorseAutoRunAudioObject.SetActive(false);
        //atas mom
    }
    #endregion
}
