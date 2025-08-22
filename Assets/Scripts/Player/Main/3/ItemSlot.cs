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

        vm.LabelText.Subscribe(text => label.text = text).AddTo(disposables);
        vm.Icon.Subscribe(sprite =>
        {
            icon.sprite = sprite;
            icon.enabled = sprite != null;
        }).AddTo(disposables);

        var button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(() => onClick?.Invoke(index));
    }

    public void SetImageActive(bool active) => icon.enabled = active;

    private void OnDestroy() => disposables.Dispose();
}