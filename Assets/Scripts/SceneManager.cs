using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    public GameObject enemyManager;
    public Button newRound;
    public Text moneyText;
    public Text healthText;
    enum states
    {
        gameStart, inRound, roundOver, GameOver, Win
    }

    public int health = 20;
    int maxHealth;
    public int wallPrice = 10;
    public int money = 100;
    int round = 0;
    public GameObject turret;
    List<GameObject> turrets;
    
    int currentState;

	// Use this for initialization
	void Start () {
        health = 20;
        maxHealth = health;
        turrets = new List<GameObject>();
        newRound.gameObject.SetActive(true);
        newRound.onClick.AddListener(startNewRound);
        currentState =(int) states.roundOver;
        healthText.text = health.ToString();
        moneyText.text = money.ToString();
    }

    //Increases round number and begins new round
    //Add money here
    void startNewRound()
    {

        int[] enemySpawns =  { 5, 1 };

        currentState = (int)states.inRound;
        round++;
        enemyManager.GetComponent<EnemyPath>().startRound(round, enemySpawns);
        newRound.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        

        if (currentState != (int)states.roundOver)
        {
            //if their are enemys remaining the round is not over
            if (!newRound.IsActive() && !enemyManager.GetComponent<EnemyPath>().enemysRemaining() && currentState != (int)states.GameOver)
            {
                newRound.gameObject.SetActive(true);
                currentState = (int)states.roundOver;
                money += 40;
                moneyText.text = money.ToString();
            }
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

    //checks if the turret is on a valid spot and then creates the turret

    public void createTurret(Vector3Int pos)
    {
        if(money - turret.GetComponent<TurretScrypt>().price < 0)
        {
            return;
        }
        
        Vector3 position = new Vector3(.5f, .5f, 0);
        for(int i = 0; i <turrets.Count; i++)
        {
            if(Vector3.Magnitude( turrets[i].transform.position - (pos+position)) < 1)
            {
                return;
            }
           
        }
        GameObject tempObj = Instantiate(turret, pos+position, Quaternion.identity);
        money -= (int)turret.GetComponent<TurretScrypt>().price;
        moneyText.text = money.ToString();
        turrets.Add(tempObj);
    }



    public void removeTurret(Vector3Int pos)
    {
        Debug.Log("test");
        Vector3 position = new Vector3(.5f, .5f, 0);
        for (int i = 0; i < turrets.Count; i++)
        {
            if (Vector3.Magnitude(turrets[i].transform.position - (pos + position)) < 1)
            {
                money += (int)turret.GetComponent<TurretScrypt>().price *2/3;
                moneyText.text = money.ToString();
                Destroy(turrets[i]);
                turrets.RemoveAt(i);
                
            }

        }

    }

    //checks if there is enough money to place a wall
   
    public bool canPlaceWall()
    {

        if (!enemyManager.GetComponent<EnemyPath>().isValidPath())
        {
            Debug.Log("test");
            return false;
           
        }

        if(money-wallPrice < 0)
        {
            return false;
        }
        money -= wallPrice;
        moneyText.text = money.ToString();
        return true;
    }

    public void removeWall()
    {
        money += wallPrice * 2 / 3;
        moneyText.text = money.ToString();
    }

    public void takeDammage()
    {
        health--;
        healthText.text = health.ToString();
        if (health < 0)
        {
            currentState = (int)states.GameOver;
        }
    }

}
