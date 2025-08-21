using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerViewModel : IDisposable
{
    private readonly PlayerModel model;
    private IDisposable staminaRecoverLoop;

    public ReactiveProperty<int> Health { get; }
    public int MaxHealth => model.MaxHealth;

    public ReactiveProperty<int> Stamina { get; }
    public int MaxStamina => model.MaxStamina;

    // 플레이어가 인벤토리를 '소유' (분리된 VM)
    public InventoryViewModel InventoryVM { get; }

    public PlayerViewModel(PlayerModel model)
    {
        this.model = model ?? throw new ArgumentNullException(nameof(model));

        Health = new ReactiveProperty<int>(this.model.Health);
        Stamina = new ReactiveProperty<int>(this.model.Stamina);

        InventoryVM = new InventoryViewModel(this.model.Inventory);

        StartStaminaRecovery();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        model.Health = Mathf.Max(0, model.Health - amount);
        Health.Value = model.Health;
        Debug.Log(Health.Value);
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        model.Health = Mathf.Min(model.MaxHealth, model.Health + amount);
        Health.Value = model.Health;
    }

    public void ConsumeStamina(int amount)
    {
        if (amount <= 0) return;
        model.Stamina = Mathf.Max(0, model.Stamina - amount);
        Stamina.Value = model.Stamina;
    }

    // 프레임 독립 회복 (초당 10 회복 예시)
    private void StartStaminaRecovery()
    {
        const float recoverPerSec = 10f;

        staminaRecoverLoop = Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (model.Stamina < model.MaxStamina)
                {
                    int delta = Mathf.CeilToInt(recoverPerSec * Time.deltaTime);
                    model.Stamina = Mathf.Min(model.MaxStamina, model.Stamina + delta);
                    Stamina.Value = model.Stamina;
                }
            });
    }

    public void Dispose()
    {
        staminaRecoverLoop?.Dispose();
        InventoryVM?.Dispose();
        Health?.Dispose();
        Stamina?.Dispose();
    }
}