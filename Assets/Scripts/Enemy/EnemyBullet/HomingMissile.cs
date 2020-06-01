using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : BulletManager
{
    private Transform target;
    public float rotationSpeed;
    Rigidbody2D rb;




    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();

        float rotationAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotationAmount * rotationSpeed;

        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Reduce the overall time
            Debug.Log("Player took a hit");
            //manager.timeLeft -= damage;
            Destroy(this.gameObject);
        }
        if (collision.transform.tag == "Tilemap")
        {
            Destroy(this.gameObject);
        }
    }


}
