using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerViewModel //값이 바뀌면 자동으로 구독자에게 알림
{
    private PlayerModel model;

    // ReactiveProperty를 통해 UI 자동 갱신
    public ReactiveProperty<int> Health { get; private set; }
    public ReactiveProperty<int> Stamina { get; private set; }

    // 인벤토리: 아이템 이름과 개수를 반응형으로

    public ReactiveDictionary<string, int> Inventory { get; private set; }


    // 스태미나 자동 회복/소모 관리용
    private IDisposable staminaRecoverLoop;

    public PlayerViewModel(PlayerModel model)
    {
        this.model = model;
        Health = new ReactiveProperty<int>(model.Health);
        Stamina = new ReactiveProperty<int>(model.Stamina);
        Inventory = new ReactiveDictionary<string, int>(model.Inventory.Items);

        // 스태미나 자동 회복 시작
        RecoveryStamina();
    }

    public void TakeDamage(int amount)
    {
        model.Health = Mathf.Max(0, model.Health - amount);
        Health.Value = model.Health;
    }

    public void Heal(int amount)
    {
        model.Health = Mathf.Min(model.MaxHealth, model.Health + amount);
        Health.Value = model.Health;
    }

    public void ConsumeStamina(int amount)
    {
        model.Stamina = Mathf.Max(0, model.Stamina - amount);
        Stamina.Value = model.Stamina;
    }

    private void RecoveryStamina()
    {
        staminaRecoverLoop = Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                if (model.Stamina < model.MaxStamina) // 최대치 제한
                {
                    model.Stamina++;
                    Stamina.Value = model.Stamina;
                }
            });
    }

    // 인벤토리 조작 메서드 (View에서 직접 호출 가능)
    public void AddItem(string itemName, int amount = 1)
    {
        model.Inventory.AddItem(itemName, amount);
        Inventory[itemName] = model.Inventory.Items[itemName]; // ViewModel에도 반영
    }
    public void RemoveItem(string itemName, int amount = 1)
    {
        model.Inventory.RemoveItem(itemName, amount);

        if (model.Inventory.Items.ContainsKey(itemName))
            Inventory[itemName] = model.Inventory.Items[itemName];
        else
            Inventory.Remove(itemName);
    }

    // ViewModel이 더 이상 필요 없을 때 정리
    public void Dispose()
    {
        staminaRecoverLoop?.Dispose();
    }
}