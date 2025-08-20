using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("Player");
                    _instance = go.AddComponent<Player>();
                }
            }
            return _instance;
        }
    }

    public PlayerController _controller;
    public Animator _animator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 이미 다른 있으면 파괴
            return;
        }
        _instance = this; // 여기서 등록
        DontDestroyOnLoad(gameObject);

        _controller = SafeFetchHelper.GetOrError<PlayerController>(gameObject);
        _animator = SafeFetchHelper.GetOrError<Animator>(gameObject);
    }
}