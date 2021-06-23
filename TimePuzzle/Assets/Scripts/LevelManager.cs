using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject spawnpoint;
    private PlayerController player;
    public GameObject deathParticle;
    public GameObject respawnParticle;
    public float respawnDelay;
    public string level;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //player.body
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        StartCoroutine("RespawnPlayerCo");
    }

    public IEnumerator RespawnPlayerCo()
    {
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        player.enabled = false;
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.transform.position = spawnpoint.transform.position;
        yield return new WaitForSeconds(respawnDelay);

        SceneManager.LoadScene(level);

        /**
        Debug.Log("Player respawned");
        player.GetComponent<Renderer>().enabled = true;
        player.enabled = true;
        
        //Instantiate(respawnParticle, spawnpoint.transform.position, spawnpoint.transform.rotation);
        **/
    }
}
