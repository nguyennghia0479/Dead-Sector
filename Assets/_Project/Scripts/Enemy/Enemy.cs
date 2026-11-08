using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("State Settings")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float chaseDistance = 10f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float attackDistance = 1.2f;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackMultiplier = .15f;
    [SerializeField] private float knockbackDecay = 8f;

    [Header("References")]
    [SerializeField] private EnemyHealth enemyHealth;

    private float distanceToPlayer;
    private Vector3 knockbackVelocity;
    private Animator animator;
    private NavMeshAgent agent;
    private Collider collider;
    private StateMachine stateMachine;
    private PlayerController player;
    private bool isPlayHitAnimation;

    public float WalkSpeed => walkSpeed;
    public float ChaseDistance => chaseDistance;
    public float ChaseSpeed => chaseSpeed;
    public float AttackDistance => attackDistance;
    public float DistanceToPlayer => distanceToPlayer;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public PlayerController Player => player;
    public bool IsPlayHitAnimation => isPlayHitAnimation;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyWalkState WalkState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    private static readonly int HitHash = Animator.StringToHash("hit");
    private static readonly int Death_1Hash = Animator.StringToHash("death_1");
    private static readonly int Death_2Hash = Animator.StringToHash("death_2");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();

        stateMachine = new();
        IdleState = new EnemyIdleState(this, stateMachine, "idle");
        WalkState = new EnemyWalkState(this, stateMachine, "walk");
        ChaseState = new EnemyChaseState(this, stateMachine, "chase");
        AttackState = new EnemyAttackState(this, stateMachine, "attack");
    }

    private void OnEnable()
    {
        enemyHealth.OnDeath += HandleOnDeath;
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= HandleOnDeath;
    }

    private void Start()
    {
        stateMachine.Initialized(IdleState);
        enemyHealth.Initialzied();
    }

    private void Update()
    {
        if (!agent.enabled || !collider.enabled || enemyHealth.IsDeath) return;

        stateMachine.CurrentStateUpdate();
        HandleDistanceToPlayer();
        HandleKnockback();
    }

    public void TakeHit(Vector3 direction, float force, int gunDamage)
    {
        if (enemyHealth.IsDeath) return;

        ApplyKnockback(direction, force);
        enemyHealth.TakeDamage(gunDamage);
        PlayHitAnimation();
    }

    private void ApplyKnockback(Vector3 direction, float force)
    {
        direction.Normalize();
        knockbackVelocity += knockbackMultiplier * force * direction;
    }

    private void HandleKnockback()
    {
        if (knockbackVelocity.magnitude < .01f)
        {
            knockbackVelocity = Vector3.zero;
            return;
        }

        agent.Move(knockbackVelocity * Time.deltaTime);
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.deltaTime * knockbackDecay);
    }

    private void PlayHitAnimation()
    {
        if (enemyHealth.IsDeath || isPlayHitAnimation) return;

        isPlayHitAnimation = true;
        agent.isStopped = true;
        animator.SetTrigger(HitHash);
    }

    public void HandleLookToPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > .001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public void OnHitAnimationFinished()
    {
        isPlayHitAnimation = false;
        if (enemyHealth.IsDeath)
            return;

        agent.isStopped = false;
    }

    private void HandleDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(agent.transform.position, player.transform.position);
    }

    private void HandleOnDeath()
    {
        animator.ResetTrigger(HitHash);
        agent.enabled = false;
        collider.enabled = false;

        if (Random.Range(0, 2) == 0)
            animator.SetTrigger(Death_1Hash);
        else
            animator.SetTrigger(Death_2Hash);

        float delayTime = 10f;
        Destroy(gameObject, delayTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
