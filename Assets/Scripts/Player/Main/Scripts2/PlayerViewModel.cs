using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerViewModel //���� �ٲ�� �ڵ����� �����ڿ��� �˸�
{
    private PlayerModel model;

    // ReactiveProperty�� ���� UI �ڵ� ����
    public ReactiveProperty<int> Health { get; private set; }
    public ReactiveProperty<int> Stamina { get; private set; }

    // �κ��丮: ������ �̸��� ������ ����������

    public ReactiveDictionary<string, int> Inventory { get; private set; }


    // ���¹̳� �ڵ� ȸ��/�Ҹ� ������
    private IDisposable staminaRecoverLoop;

    public PlayerViewModel(PlayerModel model)
    {
        this.model = model;
        Health = new ReactiveProperty<int>(model.Health);
        Stamina = new ReactiveProperty<int>(model.Stamina);
        Inventory = new ReactiveDictionary<string, int>(model.Inventory.Items);

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
        staminaRecoverLoop = Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                if (model.Stamina < model.MaxStamina) // �ִ�ġ ����
                {
                    model.Stamina++;
                    Stamina.Value = model.Stamina;
                }
            });
    }

    // �κ��丮 ���� �޼��� (View���� ���� ȣ�� ����)
    public void AddItem(string itemName, int amount = 1)
    {
        model.Inventory.AddItem(itemName, amount);
        Inventory[itemName] = model.Inventory.Items[itemName]; // ViewModel���� �ݿ�
    }
    public void RemoveItem(string itemName, int amount = 1)
    {
        model.Inventory.RemoveItem(itemName, amount);

        if (model.Inventory.Items.ContainsKey(itemName))
            Inventory[itemName] = model.Inventory.Items[itemName];
        else
            Inventory.Remove(itemName);
    }

    // ViewModel�� �� �̻� �ʿ� ���� �� ����
    public void Dispose()
    {
        staminaRecoverLoop?.Dispose();
    }
}