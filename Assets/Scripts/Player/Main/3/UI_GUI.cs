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

        // �����̴� �� ����
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;

        // ������ ���
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged); //�ǽð��ݿ���
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // ������ ���� (�� �ص� ������ ����ϰ� �Ϸ��� ����)
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
    }
    public void ToggleSettings()
    {
        if (settingsPanel.activeSelf) //true���
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
