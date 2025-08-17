using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class NamedSFX
{
    public string name;
    public AudioClip clip;
}
public class AudioManager1 : MonoBehaviour
{
    public static AudioManager1 Instance;

    [Header("BGM Clips")]
    public AudioClip menuBgm;
    public AudioClip gameBgm;
    public AudioClip bossBgm;
    private AudioSource bgmSource;

    [Header("SFX Clips")]
    public List<NamedSFX> sfxClips;
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    public int sfxPoolSize = 5;

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentSfxIndex = 0;

    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AudioSource bgm = gameObject.AddComponent<AudioSource>();
            bgm.loop = true;
            bgm.playOnAwake = false;
            bgmSource = bgm;

            for (int i = 0; i < sfxPoolSize; i++)
            {
                AudioSource sfx = gameObject.AddComponent<AudioSource>();
                sfx.playOnAwake = false;
                sfxSources.Add(sfx);
            }
            foreach (var sfx in sfxClips)
            {
                if (!sfxDict.ContainsKey(sfx.name))
                    sfxDict.Add(sfx.name, sfx.clip);
                else
                    Debug.LogWarning($"[AudioManager] 중복된 SFX 이름: {sfx.name}");
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 항상 볼륨 최신화 (옵션에서 슬라이더로 조절 시 반영되게)
        bgmSource.volume = bgmVolume;
        foreach (var sfx in sfxSources)
        {
            sfx.volume = sfxVolume;
        }
        //전역 버튼소리 활성화
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;

            if (clicked != null && clicked.GetComponent<Button>() != null)
            {
                AudioManager1.Instance.PlayClickSFX();
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            PlayBGM(menuBgm);
        else if (scene.buildIndex == 1)
            PlayBGM(gameBgm);
        else if (scene.buildIndex == 2)
            PlayBGM(bossBgm);
        else if (scene.buildIndex == 3)
            PlayBGM(bossBgm);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSources[currentSfxIndex].PlayOneShot(clip);
        currentSfxIndex = (currentSfxIndex + 1) % sfxPoolSize;
    }
    public void PlaySFX(string name)
    {
        if (sfxDict.ContainsKey(name))
        {
            AudioClip clip = sfxDict[name];
            sfxSources[currentSfxIndex].PlayOneShot(clip);
            currentSfxIndex = (currentSfxIndex + 1) % sfxPoolSize;
        }
        else
        {
            Debug.LogWarning($"[AudioManager] SFX '{name}' not found!");
        }
    }
    public void PlayClickSFX()
    {
        PlaySFX("Click"); // 또는 클릭 사운드 이름에 맞게 수정
    }


    // 외부에서 조절용
    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}