using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;




public class EnemyScript : MonoBehaviour
{

    public GameObject sceneManager;

    public float speed;
    public int health;
    public bool canMoveDiagonal;
    public GameObject firstPoint;
    public Tile searchedTile;
    public GameObject targetPoint;
    public Tilemap terrainTileMap;
    public float percentToGoal;
    float boostSpeed = 1;

    //Modifyers
    public bool diagonalMove;
    public bool hop;
    public bool fly;
   
    Vector3Int currentPos;
    public Vector3 dir;
    Vector3 pos;
   // Vector3 halfPos;
    Vector3Int nextPos;
    public List<Vector3Int> pathPoints;
    int position;
    //panel[,] gameBoard;

    int maxHealth;

    // Use this for initialization
    void Start()
    {
        boostSpeed = 1;
        speed = 2;

        //gameBoard = new panel[30, 30];

        health = 5;
        maxHealth = health;
        position = 0;
      
        nextPos = currentPos;


    }

    public void takeDammage()
    {
        health--;
        transform.GetChild(2).transform.localScale = new Vector3(transform.GetChild(1).transform.localScale.x * health / maxHealth, transform.GetChild(1).transform.localScale.y , transform.GetChild(1).transform.localScale.z);
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }

    public void setPath(List<Vector3Int> points)
    {


        currentPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
       
        transform.position = currentPos;

        pathPoints = new List<Vector3Int>();
        pathPoints = points;

        nextPos = currentPos; 
       
        chooseNewTarget();
    }

    void chooseNewTarget()
    {
        if (position < pathPoints.Count - 1) { position++; }
        currentPos = nextPos;
        transform.position = currentPos;

        nextPos = pathPoints[position];

        TileBase terrainTile = terrainTileMap.GetTile(currentPos);
        if(terrainTile != null && !fly)
        {
            if(terrainTile.name == "TerrainFast")
            {
                boostSpeed = 1.5f;
            }
            else
            {
                
                boostSpeed = .5f;
            }
        }
        else
        {
            boostSpeed = 1;
        }
        dir = nextPos - currentPos;
        dir = Vector3.Normalize(dir);
        //transform.rotation = Quaternion.Euler(dir);
        transform.GetChild(0).transform.rotation = Quaternion.identity;
        
        if(dir.x == 0)
        {
            transform.GetChild(0).Rotate(new Vector3(0, 0, Mathf.Asin(dir.x) * Mathf.Rad2Deg));
        }
        else
        {
            transform.GetChild(0).Rotate(new Vector3(0, 0, Mathf.Acos(dir.y) * Mathf.Rad2Deg));
        }
      

    }

    /*
    public void addPath(GameObject Point1, List<GameObject> points, GameObject fPoint)
    {
        firstPoint = Point1;
        pathPoints = points;
        finalPoint = fPoint;

        isEnabled = true;

    }*/

    // Update is called once per frame
    void Update()
    {


      

        //Debug.DrawLine(currentPos, pathPoints[0], Color.red);
        for (int i = 1; i < pathPoints.Count; i++)
        {
            Debug.DrawLine(pathPoints[i - 1], pathPoints[i], Color.red);
        }


        if (checkIntersection(nextPos))
        {
            chooseNewTarget();
        }
        else if (Vector3.Magnitude(transform.position - currentPos) > Vector3.Magnitude(nextPos - currentPos))
        {
            chooseNewTarget();
        }
        else
        {
           
            
            // Debug.DrawLine(currentPos, nextPos, Color.blue);

            pos = transform.position;
            pos += dir * Time.deltaTime * speed*boostSpeed;
            transform.position = pos;



        }
       // Debug.Log(percentToGoal);
        getPercentToGoal();
        if(percentToGoal > .99f)
        {
            sceneManager.GetComponent<SceneManager>().takeDammage();
            Destroy(gameObject);
        }


    }


   public float getPercentToGoal()
    {
        float index = pathPoints.IndexOf(currentPos);
        
        float total = pathPoints.Count-1;
        
        percentToGoal = index * (1.0f / total)+(Vector3.Magnitude(transform.position-currentPos)* (1.0f / total));
        
        return percentToGoal;
    }



    bool checkIntersection(Vector3 targetPos)
    {
        if (Mathf.Abs(transform.position.x - targetPos.x) < .05f && Mathf.Abs(transform.position.y - targetPos.y) < .05f)
            return true;
        return false;

    }
}



