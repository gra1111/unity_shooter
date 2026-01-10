using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField] private float throwForce = 500f;

    [SerializeField] private GameObject grenade;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw();
        }
    }

    void Throw()
    {
        GameObject newGrenade = Instantiate(grenade,transform.position,transform.rotation);

        newGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
    }
}
