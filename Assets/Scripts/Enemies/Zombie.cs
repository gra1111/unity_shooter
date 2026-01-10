using UnityEngine;

public class Zombie : EnemyGrounded
{
    [Header("Wander")]
    [Header("Wander")]
    [SerializeField] private float wanderDistance = 15f;
    [SerializeField] private float wanderChangeInterval = 4f;
    [SerializeField] private float tookDamageDuration = 5f;
    private float lastTookDamage = 0f;
    private Vector3 initialPosition;

    [Header("Visión")]
    [SerializeField] private float detectionRadius = 15f;

    [Header("Ataque")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private Vector3 wanderTarget;
    [SerializeField] private int attackDamage = 10;

    protected override void StateMachine()
    {
        Vector3 toPlayer = target.position - transform.position;
        float dist = new Vector3(toPlayer.x, 0, toPlayer.z).magnitude;

        bool canSeePlayer = CanSeePlayer(toPlayer, dist);

        if (canSeePlayer || Time.time - lastTookDamage < tookDamageDuration)
        {
            if (dist > attackDistance)
                state = State.Chasing;
            else
                state = State.Attacking;
        }
        else
        {
            if (state != State.Wandering)
                initialPosition = transform.position;

            state = State.Wandering;
        }

        switch (state)
        {
            case State.Wandering:
                WanderBehaviour();
                break;
            case State.Chasing:
                ChaseBehaviour(toPlayer);
                break;
            case State.Attacking:
                AttackBehaviour(toPlayer);
                break;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (state == State.Dead) return;
        StateMachine();
    }
          
    protected override void HandleDamaged(Health h, int amount)
    {
        base.HandleDamaged(h, amount);
        lastTookDamage = Time.time;
    }
    private bool CanSeePlayer(Vector3 toPlayer, float dist)
    {
        if (dist > detectionRadius)
            return false;

        Vector3 dir = new Vector3(toPlayer.x, 0, toPlayer.z).normalized;
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > detectionAngle * 0.5f)
            return false;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        return !Physics.Raycast(eyePos, dir, dist, obstacleMask);
    }          

    private void WanderBehaviour()
    {
        if (Time.time >= nextWanderChangeTime ||
            Vector3.Distance(transform.position, wanderTarget) < 1f)
        {
            ChooseNewWanderTarget();
        }

        MoveTowards(wanderTarget, 0.5f);
    }
    private float nextWanderChangeTime;

    private void ChaseBehaviour(Vector3 toPlayer)
    {
        MoveTowards(target.position, 1f);
    }


    private void AttackBehaviour(Vector3 toPlayer)
    {
        LookAt(target.position);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("IsAttacking", true);

        // daño al jugador
    }
    public float detectionAngle = 180f;   

    private void MoveTowards(Vector3 pos, float rotationFactor)
    {
        Vector3 dir = pos - transform.position;
        dir.y = 0;
        dir.Normalize();

        LookAtDirection(dir, rotationFactor);

        Vector3 horizontal = dir * speed;
        controller.Move(horizontal * Time.deltaTime);

        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", speed);
    }
    public float eyeHeight = 1.5f;   

    private void LookAt(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        dir.y = 0;
        LookAtDirection(dir, 1f);
    }
    public LayerMask obstacleMask;   

    private void LookAtDirection(Vector3 dir, float factor)
    {
        if (dir == Vector3.zero) return;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * factor * Time.deltaTime
        );
    }


    private void ChooseNewWanderTarget()
    {
        wanderTarget = initialPosition +
                       new Vector3(
                           Random.Range(-wanderDistance, wanderDistance),
                           0,
                           Random.Range(-wanderDistance, wanderDistance)
                       );

        nextWanderChangeTime = Time.time + wanderChangeInterval;
    }

    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        ChooseNewWanderTarget();
        state = State.Wandering;
    }
    public void AnimEvent_AttackHit()
    {
        TryHitPlayer(attackDamage, attackDistance, ignoreAngle: false);
    }
}


