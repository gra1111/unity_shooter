using UnityEngine;

public class Boss : EnemyGrounded
{
    [Header("Ataque")]
    [SerializeField] private float attackDistance = 6f;
    [SerializeField] private float jumpMinDistance = 2f;
    [SerializeField] protected float punchMaxDistance = 2f;

    [Header("Daño por ataque (Animation Events)")]
    [SerializeField] private int[] attackDamages = new int[] { 50, 35 };
    [SerializeField] private float[] attackHitRanges = new float[] { 2f, 2f };
    [SerializeField] private bool[] attackIgnoreAngle = new bool[] { true, false};

    public void AnimEvent_BossAttackHit(int attackId)
    {
        if (attackDamages == null || attackDamages.Length == 0) return;

        int dmg = (attackId >= 0 && attackId < attackDamages.Length) ? attackDamages[attackId] : attackDamages[0];

        float rangeFallback = punchMaxDistance;
        float range = (attackHitRanges != null && attackId >= 0 && attackId < attackHitRanges.Length)
            ? attackHitRanges[attackId]
            : rangeFallback;

        bool ignoreAngle = (attackIgnoreAngle != null && attackId >= 0 && attackId < attackIgnoreAngle.Length)
            ? attackIgnoreAngle[attackId]
            : true;

        TryHitPlayer(dmg, range, ignoreAngle: ignoreAngle);
    }
    protected override void StateMachine()
    {
        Vector3 toPlayer = target.position - transform.position;
        float dist = new Vector3(toPlayer.x, 0f, toPlayer.z).magnitude;

        if (dist <= attackDistance)
        {
            state = State.Attacking;
        }
        else
        {
            state = State.Chasing;
        }

        switch (state)
        {
            case State.Chasing:
                ChaseBehaviour(toPlayer);
                break;
            case State.Attacking:
                AttackBehaviour(toPlayer, dist);
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
        animator.SetBool("IsFirstAttack", false);
    }

    private void AttackBehaviour(Vector3 toPlayer, float dist)
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
        bool doJumpAttack = false;
        if (dist >= jumpMinDistance)
        {
            doJumpAttack=true;
        }

        animator.SetBool("IsFirstAttack", doJumpAttack);
        animator.SetBool("IsAttacking", true);
        animator.SetFloat("Speed", 0f);

        if (doJumpAttack)
        {
            // daño al jugador del ataque de salto
        }
        else
        {
            // daño al jugador del ataque normal
        }
    }
}
