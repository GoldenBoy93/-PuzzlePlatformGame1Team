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
        Debug.Log("PlaySoundEffect �Լ��� ȣ��Ǿ����ϴ�.");

        if (isInitialState == true)
        {
            Debug.Log("isInitialState�� true���� ���� ����� �ǳʶݴϴ�.");
            isInitialState = false;
            Debug.Log(isInitialState);
            return;
        }

        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
            Debug.Log("���� ����Ʈ�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("���� ����Ʈ �Ǵ� ����� �ҽ��� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
