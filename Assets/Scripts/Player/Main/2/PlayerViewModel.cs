using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerViewModel //값이 바뀌면 자동으로 구독자에게 알림
{
    public PlayerModel model;

    public Action addItem;



    public ReactiveDictionary<ItemData, int> Inventory { get; private set; }
    public ReactiveProperty<int> Health { get; private set; }
    public int MaxHealth { get; private set; }

    // ReactiveProperty를 통해 UI 자동 갱신
    public ReactiveProperty<int> Stamina { get; private set; }
    public int MaxStamina => model.MaxStamina;
    // 인벤토리: 아이템 이름과 개수를 반응형으로



    // 스태미나 자동 회복/소모 관리용
    private IDisposable staminaRecoverLoop;

    public PlayerViewModel(PlayerModel model)
    {
        this.model = model;
        Health = new ReactiveProperty<int>(model.Health);
        Stamina = new ReactiveProperty<int>(model.Stamina);
        Inventory = new ReactiveDictionary<ItemData, int>(model.Inventory.Items);

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
        staminaRecoverLoop = Observable.Interval(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                if (model.Stamina < model.MaxStamina) // 최대치 제한
                {
                    model.Stamina += 10;
                    Stamina.Value = model.Stamina;
                }
            });
    }

    // 인벤토리 조작 메서드 (View에서 직접 호출 가능)
    public void AddItem(ItemData data, int amount = 1)
    {
        Inventory[data] = Inventory.ContainsKey(data) ? Inventory[data] + amount : amount;
    }

    public void RemoveItem(ItemData data, int amount = 1)
    {
        if (Inventory.ContainsKey(data))
        {
            Inventory[data] -= amount;
            if (Inventory[data] <= 0) Inventory.Remove(data);
        }
    }

    #region --- UIInventoryView + ItemSlot ---
    #endregion


    // ViewModel이 더 이상 필요 없을 때 정리
    public void Dispose()
    {
        staminaRecoverLoop?.Dispose();
    }


    //// 현재 장비
    //private Equip _currentEquip;
    //public IObservable<Equip> CurrentEquipChanged => _currentEquipSubject;
    //private readonly Subject<Equip> _currentEquipSubject = new Subject<Equip>();
    //
    //public void Equip(Equip newEquip)
    //{
    //    _currentEquip = newEquip;
    //    _currentEquipSubject.OnNext(newEquip); // 구독자에게 통보
    //}
    //
    //public void Unequip()
    //{
    //    _currentEquip = null;
    //    _currentEquipSubject.OnNext(null);
    //}
}