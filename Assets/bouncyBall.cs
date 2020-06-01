using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyBall : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 velocity;
    GameManager gm;
    public float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = new Vector2(velocity.x, -velocity.y);
        if (collision.transform.tag == "Player"){
            gm.timeLeft -= damage;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "BackWall")
        {
            Destroy(gameObject);
        }
    }
}
