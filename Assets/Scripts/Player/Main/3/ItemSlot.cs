using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI label;

    private ItemSlotViewModel viewModel;
    private System.Action<int> onClick;
    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(ItemSlotViewModel vm, System.Action<int> onClick)
    {
        this.viewModel = vm;
        this.onClick = onClick;

        // 슬롯 데이터가 바뀌면 UI 갱신
        viewModel.Slot.Subscribe(_ => Refresh()).AddTo(disposables);
    }

    public void Clear()
    {
        viewModel = null;
        label.text = "";
    }

    public void Refresh()
    {
        if (viewModel != null)
            label.text = viewModel.LabelText;
    }

    public void OnClick()
    {
        onClick?.Invoke(index);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
