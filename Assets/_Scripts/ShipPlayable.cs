using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Behaviors/Playable")]
public class ShipPlayable : ShipBehavior
{

    private bool isMove = false;
    public override void Behavior(ShipController shipController)
    {
        isMove = GameInput.Instance.InputAxisNormalized().y > 0;

        if (isMove)
        {
            Forward(shipController);
        }
        else
        {
            shipController.ShipRigidbody.velocity = Vector2.zero;
        }

        float rotateInput = GameInput.Instance.InputAxisNormalized().x;
        if (rotateInput != 0)
        {
            Rotate(shipController, rotateInput);
        }
        

    }

    void Forward(ShipController shipController)
    {
        Vector3 force = shipController.transform.up * shipController.SpeedMovement;
        shipController.ShipRigidbody.AddForce(force.normalized, ForceMode2D.Force);
    }
    void Rotate(ShipController shipController, float deg)
    {
        float rotation = shipController.ShipRigidbody.rotation;
        float speedRotation = shipController.SpeedRotation;
        if (isMove) speedRotation = speedRotation / 2;
        shipController.ShipRigidbody.MoveRotation(rotation + (-deg * speedRotation));
    }
}
