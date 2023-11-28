using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/**
 * <summary>
 * Class sets enemies by extending ship controller
 * </summary>
 */
public class ShipEnemy : ShipController
{
    
    private EnemyState state = EnemyState.CHASE;

    private bool isAvoiding;
    private Vector3 dirToAvoid = Vector3.zero;
    [Header("Type")]
    [SerializeField] private EnemyType type;

    [Header("Distances to check to Avoid")]
    [SerializeField] private float distanceToCheckSides = 3f;

    [Header("Time")]
    [SerializeField] private float timeToAvoid = 2f;
    [SerializeField] private float timeToAvoidWhenHasObstacleOnTwoSides = 6f;

    [Header("Chaser ship param")]
    [SerializeField] private float speedMultipleForChaserAttack = 3f;

    /// <summary>
    /// Function that must be called by the enemy generator to execute the enemy's initial behavior
    /// </summary>
    public void OnActivate()
    {
        state = EnemyState.STARTING;
        isDead = false;
        canMove = true;
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Limits"));
        StartCoroutine(ChangeStartingStateToChaseState());
    }

    /// <summary>
    /// Change initial state to chase state allowing enemy to collide with map boundaries 
    /// </summary>
    private IEnumerator ChangeStartingStateToChaseState()
    {
        float timeIgnoringLimts = 1.8f;
        yield return new WaitForSeconds(timeIgnoringLimts);
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Limits"), false);
        state = EnemyState.CHASE;
    }


    private void FixedUpdate()
    {
        if (shipHealth.CurrentHealth <= 0 && !isDead)
        {
            StartCoroutine(Death());
        }
        if (isDead) return;

        DefineStateMachine();
    }

    public void DefineStateMachine()
    {
        if (GameManager.Instance.State != GameState.PLAY) return;
        TryGetComponent(out BoxCollider2D boxCollider);
        if( boxCollider != null) boxCollider.enabled = canAttack;
        
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
                if (canAttack)
                {
                    if (!CheckObstaclesForward()) Forward();
                    if (CheckObstaclesForward()) state = EnemyState.AVOID;
                }
                break;
            case EnemyState.AVOID:
                DefineDirectionToAvoid();
                if (dirToAvoid != Vector3.zero) Forward();
                break;

            case EnemyState.DISTANCIATE:
                canSeeTarget = LookToDirection(-directionToPlayer);
                Forward();
                break;
        }

    }

    /// <summary>
    /// Define direction to go to avoid obstacles using raycast
    /// </summary>
    private void DefineDirectionToAvoid()
    {
        CheckObstaclesForward();
        LayerMask LayerToAvoid = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Limits");
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, -transform.right, distanceToCheckSides, LayerToAvoid);
        Debug.DrawRay(transform.position, -transform.right * distanceToCheckSides, Color.yellow);

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, transform.right, distanceToCheckSides, LayerToAvoid);
        Debug.DrawRay(transform.position, transform.right * distanceToCheckSides, Color.yellow);
        if (!isAvoiding)
        {
            isAvoiding = true;

            if (!hitRight.collider && !hitLeft.collider)
            {
                float distanceRight = Vector3.Distance(transform.position + Vector3.right, GetPlayerPosition());
                float distanceLeft = Vector3.Distance(transform.position + (-Vector3.right), GetPlayerPosition());

                dirToAvoid = (distanceLeft < distanceRight) ? -transform.right : transform.right;
                StartCoroutine(TimeToAvoid(timeToAvoidWhenHasObstacleOnTwoSides));

                return;
            }
            else
            {
                StartCoroutine(TimeToAvoid(timeToAvoid));

                dirToAvoid = (hitRight.collider && !hitLeft.collider) ? -transform.right :
                             (!hitRight.collider && hitLeft.collider) ? transform.right :
                             (hitRight.collider && hitLeft.collider) ? -transform.up : dirToAvoid;
            }

            LookToDirection(dirToAvoid);
        }
    }

    /// <summary>
    /// Set the time the enemy spends trying to avoid obstacles before chasing the player again
    /// </summary>
    /// <param name="time">Time the enemy spends trying to avoid obstacles.</param>
    private IEnumerator TimeToAvoid(float time)
    {
        yield return new WaitForSeconds(time);
        dirToAvoid = Vector3.zero;
        isAvoiding = false;
        state = EnemyState.CHASE;

    }

    /// <summary>
    /// Attacks the player according to their attack type.
    /// Shooter: Shoot on player direction.
    /// Chaser: advances to collide with the player.
    /// </summary>
    private void Attack()
    {
        if (type == EnemyType.CHASER)
        {
            ChaserAttack();
        }
        if (type == EnemyType.SHOTTER && cannon != null)
        {

            cannon.TryGetComponent(out Cannon cannonInstance);
            
            if(cannonInstance != null) cannonInstance.Shoot(damage, gameObject);
        }
        StartCoroutine(DelayAttack());

    }

    private void ChaserAttack()
    {
        shipRigidbody.freezeRotation = true;
        Vector3 force = transform.up * speedMovement;
        shipRigidbody.AddForce(force * speedMultipleForChaserAttack, ForceMode2D.Impulse);

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

    /// <summary>
    /// Looks in the specified direction by rotating its front and using a raycast to detect objects;
    /// </summary>
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


    IEnumerator Death(bool countPoints = true)
    {
        isDead = true;
        canMove = false;

        // When chaser colliding with player he dead.
        // Does not cause effects to ships that have been defeated.
        shipHealth.TakeDamage(shipHealth.CurrentHealth);

        //Time to show death animation.
        yield return new WaitForSeconds(0.3f);

        // When chaser colliding with player  he dead but not count points
        if (countPoints) GameManager.Instance.AddPoint();

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
            shipRigidbody.freezeRotation = false;
            if(state == EnemyState.ATTACK) state = EnemyState.CHASE;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" &&
            state == EnemyState.ATTACK  &&
            type == EnemyType.CHASER)
        {
            collision.gameObject.TryGetComponent(out ShipHealth shipHealthInstance);
            if (shipHealthInstance != null) shipHealthInstance.TakeDamage(damage);
            StartCoroutine(Death(false));
        }

        if (collision.gameObject.tag == "Player" &&
            state != EnemyState.ATTACK &&
            type != EnemyType.CHASER)
        {
            StartCoroutine(StartDistanciateBehavior());
        }
    }

    IEnumerator StartDistanciateBehavior()
    {
        yield return new WaitForSeconds(0.5f);
        state = EnemyState.DISTANCIATE;
        yield return new WaitForSeconds(4f);
        state = EnemyState.CHASE;

    }
}
