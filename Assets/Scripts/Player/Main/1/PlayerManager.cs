using System;
using UnityEngine;

// �÷��̾� �ν��Ͻ� ���ټ���
// : GameManager�� Instance(�޼���) -> _instance(�ν��Ͻ�) -> PlayerInstance(�޼���) -> _player(�ν��Ͻ�)
// (ex.GameManager.Instance.PlayerInstance.playerController)
public class PlayerManager : MonoBehaviour
{
    public PlayerController controller;
    public Animator animator;



    private void Awake()
    {
        GameManager.Instance.PlayerManager = this;

        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();


        DontDestroyOnLoad(gameObject);
    }
}