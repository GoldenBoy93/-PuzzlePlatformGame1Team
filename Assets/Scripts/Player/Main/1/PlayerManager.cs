using System;
using UnityEngine;

// 플레이어 인스턴스 접근순서
// : GameManager의 Instance(메서드) -> _instance(인스턴스) -> PlayerInstance(메서드) -> _player(인스턴스)
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