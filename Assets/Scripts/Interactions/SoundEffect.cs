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
        // 게임 시작 시에는 사운드 재생을 건너뜁니다.
        if (isInitialState)
        {
            isInitialState = false; // 다음부터는 사운드 재생 허용
            return;
        }

        // 그 외의 경우에는 사운드 재생
        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
