using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class ShipPlayable : ShipController
{

    private bool isMove = false;
    [SerializeField] private GameObject TrippleCannon;



    private void Update()
    {
        if (GameManager.Instance.State != StateGame.PLAY) return;

        canMove = !CheckObstacles();
        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State != StateGame.PLAY || isDead) return;

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
            ShipRigidbody.velocity = Vector2.zero;
        }

        float rotateInput = GameInput.Instance.InputAxisNormalized().x;
        if (rotateInput != 0)
        {
            ShipRigidbody.freezeRotation = false;
            Rotate(rotateInput);
        }
        else
        {
            ShipRigidbody.freezeRotation = true;
        }

        if (GameInput.Instance.JIsPressed() && canAttack && cannon != null)
        {
            cannon.GetComponent<Cannon>().Shoot(damage, gameObject);
            StartCoroutine(DelayAttack());
        }

        if (GameInput.Instance.KIsPressed() && canAttack)
        {
            Cannon[] cannons = TrippleCannon.GetComponentsInChildren<Cannon>();
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
        float rotation = ShipRigidbody.rotation;
        float _speedRotation = speedRotation;
        if (isMove) _speedRotation = speedRotation / 2;
        ShipRigidbody.MoveRotation(rotation + (-deg * _speedRotation));
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
