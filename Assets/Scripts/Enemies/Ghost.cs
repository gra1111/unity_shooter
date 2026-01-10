using UnityEngine;

public class Ghost : Enemy
{
    [Header("Ataque")]
    [SerializeField] public float attackDistance = 1.5f;
    [SerializeField] private int attackDamage = 8;

    public void AnimEvent_AttackHit()
    {
        TryHitPlayer(attackDamage, attackDistance, ignoreAngle: true);
    }
    protected override void StateMachine()
    {

        Vector3 toPlayer = target.position - transform.position;
        float dist = toPlayer.magnitude;
        if (dist < attackDistance) { state = State.Attacking; } else { state = State.Chasing; }

        switch (state)
        {
            case State.Chasing:
                ChaseBehaviour(toPlayer);
                break;
            case State.Attacking:
                AttackBehaviour(toPlayer);
                break;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        state = State.Chasing;
    }

    protected override void Update()
    {
        if (state == State.Dead) return;
        StateMachine();
    }

    private void ChaseBehaviour(Vector3 toPlayer)
    {
        Vector3 dir = toPlayer.normalized;
        if (dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );

        Vector3 horizontal = dir * speed;
        controller.Move(horizontal * Time.deltaTime);

        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", speed);

    }

    private void AttackBehaviour(Vector3 toPlayer)
    {
        Vector3 dir = toPlayer.normalized;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        if (animator != null)
        {
            animator.SetBool("IsAttacking", true);
            animator.SetFloat("Speed", 0f);
        }

        // daño al jugador
    }
}
