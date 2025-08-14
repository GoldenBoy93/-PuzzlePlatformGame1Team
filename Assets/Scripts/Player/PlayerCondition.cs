using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    //Condition hunger { get { return uiCondition.hunger; } }
    //Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay; // 배고픔이 없을 때 체력 감소량
    public event Action onTakeDamage;

    private void Update()
    {
        // Intro씬에서 null 참조 오류 발생하지 않게
        if (health == null) return;

        //hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        //stamina.Add(stamina.passiveValue * Time.deltaTime);

        //if (hunger.curValue < 0f)
        //{
        //    health.Subtract(noHungerHealthDecay * Time.deltaTime);
        //}

        health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    //public void Eat(float amount)
    //{
    //    hunger.Add(amount);
    //}

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
        GameManager.Instance.GameOver();
    }

    // UIManager가 호출하여 체력바 스크립트를 전달해주는 함수
    public void SetUICondition(UICondition newHealthBarUI)
    {
        this.uiCondition = newHealthBarUI;
    }
}