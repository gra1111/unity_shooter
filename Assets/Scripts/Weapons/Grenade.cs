using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float delay = 0.3f;

    [SerializeField] private float radius = 5f;

    private int damage = 15;

    [SerializeField] private float explosionForce = 700f;

    private bool exploded = false;

    private float countdown;

    [SerializeField] private GameObject explosionEffect;


    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && exploded == false)
        {
            exploded = true;
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect,transform.position,transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (var  collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce,transform.position,radius);
            }

            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
