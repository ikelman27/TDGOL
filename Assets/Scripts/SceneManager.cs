using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour {

    public GameObject enemyManager;
    public Button newRound;
    int round = 0;
    public GameObject turret;
    List<GameObject> turrets;
	// Use this for initialization
	void Start () {
        turrets = new List<GameObject>();
        newRound.gameObject.SetActive(false);
        newRound.onClick.AddListener(startNewRound);
	}
	
    void startNewRound()
    {
        round++;
        enemyManager.GetComponent<EnemyPath>().startRound(round);
        newRound.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {

        if (!newRound.IsActive() && !enemyManager.GetComponent<EnemyPath>().enemysRemaining())
        {
            newRound.gameObject.SetActive(true);

        }
        if (newRound.IsActive())
        {
            gameObject.GetComponent<Grid>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Grid>().enabled = false;
        }
	}

    public void createTurret(Vector3Int pos)
    {
        Vector3 position = new Vector3(.5f, .5f, 0);
        for(int i = 0; i <turrets.Count; i++)
        {
            if(Vector3.Magnitude( turrets[i].transform.position - (pos+position)) < 1)
            {
                return;
            }
        }
        GameObject tempObj = Instantiate(turret, pos+position, Quaternion.identity);
        turrets.Add(tempObj);
    }
}
