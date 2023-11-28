using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D bulletRigidbody;
    private int damage;
    private GameObject originObj;

    public GameObject OriginObj { set { originObj = value; } }
    public int Damage { set{ damage = value; } }

    private bool isHit = false;
    private bool canExplode = false;
    private Animator animator;



    private void OnEnable()
    {
        
        TryGetComponent(out animator);
        TryGetComponent(out bulletRigidbody);
        isHit = false;
        canExplode = false;

    }

    void Update()
    {
        if (!isHit)
        { 
            Vector3 force = transform.up * speed;
            if(bulletRigidbody != null) bulletRigidbody.AddForce(force, ForceMode2D.Force);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != originObj &&
            !isHit &&
            canExplode &&
            !collision.isTrigger)
        {  
            StartCoroutine(ExplodeBullet(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If not colling with origin object can explode
        if (collision.gameObject == originObj)
        {
            canExplode = true;
        }
    }


    private IEnumerator ExplodeBullet(Collider2D collision)
    {
        isHit = true;  
        if(bulletRigidbody != null) bulletRigidbody.velocity = Vector2.zero;
        if(animator != null) animator.SetTrigger("isHit");
        yield return new WaitForSeconds(.3f);
        collision.transform.TryGetComponent(out ShipHealth health);
        if (health != null) health.TakeDamage(damage);

        gameObject.SetActive(false);

    }
}
