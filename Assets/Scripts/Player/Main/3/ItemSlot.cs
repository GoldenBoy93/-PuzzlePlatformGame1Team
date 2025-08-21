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
    public TextMeshProUGUI label;
    public int index;

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(ItemSlotViewModel vm, Action<int> onClick)
    {
        // LabelText 자동 갱신
        vm.LabelText
            .Subscribe(text => label.text = text)
            .AddTo(disposables);

        // 버튼 클릭 이벤트
        var button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(() => onClick?.Invoke(index));
    }

    private void OnDestroy() => disposables.Dispose();
}