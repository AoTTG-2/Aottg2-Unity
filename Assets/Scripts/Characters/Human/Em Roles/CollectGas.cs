using Characters;
using Photon.Pun;
using UnityEngine;

public class CollectGas : MonoBehaviour
{
    public bool DroppedByDead = false;

    private float timer = 0f;
    private float delay = 5f * 60f; // 5 minutes in seconds

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= delay)
        {
            // Code to execute after 5 minutes
            Destroy(gameObject);

            timer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetPhotonView().IsMine)
            {
                GameObject HumanObj = PhotonExtensions.GetMyHuman();
                Human HumanComp = HumanObj.GetComponent<Human>();

                if (DroppedByDead)
                {
                    HumanComp.CurrentGas = HumanComp.MaxGas * 0.3f;
                }
                else
                {
                    HumanComp.CurrentGas = HumanComp.MaxGas;
                }
            }
            Destroy(gameObject);
        }
    }
}
