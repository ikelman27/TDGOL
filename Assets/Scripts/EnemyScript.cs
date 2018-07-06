using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;




public class EnemyScript : MonoBehaviour
{



    public float speed;
    public float health;
    public bool canMoveDiagonal;
    public GameObject firstPoint;
    public Tile searchedTile;
    public GameObject targetPoint;

    public float percentToGoal;

   // GameObject target;
    //GameObject prevPoint;
    public Tilemap wallmap;
    public Tilemap enemyMap;
    Vector3Int currentPos;
    public Vector3 dir;
    Vector3 pos;
   // Vector3 halfPos;
    Vector3Int nextPos;
    public List<Vector3Int> pathPoints;
    int position;
    panel[,] gameBoard;



    // Use this for initialization
    void Start()
    {
        speed = 2;

        gameBoard = new panel[30, 30];

        health = 5;

        position = 0;
        



        //getDirectionsToPoint(Vector2Int.FloorToInt((Vector2)(targetPoint.transform.position)));

        nextPos = currentPos;


      




    }

    public void takeDammage()
    {
        health--;
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

        currentPos = nextPos;
        transform.position = currentPos;

        nextPos = pathPoints[position];

        if (position < pathPoints.Count - 1) { position++; }

        dir = nextPos - currentPos;
        dir = Vector3.Normalize(dir);
        //transform.rotation = Quaternion.Euler(dir);


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


        getPercentToGoal();
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
            pos += dir * Time.deltaTime * speed;
            transform.position = pos;



        }

    

    }


   public float getPercentToGoal()
    {
        float index = pathPoints.IndexOf(currentPos);
        float total = pathPoints.Count;
       
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



