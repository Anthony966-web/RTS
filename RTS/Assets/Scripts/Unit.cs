using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float unitHealth;
    public float unitMaxHealth;

    public HealthTracker healthTracker;

    private void Start()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);

        unitHealth = unitMaxHealth;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);

        if (unitHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    public void TakeDamage(float damageToInflict)
    {
        unitHealth -= damageToInflict;
    }
}
