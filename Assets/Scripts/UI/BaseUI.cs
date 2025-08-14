using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 UI의 기본 동작을 정의하는 추상 클래스
public abstract class BaseUI : MonoBehaviour
{
    protected UIManager uiManager;

    // 필요하다면 UIManager 메서드 호출을 가능하게 하기 위한 초기화 메서드
    // UIManager에서 각 UI에 Init 해줌 (여기선 받는것)
    // virtual이니 자식 클래스에서 override도 가능
    public virtual void Init(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }
}