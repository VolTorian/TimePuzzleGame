using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject spawnpoint;
    PlayerController player;
    public GameObject deathParticle;
    public GameObject respawnParticle;
    public float respawnDelay;
    public string level;

    public TextMeshProUGUI movingTimer;
    public TextMeshProUGUI stillTimer;
    public TextMeshProUGUI overallTimer;
    float movingTotalTime;
    float stillTotalTime;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        movingTotalTime = 0;
        stillTotalTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetIsMoving())
        {
            movingTotalTime += Time.deltaTime;
            movingTimer.text = "Moving:\n    " + FormatTime(movingTotalTime);
        }
        else
        {
            stillTotalTime += Time.deltaTime;
            stillTimer.text = "Stationary:\n    " + FormatTime(stillTotalTime);
        }
        overallTimer.text = "Total:\n    " + FormatTime(stillTotalTime + movingTotalTime);
    }

    public void RespawnPlayer()
    {
        StartCoroutine("RespawnPlayerCo");
    }

    string FormatTime(float totalTime)
    {
        int minutes = ((int)totalTime) / 60;
        int seconds = ((int)totalTime) % 60;
        float centiseconds = totalTime * 100;
        centiseconds = centiseconds % 100;
        //int centiseconds = Mathf.RoundToInt((totalTime * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, (int)centiseconds);
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
