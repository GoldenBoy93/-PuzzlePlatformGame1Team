using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerViewModel //���� �ٲ�� �ڵ����� �����ڿ��� �˸�
{
    public PlayerModel model;

    public Action addItem;



    public ReactiveDictionary<ItemData, int> Inventory { get; private set; }
    public ReactiveProperty<int> Health { get; private set; }
    public int MaxHealth { get; private set; }

    // ReactiveProperty�� ���� UI �ڵ� ����
    public ReactiveProperty<int> Stamina { get; private set; }
    public int MaxStamina => model.MaxStamina;
    // �κ��丮: ������ �̸��� ������ ����������



    // ���¹̳� �ڵ� ȸ��/�Ҹ� ������
    private IDisposable staminaRecoverLoop;

    public PlayerViewModel(PlayerModel model)
    {
        this.model = model;
        Health = new ReactiveProperty<int>(model.Health);
        Stamina = new ReactiveProperty<int>(model.Stamina);
        Inventory = new ReactiveDictionary<ItemData, int>(model.Inventory.Items);

        // ���¹̳� �ڵ� ȸ�� ����
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
                if (model.Stamina < model.MaxStamina) // �ִ�ġ ����
                {
                    model.Stamina += 10;
                    Stamina.Value = model.Stamina;
                }
            });
    }

    // �κ��丮 ���� �޼��� (View���� ���� ȣ�� ����)
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


    // ViewModel�� �� �̻� �ʿ� ���� �� ����
    public void Dispose()
    {
        staminaRecoverLoop?.Dispose();
    }


    //// ���� ���
    //private Equip _currentEquip;
    //public IObservable<Equip> CurrentEquipChanged => _currentEquipSubject;
    //private readonly Subject<Equip> _currentEquipSubject = new Subject<Equip>();
    //
    //public void Equip(Equip newEquip)
    //{
    //    _currentEquip = newEquip;
    //    _currentEquipSubject.OnNext(newEquip); // �����ڿ��� �뺸
    //}
    //
    //public void Unequip()
    //{
    //    _currentEquip = null;
    //    _currentEquipSubject.OnNext(null);
    //}
}