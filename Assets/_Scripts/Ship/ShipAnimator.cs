using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimator : MonoBehaviour
{
    private Animator anim;
    private ShipHealth health;
    private TrailRenderer trailWater;


    void Start()
    {
        health = GetComponentInParent<ShipHealth>();
        trailWater = GetComponentInChildren<TrailRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("health",health.GetHealthAsFraction());
        trailWater.enabled = health.CurrentHealth >= 0;
        
    }
}
