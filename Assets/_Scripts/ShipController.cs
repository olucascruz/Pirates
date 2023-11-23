using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float speedMovement;
    [SerializeField] private float speedRotation;

    public float SpeedMovement { get { return speedMovement; } }
    public float SpeedRotation { get { return speedRotation; } }


    [SerializeField] private ShipBehavior shipBehavior;
    private Rigidbody2D shipRigidbody;
    public Rigidbody2D ShipRigidbody { get { return shipRigidbody; } }

    private void Start()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        shipBehavior.Behavior(this);
    }
}
