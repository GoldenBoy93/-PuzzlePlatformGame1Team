using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// AI 상태를 구별하기 위한 enum 선언
public enum AIState
{
    Idle,
    Wandering,
    Attacking
}

public class NPC : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;

    [Header("Wandering")]
    public Vector3 wanderPointA = new Vector3(7, -7, -19);
    public Vector3 wanderPointB = new Vector3(7, -7, 11);
    private Vector3 currentWanderTarget;

    // Attacking 상태에 필요한 정보들
    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;     // player와의 거리를 담아 둘 변수

    public float fieldOfView = 120f;

    private Animator animator;
    // NPC모델의 meshRenderer를 담아둘 배열 → 공격 받을 때 색 변경 예정
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        // 시작 지점을 A로 설정
        currentWanderTarget = wanderPointA;
        // 최초 상태는 Wandering으로 설정
        SetState(AIState.Wandering);
    }


    private void Update()
    {
        // player와의 거리를 매 프레임마다 계산
        playerDistance = Vector3.Distance(transform.position, GameManager.Instance.PlayerInstance.transform.position);

        animator.SetBool("IsMove", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    // 상태에 따른 agent의 이동속도, 정지여부를 설정
    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        // 기본 값(walkSpeed)에 대한 비율로 재설정
        animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        // 배회 중일 때
        if (aiState == AIState.Wandering)
        {
            // 목표 지점에 거의 도착했을 경우
            if (agent.remainingDistance < 0.5f)
            {
                // 목표 지점 변경: A에서 B, B에서 A로
                if (currentWanderTarget == wanderPointA)
                {
                    currentWanderTarget = wanderPointB;
                }
                else
                {
                    currentWanderTarget = wanderPointA;
                }

                agent.SetDestination(currentWanderTarget);
            }
        }

        // 플레이어가 감지 범위에 들어오면 공격 상태로 전환
        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void AttackingUpdate()
    {
        // 플레이어와의 거리가 공격범위 안에 있고 시야각 안에 있을 때
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                //GameManager.Instance.PlayerInstance.playerController.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("IsAttack");
            }
        }
        else
        {
            // 공격범위 안에는 없지만 감지범위 안에는 있을 때
            if (playerDistance < detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(GameManager.Instance.PlayerInstance.transform.position, path))
                {
                    agent.SetDestination(GameManager.Instance.PlayerInstance.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            // 감지범위 밖으로 나갔을 때
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        // 뱡향 구하기 (타겟 - 내 위치) -- ⓐ
        Vector3 directionToPlayer = GameManager.Instance.PlayerInstance.transform.position - transform.position;
        // 내 정면 방향과 ⓐ 사이의 각도 구하기
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        // 설정한 시야각의 1/2 보다 작다면 시야각 안에 있는 것.
        // 시야각(ex.120도) = 내 정면 방향으로 좌우로 60도씩
        return angle < fieldOfView * 0.5f;
    }
}