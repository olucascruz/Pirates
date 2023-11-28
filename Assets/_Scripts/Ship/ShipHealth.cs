using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Image healthBarImage;
    private int currentHealth;
    private bool isUnhittable;


    public int CurrentHealth { get { return currentHealth; } }

    private void OnEnable()
    {
        isUnhittable = false;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    /// <summary>
    /// Get health as fraction to use on health UI slider 
    /// </summary>
    public float GetHealthAsFraction()
    {
        return Mathf.Clamp01((float) currentHealth / maxHealth);
    }

    /// <summary>
    /// Updades health by decreasing the amount of damage taken
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (!isUnhittable) { 
            currentHealth -= damage;
            UpdateHealthUI();
            StartCoroutine(DelayUnhittable());
        }
    }

    /// <summary>
    /// Updades health UI slider
    /// </summary>
    void UpdateHealthUI()
    {
        if(healthBarImage)healthBarImage.fillAmount = GetHealthAsFraction();
    }

    /// <summary>
    /// Delay to take damage
    /// </summary>
    IEnumerator DelayUnhittable()
    {
        isUnhittable = true;
        yield return new WaitForSeconds(1f);
        isUnhittable = false;
    }
}
