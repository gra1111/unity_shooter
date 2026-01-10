using UnityEngine;

public class Demon : EnemyGrounded
{
    [Header("Ataque")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private int attackDamage = 12;

    public void AnimEvent_AttackHit()
    {
        TryHitPlayer(attackDamage, attackDistance, ignoreAngle: false);
    }
    protected override void Awake()
    {
        base.Awake();
        state = State.Chasing;
    }
    protected override void StateMachine()
    {
        Vector3 toPlayer = target.position - transform.position;
        float dist = new Vector3(toPlayer.x, 0f, toPlayer.z).magnitude;
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
    protected override void Update()
    {
        base.Update();
        if (state == State.Dead) return;
        StateMachine();
    }

    private void ChaseBehaviour(Vector3 toPlayer)
    {
        Vector3 dir = new Vector3(toPlayer.x, 0f, toPlayer.z).normalized;
        if (dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
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
        Vector3 dir = new Vector3(toPlayer.x, 0f, toPlayer.z).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
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
