using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D bulletRigidbody;
    private int damage;
    private GameObject originObj;
    public GameObject OriginObj { get { return originObj; } set { originObj = value; } }
    public int Damage { get { return damage; } set{ damage = value; } }

    private bool isHit = false;
    private bool canExplode = false;
    private Animator animator;



    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        isHit = false;
        canExplode = false;
        bulletRigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (!isHit)
        { 
            Vector3 force = transform.up * speed;
            if(bulletRigidbody)bulletRigidbody.AddForce(force, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != originObj &&
            !isHit &&
            canExplode &&
            !collision.isTrigger)
        {  
            StartCoroutine(ExplodeBullet(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == originObj)
        {
            canExplode = true;
        }
    }


    private IEnumerator ExplodeBullet(Collider2D collision)
    {
        isHit = true;  
        bulletRigidbody.velocity = Vector2.zero;
        animator.SetTrigger("isHit");
        yield return new WaitForSeconds(.3f);
        ShipHealth heath = collision.gameObject.GetComponent<ShipHealth>();
        if (heath) heath.TakeDamage(damage);
        gameObject.SetActive(false);

    }
}
