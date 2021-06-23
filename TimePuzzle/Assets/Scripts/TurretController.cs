using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    //public Transform player;
    private PlayerController player;
    Vector2 direction;
    float angle;
    Rigidbody2D turret;
    RaycastHit2D hit;
    float seePlayer;
    //public GameObject rocket;
    public RocketController projectile;
    int ignoreLayer;

    public float rocketMaxSpeed;
    public float rocketAcceleration;
    public float rocketAngleChangeSpeed;
    public bool moveWithPlayer;
    public int fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        turret = GetComponent<Rigidbody2D>();
        seePlayer = 0;
        ignoreLayer = 1 << 10;
        ignoreLayer = ~ignoreLayer; //ignore layer 10 (layer with projectiles)
    }

    // Update is called once per frame
    void Update()
    {
        //if (moveWithPlayer == PlayerController.GetIsMoving())
        if (moveWithPlayer == player.GetIsMoving())
        {
            direction = player.transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, ignoreLayer);

            if (hit.collider.name == "Player")
            {
                seePlayer += 1;
                turret.rotation = angle;
                //transform.rotation = angle;

                if (seePlayer == fireDelay) //in sight for 120 continuous frames (2 seconds)
                {
                    //Instantiate(rocket, transform.position, transform.rotation);
                    RocketController clone = Instantiate(projectile, transform.position, transform.rotation) as RocketController;
                    clone.setAttributes(rocketMaxSpeed, rocketAcceleration, moveWithPlayer, rocketAngleChangeSpeed);
                    //transform.position = player.transform.position;
                    seePlayer = 0;
                    //fire rocket
                }
            }
            else
            {
                seePlayer = 0;
            }
        }
    }
}
