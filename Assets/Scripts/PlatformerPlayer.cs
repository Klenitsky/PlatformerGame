using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{

    public float speed = 250.0f;
    public float jumpForce = 12.0f;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        Vector2 movement = new Vector2(deltaX, body.velocity.y);
      
        body.velocity = movement;

        Vector3 max = hitbox.bounds.max;
        Vector3 min = hitbox.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        bool grounded = false;
        if(hit != null)
        {
            grounded = true;
        }

        if(grounded && deltaX == 0) 
        {
            body.gravityScale = 0;
        }
        else
        {
            body.gravityScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded) 
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
      

        MovingPlatform platform = null;
        if(hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>();
        }

        if (platform != null)
        {
              transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }

        anim.SetFloat("Speed", Mathf.Abs(deltaX));
        Vector3 pScale = Vector3.one;
        
        if (platform != null)
        {
            pScale= platform.transform.localScale;
        }
        
        if (deltaX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x , 1 / pScale.y,1);

        }
    }
}
