using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDamageble //���ݹ������ִ� ��ü�� ���
{
    void TakePhysicalDamage(int damage);
}

public enum PlayerState //�÷��̾��� �������
{
    Idle,
    Run,
    Jump,
    Attack,
    Damaged,
    Dead
}

public class UI_Manager : MonoBehaviour //�����Ͷ� ���� ������
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
