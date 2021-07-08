using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    bool isMoving;
    Vector2 prevPosition;
    LevelManager levelManager;

    public float runSpeed;
    public AudioClip pickup;

    int keysCollected;
    GameObject[] keys;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        body = GetComponent<Rigidbody2D>();
        isMoving = false;
        prevPosition = body.transform.position;

        keysCollected = 0;
        keys = GameObject.FindGameObjectsWithTag("MainKey");
        Debug.Log("number of keys: " + keys.Length);
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        //Debug.Log(body.velocity.magnitude);

        if (Vector2.Distance(prevPosition, body.transform.position) < .001f)//so the player can't pass the time by moving into a wall without actually changing positions
            isMoving = false;
        else
            isMoving = true;

        prevPosition = body.transform.position;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other + " entered");
        if (other.gameObject.CompareTag("MainKey"))
        {
            //AudioSource.PlayClipAtPoint(pickup, transform.position);
            other.gameObject.SetActive(false);
            keysCollected++;

            if (keysCollected >= keys.Length)
                levelManager.RemoveGate();
        }
    }
}
