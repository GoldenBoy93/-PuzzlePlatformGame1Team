using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ComboLock : MonoBehaviour
{
    [Header("Combination")]
    [Tooltip("정답 (자릿수 = Wheels 길이). 예: 4자리면 1234")]
    public string combination = "1234";
    [Tooltip("휠이 바뀔 때마다 자동 체크(=즉시 오픈)")]
    public bool autoCheckOnChange = false;

    [Header("References")]
    public DigitWheel[] wheels;     // 4개 휠(0~9)
    public Animator lockAnimator;   // 자물쇠 애니메이터(선택) - 열림 연출

    [Header("Events")]
    public UnityEvent OnOpen;       // 정답일 때만 호출됨
    public UnityEvent OnWrong;      // 오답일 때 피드백

    bool unlocked = false;

    void Awake()
    {
        // 각 휠에 변경 콜백 연결
        foreach (var w in wheels)
            w.onChanged += OnWheelChanged;
    }

    void OnWheelChanged()
    {
        if (unlocked) return;
        if (autoCheckOnChange) TryOpen();
    }

    public void TryOpen() // '확인 버튼/손잡이 당기기'에 연결
    {
        if (unlocked) return;

        string current = string.Concat(wheels.Select(w => w.Value.ToString()));
        bool ok = current == combination;

        if (ok)
        {
            unlocked = true;
            // 자물쇠 자체 애니메이션 먼저
            if (lockAnimator) lockAnimator.SetTrigger("Open");
            // 연출이 필요 없으면 곧바로 이벤트
            if (!lockAnimator) OnOpen?.Invoke();
        }
        else
        {
            OnWrong?.Invoke();
        }
    }

    // 애니메이션 이벤트용(자물쇠 클립 중간에 "OpenEvent" 이벤트를 박아 호출)
    public void OpenEvent()
    {
        if (unlocked)
            OnOpen?.Invoke();
    }

    // 모든 휠 0으로 리셋
    public void ResetLock()
    {
        unlocked = false;
        foreach (var w in wheels) w.SetValue(0, instant: true);
    }
}
