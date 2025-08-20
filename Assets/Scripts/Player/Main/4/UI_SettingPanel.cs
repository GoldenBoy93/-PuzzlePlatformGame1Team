using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SettingPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void OnToggleSettings()
    {
        if (settingsPanel.activeSelf) //true라면
            CloseSettings();
        else
            OpenSettings();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        Time.timeScale = 0.1f;

        // 슬라이더 값 세팅
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;

        // 리스너 등록, 실시간반영용
        bgmSlider.onValueChanged.AddListener(OnBgmSlider);
        sfxSlider.onValueChanged.AddListener(OnSfxSlider);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // 리스너 해제 (안 해도 되지만 깔끔하게 하려면 유지)
        bgmSlider.onValueChanged.RemoveListener(OnBgmSlider);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSlider);
    }
    public void OnCloseTheScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSave()
    {

    }
    public void OnLoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 현재 씬 인덱스
        SceneManager.LoadScene(currentSceneIndex + 1); // 다음 씬으로 이동
    }
    public void OnGodMode()
    {

    }

    public void OnBgmSlider(float value)
    {
        AudioManager.Instance.SetBgmVolume(value);
    }

    public void OnSfxSlider(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }
}
