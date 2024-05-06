using Characters;
using Photon.Pun;
using UnityEngine;

class CollectBlade : MonoBehaviour
{
    private float timer = 0f;
    private float delay = 5f * 60f; // 5 minutes in seconds

    public BaseUseable Weapon;

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
                //((BladeWeapon)Weapon).BladesLeft++;
                Weapon = PhotonExtensions.GetMyHuman().GetComponent<Human>().Weapon;
                if (Weapon is BladeWeapon)
                {
                    var weapon = (BladeWeapon)Weapon;
                    if (weapon.BladesLeft < weapon.MaxBlades)
                        weapon.BladesLeft++;
                }
                else if (Weapon is AHSSWeapon || Weapon is APGWeapon)
                {
                    var weaponahss = (AHSSWeapon)Weapon;
                    if (weaponahss.AmmoLeft < weaponahss.MaxAmmo)
                        weaponahss.AmmoLeft++;
                }
                else if (Weapon is ThunderspearWeapon)
                {
                    var weaponts = (ThunderspearWeapon)Weapon;
                    if (weaponts.AmmoLeft < weaponts.MaxAmmo)
                        weaponts.AmmoLeft++;
                }

            }
            Destroy(gameObject);
        }
    }
}
