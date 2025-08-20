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
        if (settingsPanel.activeSelf) //true���
            CloseSettings();
        else
            OpenSettings();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        Time.timeScale = 0.1f;

        // �����̴� �� ����
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;

        // ������ ���, �ǽð��ݿ���
        bgmSlider.onValueChanged.AddListener(OnBgmSlider);
        sfxSlider.onValueChanged.AddListener(OnSfxSlider);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // ������ ���� (�� �ص� ������ ����ϰ� �Ϸ��� ����)
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ���� �� �ε���
        SceneManager.LoadScene(currentSceneIndex + 1); // ���� ������ �̵�
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
