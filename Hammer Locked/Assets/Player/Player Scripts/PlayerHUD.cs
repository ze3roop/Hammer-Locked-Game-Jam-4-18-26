using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image HealthBar; 
    private float maxHealth; 

    public void OnEnable()
    {
        GameEventsManager.Instance.healthEvents.OnSetMax += OnSetMax; 
        GameEventsManager.Instance.healthEvents.OnHealthChanged += OnHealthChanged; 
    }
    public void OnDisable()
    {
        GameEventsManager.Instance.healthEvents.OnSetMax -= OnSetMax; 
        GameEventsManager.Instance.healthEvents.OnHealthChanged -= OnHealthChanged; 
    }

    private void OnSetMax(float maxHealth)
    {
        this.maxHealth = maxHealth; 
        HealthBar.fillAmount = maxHealth; 
    }

    private void OnHealthChanged(float currentHealth)
    {
        Debug.Log("ON HEALTH CHANGED");
        HealthBar.fillAmount = currentHealth / maxHealth; 
    }
}
