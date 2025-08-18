using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public GameObject wallCollider;
    public string KeyName;

    public AudioClip soundEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // ����Ŵ����� Űüũ �Լ��� ȣ��
        PuzzleManager.Instance.KeyCheck(other);
    }
}