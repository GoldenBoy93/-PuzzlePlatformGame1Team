using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// AI ���¸� �����ϱ� ���� enum ����
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

    // Attacking ���¿� �ʿ��� ������
    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;     // player���� �Ÿ��� ��� �� ����

    public float fieldOfView = 120f;

    private Animator animator;
    // NPC���� meshRenderer�� ��Ƶ� �迭 �� ���� ���� �� �� ���� ����
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        // ���� ������ A�� ����
        currentWanderTarget = wanderPointA;
        // ���� ���´� Wandering���� ����
        SetState(AIState.Wandering);
    }


    private void Update()
    {
        // player���� �Ÿ��� �� �����Ӹ��� ���
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

    // ���¿� ���� agent�� �̵��ӵ�, �������θ� ����
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

        // �⺻ ��(walkSpeed)�� ���� ������ �缳��
        animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        // ��ȸ ���� ��
        if (aiState == AIState.Wandering)
        {
            // ��ǥ ������ ���� �������� ���
            if (agent.remainingDistance < 0.5f)
            {
                // ��ǥ ���� ����: A���� B, B���� A��
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

        // �÷��̾ ���� ������ ������ ���� ���·� ��ȯ
        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void AttackingUpdate()
    {
        // �÷��̾���� �Ÿ��� ���ݹ��� �ȿ� �ְ� �þ߰� �ȿ� ���� ��
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
            // ���ݹ��� �ȿ��� ������ �������� �ȿ��� ���� ��
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
            // �������� ������ ������ ��
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
        // ���� ���ϱ� (Ÿ�� - �� ��ġ) -- ��
        Vector3 directionToPlayer = GameManager.Instance.PlayerInstance.transform.position - transform.position;
        // �� ���� ����� �� ������ ���� ���ϱ�
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        // ������ �þ߰��� 1/2 ���� �۴ٸ� �þ߰� �ȿ� �ִ� ��.
        // �þ߰�(ex.120��) = �� ���� �������� �¿�� 60����
        return angle < fieldOfView * 0.5f;
    }
}