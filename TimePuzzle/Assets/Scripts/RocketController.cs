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
        //angleChangeSpeed = 160;
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

            //if (rocket.velocity.sqrMagnitude < maxSpeed * maxSpeed) //does this perform better than simply magnitude < maxSpeed ?
            //if ((transform.rotation * rocket.velocity * -1).y < maxSpeed && acceleration + (transform.rotation * rocket.velocity * -1).y < maxSpeed) //velocity in the rocket's up direction - figure out why this works
            if (transform.InverseTransformDirection(rocket.velocity).y < maxSpeed && acceleration + transform.InverseTransformDirection(rocket.velocity).y < maxSpeed)
            {
                rocket.AddForce(transform.up * acceleration);
                //storedVelocity = rocket.velocity;
                //Debug.Log("if true");
            }
            else
            {
                //rocket.velocity = Vector2.ClampMagnitude(rocket.velocity, maxSpeed);
                //rocket.velocity.Set((transform.rotation * rocket.velocity).x, maxSpeed);
                //rocket.AddForce(transform.up * (maxSpeed - (transform.rotation * rocket.velocity * -1).y));
                //rocket.AddForce(transform.up * (maxSpeed - transform.InverseTransformDirection(rocket.velocity).y));
                //Debug.Log(maxSpeed - (transform.rotation * rocket.velocity).y);
                Vector2 localVelocity = transform.InverseTransformDirection(rocket.velocity);
                localVelocity.y = maxSpeed;
                rocket.velocity = transform.TransformDirection(localVelocity);
                //Debug.Log("if false");

            }
            //Debug.Log(rocket.velocity.magnitude);
            storedVelocity = rocket.velocity;//needs to be here so rockets don't get a "burst" in the direction they are facing if they start moving again after a freeze
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

    public void setAttributes(float maxSpeed, float acceleration, bool homing, bool moveWithPlayer, float angleChangeSpeed)
    {
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.homing = homing;
        this.moveWithPlayer = moveWithPlayer;
        this.angleChangeSpeed = angleChangeSpeed;
    }
}
