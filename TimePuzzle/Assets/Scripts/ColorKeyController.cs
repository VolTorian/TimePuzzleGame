using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorKeyController : MonoBehaviour
{
    public string barrierTag;
    GameObject[] barriers;

    // Start is called before the first frame update
    void Start()
    {
        barriers = GameObject.FindGameObjectsWithTag(barrierTag);//tags will be something like barrier[number] or barrier[color]
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other + " entered");
        if (other.gameObject.CompareTag("Player"))
        {
            //AudioSource.PlayClipAtPoint(pickup, transform.position);
            gameObject.SetActive(false);
            foreach (GameObject barrier in barriers)
            {
                barrier.SetActive(false);//disable every corresponding barrier
            }
        }
    }
}
