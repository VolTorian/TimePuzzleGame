using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    Rigidbody2D rocket;
    private PlayerController player;
    LevelManager levelManager;
    int ignoreLayer;

    float maxSpeed;
    float acceleration;
    bool moveWithPlayer;
    float angleChangeSpeed;//zero for non homing
    float blastRadius;

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
        ignoreLayer = 1 << 10;
        ignoreLayer = ~ignoreLayer; //ignore layer 10 (layer with projectiles)
        levelManager = FindObjectOfType<LevelManager>();
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
        Vector2 explosionPos = (Vector2)transform.position - 0.5f * rocket.velocity.normalized;//so it doesn't start the raycast in the wall?
        //Debug.Log("position of collision: " + (Vector2)transform.position);
        //Debug.Log(rocket.velocity.normalized);
        //Debug.Log("raycasting from " + explosionPos + " to " + (Vector2)player.transform.position + ". Distance: " + Vector2.Distance(player.transform.position, explosionPos));

        RaycastHit2D raycastCheck;
        Vector2 direction;
        float angle;
        Destroy(gameObject);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, blastRadius);
        //coroutine for explosion animation?
        foreach (Collider2D hit in colliders)
        {
            direction = player.transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            raycastCheck = Physics2D.Raycast(transform.position, direction, blastRadius, ignoreLayer);
            if (raycastCheck && raycastCheck.collider.name == "Player")
            {
                levelManager.RespawnPlayer();
            }
        }

    }

    public void setAttributes(float maxSpeed, float acceleration, bool moveWithPlayer, float angleChangeSpeed, float blastRadius)
    {
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.moveWithPlayer = moveWithPlayer;
        this.angleChangeSpeed = angleChangeSpeed;
        this.blastRadius = blastRadius;
    }
}
