using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SettingPanel : MonoBehaviour
{
    public UI_SettingPanel settings;
    public Slider bgmSlider;
    public Slider sfxSlider;
    GameObject start;
    GameObject save;

    PlayerController _controller;

    public void InitPanel()
    {
        settings = SafeFetchHelper.GetChildOrError<UI_SettingPanel>(UI_Manager.Instance.gameObject);
        start = UI_Manager.Instance._start;
        save = UI_Manager.Instance._save;
        _controller = SafeFetchHelper.GetChildOrError<PlayerController>(Player.Instance.gameObject);
    }

    public void OnToggleSettings()
    {
        if (settings.gameObject.activeSelf) //true���
        {
            CloseUI();
            settings.gameObject.SetActive(false);

            // ������ ���� (�� �ص� ������ ����ϰ� �Ϸ��� ����)
            bgmSlider.onValueChanged.RemoveListener(OnBgmSlider);
            sfxSlider.onValueChanged.RemoveListener(OnSfxSlider);
        }
        else
        {
            OpenUI();
            settings.gameObject.SetActive(true);

            // �����̴� �� ����
            bgmSlider.value = AudioManager.Instance.bgmVolume;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
            // ������ ���, �ǽð��ݿ���
            bgmSlider.onValueChanged.AddListener(OnBgmSlider);
            sfxSlider.onValueChanged.AddListener(OnSfxSlider);

        }
    }

    public void OpenUI()
    {
        Time.timeScale = 0.1f;
        _controller.LockInputOn();
        DirectionManager.Instance.LockCamOn(true);
    }
    public void CloseUI()
    {
        Time.timeScale = 1f;
        _controller.LockInputOff();
        DirectionManager.Instance.LockCamOn(false);
    }
    public void OnCloseTheScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSave()
    {
        save.SetActive(!save.activeSelf);
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

    public void OnStart()
    {
        start.SetActive(!start.activeSelf);
        DirectionManager.Instance.Direction();
    }

    public void OnGameOver()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ �÷��� ��� ����
#else
              Application.Quit(); // ����� ���� ����
#endif
    }
}