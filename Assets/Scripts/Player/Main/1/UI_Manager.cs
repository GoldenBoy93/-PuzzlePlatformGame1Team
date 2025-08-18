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
    public PlayerModel Model { get; private set; }
    public PlayerViewModel ViewModel { get; private set; }

    void Awake()
    {
        GameManager.Instance.UIManager = this;
        DontDestroyOnLoad(gameObject);

        Model = new PlayerModel();
        ViewModel = new PlayerViewModel(Model);
    }
}
