using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SettingPanel : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    GameObject start;
    GameObject save;

    PlayerController _controller;
    InventoryView _inventory;

    public void InitPanel()
    {
        start = UI_Manager.Instance._start;
        save = UI_Manager.Instance._save;
        _controller = SafeFetchHelper.GetChildOrError<PlayerController>(Player.Instance.gameObject);
        _inventory = SafeFetchHelper.GetChildOrError<InventoryView>(UI_Manager.Instance.gameObject);
    }

    public void OpenUI()
    {
        Time.timeScale = 0.1f;
        DirectionManager.Instance.LockOnCam(true);
    }
    public void CloseUI()
    {
        Time.timeScale = 1f;
        DirectionManager.Instance.LockOnCam(false);
    }

    public void OnToggleSettings()
    {
        if (!gameObject.activeSelf)
        {
            OpenUI();
            gameObject.SetActive(true);

            // ������ ���� (�� �ص� ������ ����ϰ� �Ϸ��� ����)
            bgmSlider.onValueChanged.RemoveListener(OnBgmSlider);
            sfxSlider.onValueChanged.RemoveListener(OnSfxSlider);
        }
        else
        {
            CloseUI();
            gameObject.SetActive(false);

            // �����̴� �� ����
            bgmSlider.value = AudioManager.Instance.bgmVolume;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
            // ������ ���, �ǽð��ݿ���
            bgmSlider.onValueChanged.AddListener(OnBgmSlider);
            sfxSlider.onValueChanged.AddListener(OnSfxSlider);

        }
    }
    public void OnToggleInventory()
    {
        if (!_inventory.gameObject.activeSelf)
        {
            OpenUI();
            _inventory.gameObject.SetActive(true);
        }
        else
        {
            CloseUI();
            _inventory.gameObject.SetActive(false);
        }
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
        start.SetActive(false);
        DirectionManager.Instance.Direction_Intro();
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