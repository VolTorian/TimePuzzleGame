using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    public bool up, down, left, right;
    public ParticleSystem collisionEffect;
    public bool moveWithPlayer;

    PlayerController player;
    LevelManager levelManager;
    int ignoreLayer;

    public LineRenderer laserUp;
    public LineRenderer laserDown;
    public LineRenderer laserLeft;
    public LineRenderer laserRight;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        levelManager = FindObjectOfType<LevelManager>();
        //laserUp = GetComponent<LineRenderer>();
        //laserDown = GetComponent<LineRenderer>();
        //laserLeft = GetComponent<LineRenderer>();
        //laserRight = GetComponent<LineRenderer>();
        //Ray2D ray = new Ray2D(transform.position, transform.forward);
        laserUp.startWidth = 0.25f;
        laserUp.startWidth = laserDown.startWidth = laserLeft.startWidth = laserRight.startWidth = 0.25f;
        laserUp.endWidth = laserDown.endWidth = laserLeft.endWidth = laserRight.endWidth = 0.25f;
        laserUp.startColor = laserDown.startColor = laserLeft.startColor = laserRight.startColor = Color.red;
        laserUp.endColor = laserDown.endColor = laserLeft.endColor = laserRight.endColor = Color.red;
        ignoreLayer = 1 << 11;//laser/raycast ignores collectable items like keys
        ignoreLayer = ~ignoreLayer;
    }

    private void Update()//FixedUpdate makes the lasers "lag" behind the node, hopefully being in Update doesn't screw up anything
    {
        if (player.GetIsMoving() == moveWithPlayer)
        {
            if (up)
                LaserDirection(laserUp, Vector2.up);
            if (down)
                LaserDirection(laserDown, Vector2.down);
            if (left)
                LaserDirection(laserLeft, Vector2.left);
            if (right)
                LaserDirection(laserRight, Vector2.right);
        }
        else
        {
            laserUp.enabled = false;
            laserDown.enabled = false;
            laserLeft.enabled = false;
            laserRight.enabled = false;
        }
    }

    void LaserDirection(LineRenderer laser, Vector2 direction)
    {
        laser.enabled = true;
        Ray2D ray = new Ray2D(transform.position, direction);
        RaycastHit2D hit;

        laser.SetPosition(0, transform.position);
        //Debug.Log(transform.position);
        hit = Physics2D.Raycast(ray.origin, direction, Mathf.Infinity, ignoreLayer);

        if (hit.collider)
        {
            laser.SetPosition(1, hit.point);
        }
        if (hit.collider.name == "Player")
        {
            levelManager.RespawnPlayer();
        }
        if (hit.collider.TryGetComponent(out RocketController mycomponent))
        {
            Destroy(mycomponent.gameObject);//adjust this and RocketController so the rockets explode when hitting the laser, not just disappear
        }
    }
}
