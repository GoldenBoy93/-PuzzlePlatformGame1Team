using System;
using UnityEngine;

// 플레이어 인스턴스 접근순서
// : GameManager의 Instance(메서드) -> _instance(인스턴스) -> PlayerInstance(메서드) -> _player(인스턴스)
// (ex.GameManager.Instance.PlayerInstance.playerController)
public class Player : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerCondition playerCondition;
    public Rigidbody playerRigidbody;
    public Animator playerAnimator;

    private void Start()
    {
        // 싱글톤매니저에 Player를 참조할 수 있게 데이터를 넘긴다.
        GameManager.Instance.PlayerInstance = this;
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();

        // 씬에서 UICondition을 찾아 playerCondition에 할당
        // 플레이어보다 UICondition이 먼저 생성될 경우에도 안전하게 참조 가능
        playerCondition = GetComponent<PlayerCondition>();
        if (playerCondition != null)
        {
            playerCondition.uiCondition = FindObjectOfType<UICondition>();
        }
    }
}