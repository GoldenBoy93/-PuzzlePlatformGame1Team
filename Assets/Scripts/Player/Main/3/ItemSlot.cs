using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI label;
    public int index;

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(ItemSlotViewModel vm, int idx, Action<int> onClick)
    {
        index = idx;

        // LabelText �ڵ� ����
        vm.LabelText
            .Subscribe(text => {
                label.text = text;
                // LabelText�� ������ ������ ��
                icon.enabled = !string.IsNullOrEmpty(text);
            })
            .AddTo(disposables);

        var button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(() => onClick?.Invoke(index));
    }

    private void OnDestroy() => disposables.Dispose();
}