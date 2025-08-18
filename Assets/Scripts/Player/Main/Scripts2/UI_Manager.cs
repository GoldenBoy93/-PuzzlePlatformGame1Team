using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    public PlayerModel Model { get; private set; }
    public PlayerViewModel ViewModel { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Model = new PlayerModel();
        ViewModel = new PlayerViewModel(Model);
    }
}
