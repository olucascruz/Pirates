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
        currentHealth = maxHealth;
        UpdateHealth();
    }
    public float GetHealthAsFraction()
    {
        return Mathf.Clamp01((float) currentHealth / maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (!isUnhittable) { 
            currentHealth -= damage;
            UpdateHealth();
        }
    }
    void UpdateHealth()
    {
        if(healthBarImage)healthBarImage.fillAmount = GetHealthAsFraction();
    }

    IEnumerator BeUnhittable()
    {
        isUnhittable = true;
        yield return new WaitForSeconds(1f);
        isUnhittable = false;
    }
}
