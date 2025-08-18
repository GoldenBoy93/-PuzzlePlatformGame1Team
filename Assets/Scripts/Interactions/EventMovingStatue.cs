using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMovingStatue : MonoBehaviour
{
    private Animator animator;
    public AudioClip soundEffect;
    private AudioSource audioSource;

    private bool hasTriggered = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            animator.SetTrigger("EnterCollider");

            // 소리 재생
            audioSource.Play();

            hasTriggered = true;
        }
    }

    public void PlaySoundEffect()
    {
        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
