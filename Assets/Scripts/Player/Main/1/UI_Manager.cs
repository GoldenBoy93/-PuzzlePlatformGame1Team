using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDamageble //공격받을수있는 물체면 상속
{
    void TakePhysicalDamage(int damage);
}

public enum PlayerState //플레이어의 현재상태
{
    Idle,
    Run,
    Jump,
    Attack,
    Damaged,
    Dead
}

public class UI_Manager : MonoBehaviour //데이터랑 구독 유지용
{
    private static UI_Manager _instance;
    public static UI_Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UI_Manager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UI_Manager>();
                }
            }
            return _instance;
        }
    }

    public PlayerModel _model { get; private set; }
    public PlayerViewModel _viewModel { get; private set; }

    public PlayerView _view;

    public GameObject _damageIndigator;
    public GameObject _promptText;
    public UI_ActionKey _uiaction;
    public GameObject _crosshair;
    public GameObject _conditions;
    public GameObject _quickslot;
    public GameObject _pauseButton;
    public GameObject _equipment;

    public Inventory _inventory;
    public UI_SettingPanel _settingPanel;
    public GameObject _pausePanel;
    public GameObject _gameOver;
    public GameObject _save;
    public GameObject _start;


    ItemData _data => ScriptableObject.CreateInstance<ItemData>();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _model = new PlayerModel();
        _viewModel = new PlayerViewModel(_model);

        //_view = SafeFetchHelper.GetOrError<PlayerView>(gameObject);
        //_inventory = SafeFetchHelper.GetChildOrError<Inventory>(gameObject);
    }

    private void Start()
    {
        _crosshair.SetActive(false);
        _promptText.SetActive(false);
        _damageIndigator.SetActive(false);
        _uiaction.gameObject.SetActive(false);
        _settingPanel.gameObject.SetActive(false);
        _inventory.gameObject.SetActive(false);
        _quickslot.SetActive(false);
        _pausePanel.SetActive(false);
        _gameOver.SetActive(false);
        _save.SetActive(false);
        _equipment.SetActive(true);
        _conditions.SetActive(true);
        _start.SetActive(true);
        _pauseButton.SetActive(true);

        _settingPanel.InitPanel();
    }
}