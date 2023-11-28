using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/**
 * <summary>
 * Class sets playable ship by extending ship controller
 * </summary>
 */
public class ShipPlayable : ShipController
{

    private bool isMove = false;
   
    private void Update()
    {
        if (GameManager.Instance.State != GameState.PLAY) return;
        
        //Does not allow the player to force against the wall
        canMove = !CheckObstaclesForward();
        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State != GameState.PLAY || isDead) return;

        Behavior();
    }

    public void Behavior()
    {
        isMove = GameInput.Instance.InputAxisNormalized().y > 0;

        if (isMove)
        {
            Forward();
        }
        else
        {
            shipRigidbody.velocity = Vector2.zero;
        }

        float rotateInput = GameInput.Instance.InputAxisNormalized().x;
        if (rotateInput != 0)
        {
            shipRigidbody.freezeRotation = false;
            Rotate(rotateInput);
        }
        else
        {
            shipRigidbody.freezeRotation = true;
        }

        if (GameInput.Instance.JIsPressed() && canAttack && cannon != null)
        {
            cannon.TryGetComponent(out Cannon cannonInstance);
            if (cannonInstance != null) cannonInstance.Shoot(damage, gameObject);
            StartCoroutine(DelayAttack());
        }

        if (GameInput.Instance.KIsPressed() && canAttack)
        {
            Transform trippleCannon = transform.Find("trippleCannon");

            if (!trippleCannon) return;

            Cannon[] cannons = trippleCannon.GetComponentsInChildren<Cannon>();
            
            if (cannons[0] != null) { 
                foreach (var _cannon in cannons)
                {
                    _cannon.Shoot(damage, gameObject);
                }
            }
            StartCoroutine(DelayAttack());
        }

        CheckDeath();
    }

    
    private void Rotate(float deg)
    {
        float rotation = shipRigidbody.rotation;
        float _speedRotation = speedRotation;
        if (isMove) _speedRotation = speedRotation / 2;
        shipRigidbody.MoveRotation(rotation + (-deg * _speedRotation));
    }

    private void CheckDeath()
    {
        if (shipHealth.CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GameOver();
    }
}
