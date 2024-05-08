using System.Collections;
using UnityEngine;
using Photon.Pun;
using Characters;
using Settings;
using TMPro;
using UnityEngine.UI;
using UI;

class ZippsUIManager : MonoBehaviour
{
    private void Start()
    {
        _humanInput = SettingsManager.InputSettings.Human;
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        LogisticianUpdate();
        CannoneerUpdate();
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
            return;
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
            case 4:
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
            case 4:
                GasSupplyCount.color = green;
                break;
        }


        if (_humanInput.LogisticianMenu.GetKeyDown() && !InGameMenu.InMenu())
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

    private void CannoneerUpdate()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Cannoneer"))
            return;

        if (_humanInput.CannoneerSpawn.GetKeyDown())
        {

        }
    }

    #endregion 
}
