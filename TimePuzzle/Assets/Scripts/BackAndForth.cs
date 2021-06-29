using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public float speed; //how many one way "laps" per second (with the current implementation)
    public MovePoint pointObjA, pointObjB;
    public bool moveWithPlayer;
    public float startPos; //0 for point A, 1 for point B,
    PlayerController player;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        time += startPos / speed; //sets the starting point of the object (doesn't always have to start at point A). range from 0 to 1 (point A to point B)
        float pingPongTime = Mathf.PingPong(time * speed, 1);//makes sure pingpong is based on the moving time OR stationary time
        transform.position = Vector2.Lerp(pointObjA.transform.position, pointObjB.transform.position, pingPongTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (player.GetIsMoving() == moveWithPlayer)
        {
            //float time = Mathf.PingPong(Time.time * speed, 1);//causes object to "jump" after it starts moving again when stationary before
            time += Time.deltaTime;
            float pingPongTime = Mathf.PingPong(time * speed, 1);//makes sure pingpong is based on the moving time OR stationary time
            transform.position = Vector2.Lerp(pointObjA.transform.position, pointObjB.transform.position, pingPongTime);
        }
    }
}