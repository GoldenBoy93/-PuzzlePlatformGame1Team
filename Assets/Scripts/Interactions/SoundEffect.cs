using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip soundEffect;
    private AudioSource audioSource;

    private bool isInitialState = true; // ���� �������� Ȯ���ϴ� ����

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect()
    {
        // ���� ���� �ÿ��� ���� ����� �ǳʶݴϴ�.
        if (isInitialState)
        {
            isInitialState = false; // �������ʹ� ���� ��� ���
            return;
        }

        // �� ���� ��쿡�� ���� ���
        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
