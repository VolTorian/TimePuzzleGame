using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    Rigidbody2D rocket;
    private PlayerController player;

    float maxSpeed;
    float acceleration;
    bool moveWithPlayer;
    float angleChangeSpeed;//zero for non homing

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
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, GetComponent<Rigidbody2D>().velocity);
    }

    void FixedUpdate()
    {
        if (player.GetIsMoving() == moveWithPlayer)
        {
            if (justMoved != player.GetIsMoving())
            {
                rocket.velocity = storedVelocity; //will only go here once in any instance of continuous movement
                rocket.angularVelocity = storedAngularVelocity;
            }
            justMoved = player.GetIsMoving();

            if (player.enabled)
            {
                Vector2 direction = (Vector2)player.transform.position - rocket.position; //homing functionality
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                rocket.angularVelocity = -angleChangeSpeed * rotateAmount;
            }
            else
            {
                rocket.angularVelocity = 0;
            }
            //Debug.Log(rocket.angularVelocity);

            rocket.AddForce(transform.up * acceleration);
            //Debug.Log(transform.InverseTransformDirection(rocket.velocity).y);
            if (transform.InverseTransformDirection(rocket.velocity).y < 0)//if the rocket is going backwards
            {
                rocket.AddForce(transform.up * acceleration);
            }
            storedVelocity = rocket.velocity;//needs to be here so rockets don't get a "burst" in the direction they are facing if they start moving again after a freeze
        }
        else
        {
            rocket.angularVelocity = 0;
            rocket.velocity = new Vector2(0, 0);
            justMoved = player.GetIsMoving();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }

    public void setAttributes(float maxSpeed, float acceleration, bool moveWithPlayer, float angleChangeSpeed)
    {
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.moveWithPlayer = moveWithPlayer;
        this.angleChangeSpeed = angleChangeSpeed;
    }
}
