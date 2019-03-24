using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaController : MonoBehaviour
{
    [SerializeField] protected LayerMask collisionLayer;
    [SerializeField] protected float speed = 5;
    protected Vector2 dir;
    enum KoopaState
    {
        Walking = 1,
        InShell = 2,
        Zooming = 3,
    }

    public Sprite[] spritesArray = new Sprite[2];
    KoopaState state;
    [SerializeField]
    private Rigidbody2D koopaRb;

    private void Start()
    {
        dir = Vector2.left;
        state = KoopaState.Walking;
    }
    
    private void Update()
    {

        switch (state)
        {
            case KoopaState.Walking:
                Movement();
                break;

            case KoopaState.Zooming:
                Movement();
                break;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.tag == "Player")
        {
            switch (state)
            {
                case KoopaState.Walking:
                    /*
                     * // Detects if Mario hits Koopa from the top
                    Vector3 hit = collision.contacts[0].normal;
                    float angle = Vector3.Angle(hit, Vector3.up);

                    if (Mathf.Approximately(angle, 180))
                    {
                        state = KoopaState.InShell;
                        Debug.Log("Hit on top");
                    }
                    */

                    state = KoopaState.InShell; 
                    Debug.Log("Hit" + state);

                    break;

                case KoopaState.InShell:
                    state = KoopaState.Zooming;
                    dir *= -1;
                    speed = 10;
                    break;
            }
        }
    }

    private void Movement()
    {
        if (IsColliding())
        {
            dir *= -1;
        }
        transform.position += new Vector3(dir.x * speed * Time.deltaTime, 0, 0);
    }

    private bool IsColliding()
    {
        Vector3 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, transform.lossyScale.x / 2, collisionLayer);
        //Debug.DrawRay(origin, dir * transform.lossyScale.x/2, Color.yellow);
        return hit.collider != null;
    }

}
