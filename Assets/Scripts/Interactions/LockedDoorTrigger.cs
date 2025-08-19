using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public GameObject wallCollider;
    public string keyName;
    private int firstCheck = 0;

    public AudioClip soundEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // ����Ŵ����� Űüũ �Լ��� ȣ��
        if(PuzzleManager.Instance.KeyCheck(other, wallCollider) && firstCheck == 0)
        {
            audioSource.PlayOneShot(soundEffect);
            firstCheck++;
        }

        return;
    }
}