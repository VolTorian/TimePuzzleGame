using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    Rigidbody2D rocket;

    float maxSpeed;
    float acceleration;
    bool homing;
    bool moveWithPlayer;

    Vector2 storedVelocity;
    bool justMoved;

    // Start is called before the first frame update
    void Start()
    {
        rocket = GetComponent<Rigidbody2D>();
        storedVelocity = new Vector2(0, 0);
        justMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, GetComponent<Rigidbody2D>().velocity);
    }

    void FixedUpdate()
    {
        if (PlayerController.GetIsMoving() == moveWithPlayer)
        {
            if (justMoved != PlayerController.GetIsMoving())
                rocket.velocity = storedVelocity; //will only go here once in any instance of continuous movement
            justMoved = PlayerController.GetIsMoving();

            if (rocket.velocity.sqrMagnitude < maxSpeed * maxSpeed) //does this perform better than simply magnitude < maxSpeed ?
                rocket.AddForce(transform.up * acceleration);
            storedVelocity = rocket.velocity;
        }
        else
        {
            rocket.velocity = new Vector2(0, 0);
            justMoved = PlayerController.GetIsMoving();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }

    public void setAttributes(float maxSpeed, float acceleration, bool homing, bool moveWithPlayer)
    {
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.homing = homing;
        this.moveWithPlayer = moveWithPlayer;
    }
}
