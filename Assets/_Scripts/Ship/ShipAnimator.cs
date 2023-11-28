using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * <summary>
 * Class used to control ship animation
 * </summary>
 */
public class ShipAnimator : MonoBehaviour
{
    private Animator anim;
    private ShipHealth health;
    private TrailRenderer trailWater;
    private Transform explosion;


    void Start()
    {
        Transform trail = transform.Find("Trail");
        transform.parent.TryGetComponent(out health);
        trail.TryGetComponent(out trailWater);
        TryGetComponent(out anim);
        explosion = transform.Find("Explosion");
    }

    void Update()
    {
        if(anim != null)anim.SetFloat("health",health.GetHealthAsFraction());
        if (trailWater != null) trailWater.enabled = health.CurrentHealth >= 0;
        if(explosion != null) explosion.gameObject.SetActive(health.CurrentHealth <= 0);
        
    }
}
