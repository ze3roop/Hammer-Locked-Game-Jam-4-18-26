using System;

public class HealthEvents
{
    /* 
        Managers subscribe to OnStatGained and OnStatLost
        then from there they EXCLUSIVELY call OnStatChanged
        because they handle the logic for the argument in StatChanged(T newValue)
    */

    //Typically only called by non-manager objects (literally anything else) 
    public event Action<float> OnHealthGained;
    public void HealthGained(float amount) {
        OnHealthGained?.Invoke(amount);
    }
    //Typically only called by non-manager objects (literally anything else) 
    public event Action<float> OnHealthLost;
    public void HealthLost(float amount) {
        OnHealthLost?.Invoke(amount);
    }
    //Exclusively called by Manager, but subscribed to by everything that needs it
    public event Action<float> OnHealthChanged;
    public void HealthChanged(float newValue) {
        OnHealthChanged?.Invoke(newValue);
    }

    //typically subscribed to by UI elements that need to set the max of its health bar for instance 
    public event Action<float> OnSetMax;
    public void SetMax(float newMax) {
        OnSetMax?.Invoke(newMax);
    }
}
