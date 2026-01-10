using UnityEngine;
using TMPro;

public class ShootingWeapon : MonoBehaviour
{
    [SerializeField] public int ammoInClip;
    [SerializeField] private int totalAmmo;

    [SerializeField] private float shootRate;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float shootForce;

    [SerializeField] private GameObject bullet;

    [SerializeField] private float shootRateTime;

    [SerializeField] private TextMeshProUGUI ammoText;

    [SerializeField] private bool ContinueShooting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!ContinueShooting)
            {
                Shoot();
            }
            else
            {
                InvokeRepeating("Shoot", 0.001f, shootRate);
            }
        }

        else if (Input.GetKeyUp(KeyCode.Mouse0) && ContinueShooting)
        {
            CancelInvoke("Shoot");
        }


        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }


    public virtual void Shoot()
    {
        if (ammoInClip > 0)
        {
            if (Time.time > shootRateTime)
            {
                ammoInClip--;

                GameObject newBullet;

                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

                Rigidbody rb = newBullet.GetComponent<Rigidbody>();

                rb.AddForce(-transform.forward * shootForce);

                shootRateTime = Time.time + shootRate;

                Destroy(newBullet, 5);

                ammoText.text = ammoInClip.ToString();
            }
        }
        else
        {
            Debug.Log("Sin balas, recarga");
            return;
        }
    }

    public virtual void Reload()
    {
        ammoInClip = totalAmmo;

        ammoText.text = ammoInClip.ToString();
    }
}
