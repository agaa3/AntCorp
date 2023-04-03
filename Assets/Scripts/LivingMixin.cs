using System;
using UnityEngine;

public class LivingMixin : MonoBehaviour
{
    public float MaxHealth => maxHealth;
    public float Health => health;
    public bool IsAlive => health > float.Epsilon;

    public Action<float, float> OnTakeDamage;
    public Action<float, float> OnHeal;
    public Action OnDie;


    public void DealDamage(float amount)
    {
        float value = health - amount;
        if (value < float.Epsilon)
        {
            health = 0;
            OnDie?.Invoke();
        }

        else if (value != health)
        {
            OnTakeDamage?.Invoke(health, value);
            health = value;
        }
    }
    public void Heal(float amount)
    {
        float value = health + amount;
        if (value > MaxHealth)
        {
            value = maxHealth;
        }
        else if (value != health)
        {
            OnHeal?.Invoke(health, value);
            health = value;
        }
    }
    public void Revive()
    {
        health = default;
    }

    private void Awake()
    {
        health = defaultHealth;
    }


    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] float defaultHealth;
}
