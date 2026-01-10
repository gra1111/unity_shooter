using UnityEngine;

public enum State { Idle, Wandering, Chasing, Attacking, Dead }
public abstract class Enemy : MonoBehaviour
{
    public Transform target;
    protected Animator animator;
    protected CharacterController controller;
    protected Health health;
    protected float speed = 4f;
    protected float rotationSpeed = 5f;
    protected State state;
    protected Health playerHealth;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = target != null ? target.GetComponent<Health>() : null;
        health.OnDeath += HandleDeath;
        health.OnDamaged += HandleDamaged;
        controller = GetComponent<CharacterController>();
    }

    protected abstract void Update();
    protected abstract void StateMachine();
    protected virtual void HandleDeath(Health h)
    {
        state = State.Dead;
        animator.SetBool("IsDead", true);
        Destroy(gameObject, 3f);
    }

    protected virtual void HandleDamaged(Health h, int amount) { }
    protected bool TryHitPlayer(int damage, float maxDistance, bool ignoreAngle = true, float maxAngle = 120f)
    {
        if (state == State.Dead) return false;
        if (target == null) return false;

        if (playerHealth == null)
            playerHealth = target.GetComponent<Health>();

        if (playerHealth == null) return false;

        Vector3 toPlayer = target.position - transform.position;
        toPlayer.y = 0f;

        float dist = toPlayer.magnitude;
        if (dist > maxDistance) return false;

        if (!ignoreAngle && toPlayer.sqrMagnitude > 0.0001f)
        {
            float angle = Vector3.Angle(transform.forward, toPlayer.normalized);
            if (angle > maxAngle * 0.5f) return false;
        }

        playerHealth.TakeDamage(damage);
        return true;
    }
}


public abstract class EnemyGrounded : Enemy
{
    protected float gravity = -9.81f;
    protected float verticalVelocity;

    protected override void Update()
    {
        if (state == State.Dead) return;
        ApplyGravity();
    }

    protected void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }
}

    
