using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * <summary>
 * Class base to Ships
 * </summary>
 */
public class ShipController : MonoBehaviour
{
    [SerializeField] protected float speedMovement;
    [SerializeField] protected float speedRotation;
    [SerializeField] protected float delayAttack;
    [SerializeField] protected int damage;
    [SerializeField] protected float distanceToCheckObstaclesForward;

    protected Transform cannon;
    protected bool canAttack = true;
    protected bool canMove = true;
    protected bool isDead = false;

    protected Rigidbody2D shipRigidbody;
    protected ShipHealth shipHealth;

    private void OnEnable()
    {
        TryGetComponent(out shipRigidbody);
        TryGetComponent(out shipHealth);
        cannon = transform.Find("Cannon");
    }

    /// <summary>
    /// Move forward using AddForce
    /// </summary>
    protected void Forward()
    {
        if (!canMove) return;
        Vector3 force = transform.up * speedMovement;

        if(shipRigidbody) shipRigidbody.AddForce(force, ForceMode2D.Force);
    }


    /// <summary>
    /// Check Obstacles Forward using raycast
    /// </summary>
    protected bool CheckObstaclesForward()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distanceToCheckObstaclesForward, LayerMask.GetMask("Obstacle"));
        Debug.DrawRay(transform.position, transform.up * distanceToCheckObstaclesForward, Color.red);

        return hit.collider;
    }

    /// <summary>
    /// Create a delay between attacks
    /// </summary>
    protected IEnumerator DelayAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(delayAttack);
        canAttack = true;
        shipRigidbody.freezeRotation = false;

    }
}
