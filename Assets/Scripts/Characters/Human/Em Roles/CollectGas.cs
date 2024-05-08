using Characters;
using Photon.Pun;
using UnityEngine;

public class CollectGas : MonoBehaviour
{
    public bool DroppedByDead = false;

    private float timer = 0f;
    private float delay = 5f * 60f; // 5 minutes in seconds
    [SerializeField]
    private AudioClip collect;

    private float shrinkSpeed = 1f;

    private bool Die = false;  

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= delay)
        {
            // Code to execute after 5 minutes
            Destroy(gameObject);

            timer = 0f;
        }

        if (Die)
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        // If the GameObject is small enough, destroy it
        if (transform.localScale.x <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Die) return;

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
            Die = true;
            AudioSource.PlayClipAtPoint(collect, transform.position, 4f);
        }
    }
}
