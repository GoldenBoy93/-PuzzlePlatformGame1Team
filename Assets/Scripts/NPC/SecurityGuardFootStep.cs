using UnityEngine;
using UnityEngine.AI;

public class SecurityGuardFootStep : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent; // Rigidbody ��� NavMeshAgent ���

    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // NavMeshAgent�� �������� �̵� ������ Ȯ��
        if (navMeshAgent.hasPath && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            // ������Ʈ�� ���� �ӵ��� �Ӱ谪���� ������ Ȯ��
            if (navMeshAgent.velocity.magnitude > footstepThreshold)
            {
                if (Time.time - footStepTime > footstepRate)
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }
            }
        }
    }
}