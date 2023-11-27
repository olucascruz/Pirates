using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum EnemyState
{
    STARTING, CHASE, ATTACK, AVOID, DISTANCIATE
}

public enum EnemyType
{
    CHASER, SHOTTER
}
public class ShipEnemy : ShipController
{
    EnemyState state = EnemyState.CHASE;
    [SerializeField] private EnemyType type;
    public EnemyType Type { get { return type; } }
    public EnemyState State { get { return state; } }

    private bool isAvoiding;
    private Vector3 dirToAvoid = Vector3.zero;

    public void OnActivate()
    {
        state = EnemyState.STARTING;
        isDead = false;
        canMove = true;
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Limits"));
        StartCoroutine(TimeStarting());
    }

    private IEnumerator TimeStarting()
    {
        yield return new WaitForSeconds(1.8f);
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Limits"), false);
        state = EnemyState.CHASE;
    }


    private void FixedUpdate()
    {
        if (isDead) return;
        if (shipHealth.CurrentHealth <= 0 && !isDead)
        {
            StartCoroutine(Death());
        }
        Behavior();
    }
    public void Behavior()
    {
        if (GameManager.Instance.State != StateGame.PLAY) return;
        GetComponent<BoxCollider2D>().enabled = canAttack;
        Vector3 directionToPlayer = GetDirectionToPlayer();
        RaycastHit2D canSeeTarget;

        switch (state)
        {
            case EnemyState.STARTING:
                Quaternion targetRotation = Quaternion.LookRotation(transform.forward, directionToPlayer);
                transform.rotation = targetRotation;
                Forward();
                break;

            case EnemyState.ATTACK:
                canSeeTarget = LookToDirection(directionToPlayer);
                if (canSeeTarget.collider && canAttack) Attack();
                break;

            case EnemyState.CHASE:
                canSeeTarget = LookToDirection(directionToPlayer);
                if (canAttack) Chase();
                break;
            case EnemyState.AVOID:
                ChoiceDirectionToAvoid();
                if (dirToAvoid != Vector3.zero) Forward();
                break;

            case EnemyState.DISTANCIATE:
                canSeeTarget = LookToDirection(-directionToPlayer);
                Forward();
                break;
        }

    }


    private void ChoiceDirectionToAvoid()
    {
        CheckObstacles();
        LayerMask LayerToAvoid = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Limits");
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, -transform.right, 3f, LayerToAvoid);
        Debug.DrawRay(transform.position, -transform.right * 3f, Color.yellow);

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, transform.right, 3f, LayerToAvoid);
        Debug.DrawRay(transform.position, transform.right * 3f, Color.yellow);
        if (!isAvoiding)
        {
            isAvoiding = true;

            if (!hitRight.collider && !hitLeft.collider)
            {
                float distanceRigh = Vector3.Distance(transform.position + Vector3.right, GetPlayerPosition());
                float distanceLeft = Vector3.Distance(transform.position + (-Vector3.right), GetPlayerPosition());

                if (distanceLeft < distanceRigh)
                {
                    dirToAvoid = -transform.right;
                }
                else
                {
                    dirToAvoid = transform.right;
                }
                StartCoroutine(TimeToAvoid(6f));

                return;
            }
            StartCoroutine(TimeToAvoid());


            if (!hitRight.collider && hitLeft.collider)
            {
                dirToAvoid = transform.right;
                return;
            }
            if (!hitLeft.collider && hitRight.collider)
            {
                dirToAvoid = -transform.right;
                return;
            }

            if (hitRight.collider && hitRight.collider)
            {
                dirToAvoid = -transform.up;
                return;
            }
        }
        LookToDirection(dirToAvoid);
    }

    private IEnumerator TimeToAvoid(float time = 2f)
    {
        yield return new WaitForSeconds(time);
        dirToAvoid = Vector3.zero;
        isAvoiding = false;
        state = EnemyState.CHASE;

    }




    private void Chase()
    {
        if(!CheckObstacles()) Forward();
        if (CheckObstacles())
        {
            state = EnemyState.AVOID;
        }
       
    }
    private void Attack()
    {
        if (type == EnemyType.CHASER)
        {
            ChaserAttack();
        }
        if (type == EnemyType.SHOTTER && cannon != null)
        {
            cannon.GetComponent<Cannon>().Shoot(damage, gameObject);
        }
        StartCoroutine(DelayAttack());

    }

    private void ChaserAttack()
    {
        ShipRigidbody.freezeRotation = true;
        Vector3 force = transform.up * speedMovement;
        ShipRigidbody.AddForce(force * 3, ForceMode2D.Impulse);

    }
    

    private Vector3 GetDirectionToPlayer()
    {
        GameObject[] playerObject = GameObject.FindGameObjectsWithTag("Player");
        if (playerObject[0]!= null)
        {
           Transform playerTransform = playerObject[0].transform;
           Vector3 directionToPlayer = playerTransform.position - transform.position;
           return directionToPlayer.normalized;
        }

        return new Vector3();
    }

    private Vector3 GetPlayerPosition()
    {
        GameObject[] playerObject = GameObject.FindGameObjectsWithTag("Player");
        if (playerObject[0] != null)
        {
            return playerObject[0].transform.position;
        }

        return new Vector3();

    }

    private RaycastHit2D LookToDirection(Vector3 direction)
    {
        
 
        Quaternion targetRotationDir = Quaternion.LookRotation(transform.forward, direction);
        transform.rotation = (Quaternion.Slerp(transform.rotation,
                                                    targetRotationDir,
                                                    speedRotation * Time.deltaTime));
        
        

        float raycastDistance = 30f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
                                             transform.up, 
                                             raycastDistance, 
                                             LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, 
                      transform.up * raycastDistance, 
                      Color.red);

        return hit;
    }

    IEnumerator Death()
    {
        isDead = true;
        canMove = false;
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.AddPoint();
        gameObject.SetActive(false);

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            state = EnemyState.ATTACK;
        }
        
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ShipRigidbody.freezeRotation = false;
            if(state == EnemyState.ATTACK) state = EnemyState.CHASE;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" &&
            state==EnemyState.ATTACK  &&
            type == EnemyType.CHASER)
        {
            collision.gameObject.GetComponent<ShipHealth>().TakeDamage(Damage);

        }

        if (collision.gameObject.tag == "Player" &&
            state != EnemyState.ATTACK)
        {
            StartCoroutine(StartDistanciateState());
        }
    }

    IEnumerator StartDistanciateState()
    {
        yield return new WaitForSeconds(0.5f);
        state = EnemyState.DISTANCIATE;
        yield return new WaitForSeconds(4f);
        state = EnemyState.CHASE;

    }
}
