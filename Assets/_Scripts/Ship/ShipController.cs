using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected float delayAttack;
    [SerializeField] protected int damage;
    protected Transform cannon;

    public int Damage { get { return damage; } }
    protected bool canAttack = true;
    protected bool canMove = true;
    protected bool isDead = false;

    private Rigidbody2D shipRigidbody;
    protected Rigidbody2D ShipRigidbody { get { return shipRigidbody; } }

    protected ShipHealth shipHealth;

    private void OnEnable()
    {
        TryGetComponent(out shipRigidbody);
        TryGetComponent(out shipHealth);
        cannon = transform.Find("Cannon");
    }


    protected void Forward()
    {
        if (!canMove) return;
        Vector3 force = transform.up * speedMovement;

        if(shipRigidbody) shipRigidbody.AddForce(force, ForceMode2D.Force);
    }

    protected bool CheckObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1.2f, LayerMask.GetMask("Obstacle"));
        Debug.DrawRay(transform.position, transform.up * 1.2f, Color.red);

        return hit.collider;
    }

    protected IEnumerator DelayAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(delayAttack);
        canAttack = true;
        shipRigidbody.freezeRotation = false;

    }
}
