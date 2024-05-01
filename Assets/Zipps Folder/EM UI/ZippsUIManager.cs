using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Characters;
using Settings;
using Unity.VisualScripting;
using TMPro;
using System.Linq;

public class ZippsUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CanvasObj;

    [SerializeField]
    private TMP_InputField CoordsInputField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

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
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
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

        if (EmVariables.SelectedPlayer.CustomProperties.ContainsKey(RoleName))
        {
            playerProps[RoleName] = true;
            EmVariables.SelectedPlayer.SetCustomProperties(playerProps);
        }
        else
        {
            playerProps.Remove(RoleName);
            EmVariables.SelectedPlayer.SetCustomProperties(playerProps);
        }
    }
}
