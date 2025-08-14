using System;
using UnityEngine;

// �÷��̾� �ν��Ͻ� ���ټ���
// : GameManager�� Instance(�޼���) -> _instance(�ν��Ͻ�) -> PlayerInstance(�޼���) -> _player(�ν��Ͻ�)
// (ex.GameManager.Instance.PlayerInstance.playerController)
public class Player : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerCondition playerCondition;
    public Rigidbody playerRigidbody;
    public Animator playerAnimator;

    private void Start()
    {
        // �̱���Ŵ����� Player�� ������ �� �ְ� �����͸� �ѱ��.
        GameManager.Instance.PlayerInstance = this;
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();

        // ������ UICondition�� ã�� playerCondition�� �Ҵ�
        // �÷��̾�� UICondition�� ���� ������ ��쿡�� �����ϰ� ���� ����
        playerCondition = GetComponent<PlayerCondition>();
        if (playerCondition != null)
        {
            playerCondition.uiCondition = FindObjectOfType<UICondition>();
        }
    }
}