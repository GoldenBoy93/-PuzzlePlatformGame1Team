using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    public Image healthImage;
    public Image staminaImage;

    private PlayerViewModel viewModel;
    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(PlayerViewModel vm)
    {
        viewModel = vm;

        viewModel.Health.Subscribe(h => healthImage.fillAmount = (float)h / vm.MaxHealth).AddTo(disposables);
        viewModel.Stamina.Subscribe(s => staminaImage.fillAmount = (float)s / vm.MaxStamina).AddTo(disposables);
    }

    void OnDestroy() => disposables.Dispose();
}