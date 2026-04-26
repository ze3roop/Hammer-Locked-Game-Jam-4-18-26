using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] protected float startingHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth; 

    void Start()
    {
        currentHealth = startingHealth; 
        //typically subscribed to by UI elements that need to set the max of its health bar for instance 
        GameEventsManager.Instance.healthEvents.SetMax(maxHealth);
    }

    void OnEnable()
    {
        GameEventsManager.Instance.healthEvents.OnHealthGained += OnHealthGained;
        GameEventsManager.Instance.healthEvents.OnHealthLost += OnHealthLost;
    }
    void OnDisable()
    {
        GameEventsManager.Instance.healthEvents.OnHealthGained -= OnHealthGained;
        GameEventsManager.Instance.healthEvents.OnHealthLost -= OnHealthLost;
    }

    private void OnHealthGained(float gainedAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + gainedAmount, 0, maxHealth); 
        GameEventsManager.Instance.healthEvents.HealthChanged(currentHealth); 
    }
    private void OnHealthLost(float lostAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth - lostAmount, 0, maxHealth); 
        GameEventsManager.Instance.healthEvents.HealthChanged(currentHealth); 
    }
}
