using System;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class HealthSystem : MonoBehaviour
    {
        [SerializeField]private int currentHealth;
        public bool isDead { get; private set; } = false;
        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (value>maxHealth)
                {
                    currentHealth = maxHealth;
                }else if (value<0)
                {
                    currentHealth = 0;
                }
                else
                {
                    currentHealth = value;
                }
            }
        }

        [SerializeField]private int maxHealth;

        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                if (value<0)
                {
                    maxHealth = 0;
                }
                else
                {
                    maxHealth = value;
                }
            }
        }
        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            OnTakeDamage(amount);
            if (CurrentHealth==0 && !isDead)
            {
                OnDie();
                isDead = true;
            }
        }

        public abstract void OnTakeDamage(int amount);

        public abstract void OnDie();

        protected void RemoveHoles()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.ReturnToPool();
            }
        }
    }
}