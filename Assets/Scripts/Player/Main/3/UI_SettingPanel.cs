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
        if (settings.gameObject.activeSelf) //true라면
        {
            CloseUI();
            settings.gameObject.SetActive(false);

            // 리스너 해제 (안 해도 되지만 깔끔하게 하려면 유지)
            bgmSlider.onValueChanged.RemoveListener(OnBgmSlider);
            sfxSlider.onValueChanged.RemoveListener(OnSfxSlider);
        }
        else
        {
            OpenUI();
            settings.gameObject.SetActive(true);

            // 슬라이더 값 세팅
            bgmSlider.value = AudioManager.Instance.bgmVolume;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
            // 리스너 등록, 실시간반영용
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

    public void OnStart()
    {
        start.SetActive(!start.activeSelf);
        DirectionManager.Instance.Direction();
    }

    public void OnGameOver()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 플레이 모드 종료
#else
              Application.Quit(); // 빌드된 게임 종료
#endif
    }
}