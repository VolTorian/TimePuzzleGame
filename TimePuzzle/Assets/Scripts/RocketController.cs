using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    Rigidbody2D rocket;
    private PlayerController player;

    float maxSpeed;
    float acceleration;
    bool homing;
    bool moveWithPlayer;
    float angleChangeSpeed;

    Vector2 storedVelocity;
    float storedAngularVelocity;
    bool justMoved;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rocket = GetComponent<Rigidbody2D>();
        storedVelocity = new Vector2(0, 0);
        storedAngularVelocity = 0;
        justMoved = false;
        angleChangeSpeed = 160;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, GetComponent<Rigidbody2D>().velocity);
    }

    void FixedUpdate()
    {
        //Debug.Log("Player moving: " + PlayerController.GetIsMoving());
        //if (PlayerController.GetIsMoving() == moveWithPlayer)
        if (player.GetIsMoving() == moveWithPlayer)
        {
            //if (justMoved != PlayerController.GetIsMoving())
            if (justMoved != player.GetIsMoving())
            {
                rocket.velocity = storedVelocity; //will only go here once in any instance of continuous movement
                rocket.angularVelocity = storedAngularVelocity;
            }
            //justMoved = PlayerController.GetIsMoving();
            justMoved = player.GetIsMoving();

            Vector2 direction = (Vector2)player.transform.position - rocket.position; //homing functionality
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rocket.angularVelocity = -angleChangeSpeed * rotateAmount;

            if (rocket.velocity.sqrMagnitude < maxSpeed * maxSpeed) //does this perform better than simply magnitude < maxSpeed ?
            {
                rocket.AddForce(transform.up * acceleration);
                storedVelocity = rocket.velocity;
            }
            //storedVelocity = rocket.velocity;
        }
        else
        {
            rocket.angularVelocity = 0;
            rocket.velocity = new Vector2(0, 0);
            //justMoved = PlayerController.GetIsMoving();
            justMoved = player.GetIsMoving();
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
