using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Start : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        settingsPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        // 슬라이더 값 세팅
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;

        // 리스너 등록
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged); //실시간반영용
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // 리스너 해제 (안 해도 되지만 깔끔하게 하려면 유지)
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
    }
    public void ToggleSettings()
    {
        if (settingsPanel.activeSelf) //true라면
            CloseSettings();
        else
            OpenSettings();
    }
    public void CloseTheScene()
    {
        //SceneLoader.LoadScene(Escene.MainMenu);
    }
    public void OnClick_StageLevelUp()
    {
        //GameManager.Instance;
    }
    public void OnClick_HealingPlayer()
    {
    }

    void OnBgmSliderChanged(float value)
    {
        AudioManager.Instance.SetBgmVolume(value);
    }

    void OnSfxSliderChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }
}
