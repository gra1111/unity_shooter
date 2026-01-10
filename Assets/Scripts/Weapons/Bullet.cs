using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            return; // No hacer nada si es el jugador
        }

        Health h = collider.GetComponentInParent<Health>();
        if (h != null)
        {
            h.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
