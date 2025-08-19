using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip soundEffect;
    private AudioSource audioSource;

    private bool isInitialState = true; // 최초 실행인지 확인하는 변수

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect()
    {
        Debug.Log("PlaySoundEffect 함수가 호출되었습니다.");

        if (isInitialState == true)
        {
            Debug.Log("isInitialState가 true여서 사운드 재생을 건너뜁니다.");
            isInitialState = false;
            Debug.Log(isInitialState);
            return;
        }

        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
            Debug.Log("사운드 이펙트가 재생되었습니다.");
        }
        else
        {
            Debug.Log("사운드 이펙트 또는 오디오 소스가 할당되지 않았습니다.");
        }
    }
}
