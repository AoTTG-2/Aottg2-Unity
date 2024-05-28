using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviourPun
{
    [Header("Cannon Parts")]
    [SerializeField] private Transform CanBase;
    [SerializeField] private Transform Barrel;
    [SerializeField] private Transform BarrelEnd;
    [SerializeField] private Transform HumanMount;

    private PhotonView PV;

    private GameObject Hero;

    private void Awake()
    {
        PV = gameObject.GetComponent<PhotonView>();
        Hero = PhotonExtensions.GetPlayerFromID(PV.Owner.ActorNumber);
    }

    void Start()
    {
        Hero.transform.position = HumanMount.transform.position;
        Hero.transform.parent = HumanMount;

        if (PV.IsMine)
        {

        }
    }

    void Shoot()
    {

    }

    void UnMount() //Gotta make this a RPC
    {
        Hero.transform.parent.parent = null;
        PhotonNetwork.Destroy(gameObject);
    }

    void FixedUpdate()
    {


        if (!PV.IsMine) return;

    }
}
