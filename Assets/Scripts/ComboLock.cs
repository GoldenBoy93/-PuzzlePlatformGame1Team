using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ComboLock : MonoBehaviour
{
    [Header("Combination")]
    [Tooltip("���� (�ڸ��� = Wheels ����). ��: 4�ڸ��� 1234")]
    public string combination = "1234";
    [Tooltip("���� �ٲ� ������ �ڵ� üũ(=��� ����)")]
    public bool autoCheckOnChange = false;

    [Header("References")]
    public DigitWheel[] wheels;     // 4�� ��(0~9)
    public Animator lockAnimator;   // �ڹ��� �ִϸ�����(����) - ���� ����

    [Header("Events")]
    public UnityEvent OnOpen;       // ������ ���� ȣ���
    public UnityEvent OnWrong;      // ������ �� �ǵ��

    bool unlocked = false;

    void Awake()
    {
        // �� �ٿ� ���� �ݹ� ����
        foreach (var w in wheels)
            w.onChanged += OnWheelChanged;
    }

    void OnWheelChanged()
    {
        if (unlocked) return;
        if (autoCheckOnChange) TryOpen();
    }

    public void TryOpen() // 'Ȯ�� ��ư/������ ����'�� ����
    {
        if (unlocked) return;

        string current = string.Concat(wheels.Select(w => w.Value.ToString()));
        bool ok = current == combination;

        if (ok)
        {
            unlocked = true;
            // �ڹ��� ��ü �ִϸ��̼� ����
            if (lockAnimator) lockAnimator.SetTrigger("Open");
            // ������ �ʿ� ������ ��ٷ� �̺�Ʈ
            if (!lockAnimator) OnOpen?.Invoke();
        }
        else
        {
            OnWrong?.Invoke();
        }
    }

    // �ִϸ��̼� �̺�Ʈ��(�ڹ��� Ŭ�� �߰��� "OpenEvent" �̺�Ʈ�� �ھ� ȣ��)
    public void OpenEvent()
    {
        if (unlocked)
            OnOpen?.Invoke();
    }

    // ��� �� 0���� ����
    public void ResetLock()
    {
        unlocked = false;
        foreach (var w in wheels) w.SetValue(0, instant: true);
    }
}
