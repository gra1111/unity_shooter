using TMPro;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    [SerializeField] private int selectedWeapon = 0;

    [SerializeField] private TextMeshProUGUI ammoText;

    void Update()
    {
        int previousWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= weapons.Length - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon--;
            }
        }
        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                if (i == selectedWeapon)
                {
                    weapon.gameObject.SetActive(true);
                    ShootingWeapon weapon2 = weapon.GetComponent<ShootingWeapon>();
                    ammoText.text = weapon2.ammoInClip.ToString();
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                i++;
            }
        }
    }
}
