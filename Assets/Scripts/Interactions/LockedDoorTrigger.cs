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
        // 퍼즐매니저의 키체크 함수를 호출
        if(PuzzleManager.Instance.KeyCheck(other, wallCollider) && firstCheck == 0)
        {
            audioSource.PlayOneShot(soundEffect);
            firstCheck++;
        }

        return;
    }
}