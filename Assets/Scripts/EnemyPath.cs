using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;



public struct panel
{
    
    public float cost;
    public float baseCost;
    public bool visited;
    public bool isValid;
    public Vector2Int position;
    public Vector2Int parentDir;
    public float distFromGoal;
    public float totalCost;

    public panel(int newCost, bool isVisited, Vector2Int newPosition)
    {
        baseCost = newCost;
        cost = 0;
        visited = isVisited;
        position = newPosition;
        parentDir = new Vector2Int(0, 0);
        isValid = true;
        distFromGoal = 0;
        totalCost = 0;
    }
    public panel(Vector2Int newPosition, float newCost)
    {
        baseCost = newCost;
        position = newPosition;
        cost = 0;
        visited = false;
        parentDir = new Vector2Int(0, 0);
        isValid = true;
        distFromGoal = 0;
        totalCost = 0;
    }

    public void addParrent(panel par)
    {
        parentDir = par.position - position;

    }
    public Vector3Int getWorldPosition()
    {
        return new Vector3Int(position.x - 15, position.y - 15, 0);
    }

    public void calcTotalCost()
    {
        totalCost = distFromGoal + cost;

    }

}

public class EnemyPath : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public GameObject startPoint;
    public Tilemap enemyMap;
    public Tilemap wallMap;
    public Tile searchedTile;
    public GameObject targetPoint;
    panel[,] gameBoard;
    public Vector3Int startPos;
    List<Vector3Int> pathPoints;
   public int enemyNum;
    List<GameObject> Enemys;
    // Use this for initialization
    void Start()
    {
        Enemys = new List<GameObject>();
        pathPoints = new List<Vector3Int>();
        gameBoard = new panel[30, 30];
        startPos = new Vector3Int((int)startPoint.transform.position.x, (int)startPoint.transform.position.y, 0);
        targetPoint.transform.position = Vector3Int.FloorToInt(targetPoint.transform.position);
        Vector3Int point = Vector3Int.FloorToInt(targetPoint.transform.position);

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                float cost = Random.Range(0, 0);
                gameBoard[i, j] = new panel(new Vector2Int(i, j), cost);

                if (wallMap.GetTile(new Vector3Int(i - 15, j - 15, 0)) != null)
                {
                    gameBoard[i, j].isValid = false;
                }
                else
                {
                    gameBoard[i, j].distFromGoal = distFromGoal(new Vector3Int(i, j, 0), point + new Vector3Int(15, 15, 0));
                }
            }
        }

        startRound(0);
        

    }
    private IEnumerator timedSpawn()
    {
        while (enemyNum > 0)
        {
            
            yield return new WaitForSeconds(0.5f); // wait half a second
            GameObject tempObj = Instantiate(enemyPrefabs[0], startPoint.transform.position, Quaternion.identity);

            tempObj.GetComponent<EnemyScript>().setPath(pathPoints);
            tempObj.name = "enemy " + enemyNum;
            Enemys.Add(tempObj);
            
            enemyNum--;
            
               
        }
    }



    public void startRound(int roundNum)
    {
        Enemys = new List<GameObject>();
        StopCoroutine(timedSpawn());

        resetTileMap();
        getDirectionsToPoint(Vector2Int.FloorToInt((Vector2)(targetPoint.transform.position)));

        enemyNum = 3 + roundNum * 3;
        StartCoroutine(timedSpawn());
      
    }

    private void resetTileMap()
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                float cost = Random.Range(0, 0);
                gameBoard[i, j].visited = false;
                gameBoard[i, j].cost = 0;
                //setTilemapSearched(new Vector2Int(i, j), 0);
                if (wallMap.GetTile(new Vector3Int(i - 15, j - 15, 0)) != null)
                {
                    gameBoard[i, j].isValid = false;
                }
                else
                {
                    gameBoard[i, j].isValid = true;
                }
            }
        }

    }

    private void addPath()
    {
        //throw new NotImplementedException();
    }

    public bool enemysRemaining()
    {
        if(Enemys.Count == 0)
        {
            return true;
        }
        for(int i = 0; i < Enemys.Count; i++)
        {
            if(Enemys[i] != null)
            {
                return true;
            }
        }
        return false;
    }



    // Update is called once per frame
    void Update()
    {
        
               

    }

    void setTilemapSearched(Vector2Int position, float cost)
    {

        searchedTile.color = Color.LerpUnclamped(Color.blue, Color.red, cost * .03f);
        //enemyMap.SetTile(new Vector3Int(position.x - 15, position.y - 15, 0), null);
        //enemyMap.SetTile(new Vector3Int(position.x - 15, position.y - 15, 0), searchedTile);
    }


    void getDirectionsToPoint(Vector2Int point)
    {


        List<panel> priorityQueue = new List<panel>();
        Queue<panel> searchQueue = new Queue<panel>();
        

        Vector2Int panelPos = new Vector2Int(startPos.x + 15, startPos.y + 15);
        
        point.x += 15;
        point.y += 15;
        
        panel finalPoint = gameBoard[point.x, point.y];

        gameBoard[panelPos.x, panelPos.y].visited = true;

        searchQueue.Enqueue(gameBoard[panelPos.x, panelPos.y]);

        int count = 1;
        while (searchQueue.Count > 0)
        {
            panel tempPanel = searchQueue.Dequeue();

            setTilemapSearched(tempPanel.position, tempPanel.cost);
            if (tempPanel.position.x > 0)
            {

                panel westPanel = gameBoard[tempPanel.position.x - 1, tempPanel.position.y];


                if (westPanel.isValid)
                {
                    if (westPanel.visited == false || westPanel.cost > tempPanel.cost + 1 + westPanel.baseCost)
                    {
                        westPanel.visited = true;
                        westPanel.cost = westPanel.baseCost + tempPanel.cost + 1;
                        westPanel.addParrent(tempPanel);
                        gameBoard[tempPanel.position.x - 1, tempPanel.position.y] = westPanel;
                        setTilemapSearched(westPanel.position, westPanel.cost);
                        westPanel.calcTotalCost();

                        searchQueue.Enqueue(westPanel);
                      
                        if (westPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }
                }
            }


            if (tempPanel.position.x < 29)
            {
                panel eastPanel = gameBoard[tempPanel.position.x + 1, tempPanel.position.y];


                if (eastPanel.isValid)
                {
                    if (eastPanel.visited == false || eastPanel.cost > tempPanel.cost + 1 + eastPanel.baseCost)
                    {
                        eastPanel.visited = true;
                        eastPanel.cost = eastPanel.baseCost + tempPanel.cost + 1;
                        eastPanel.addParrent(tempPanel);
                        gameBoard[tempPanel.position.x + 1, tempPanel.position.y] = eastPanel;
                        setTilemapSearched(eastPanel.position, eastPanel.cost);
                        eastPanel.calcTotalCost();
                        searchQueue.Enqueue(eastPanel);
                        //priorityQueue.Add(eastPanel, eastPanel);
                        if (eastPanel.position == finalPoint.position)
                        {
                            break;
                        }

                    }
                }

            }
            if (tempPanel.position.y > 0)
            {
                panel southPanel = gameBoard[tempPanel.position.x, tempPanel.position.y - 1];

                if (southPanel.isValid)
                {
                    if (southPanel.visited == false || southPanel.cost > tempPanel.cost + 1 + southPanel.baseCost)
                    {
                        southPanel.visited = true;
                        southPanel.cost = southPanel.baseCost + tempPanel.cost + 1;
                        southPanel.addParrent(tempPanel);
                        gameBoard[tempPanel.position.x, tempPanel.position.y - 1] = southPanel;
                        setTilemapSearched(southPanel.position, southPanel.cost);
                        southPanel.calcTotalCost();
                        searchQueue.Enqueue(southPanel);
                        if (southPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }
                }
            }
            if (tempPanel.position.y < 29)
            {
                panel northPanel = gameBoard[tempPanel.position.x, tempPanel.position.y + 1];


                if (northPanel.isValid)
                {
                    if (northPanel.visited == false || northPanel.cost > tempPanel.cost + 1 + northPanel.baseCost)
                    {
                        northPanel.visited = true;
                        northPanel.cost = northPanel.baseCost + tempPanel.cost + 1;
                        northPanel.addParrent(tempPanel);
                        gameBoard[tempPanel.position.x, tempPanel.position.y + 1] = northPanel;
                        setTilemapSearched(northPanel.position, northPanel.cost);
                        northPanel.calcTotalCost();
                        searchQueue.Enqueue(northPanel);
                        if (northPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }
                }
            }
            /*
            if (canMoveDiagonal)
            {
                if (tempPanel.position.x > 0 && tempPanel.position.y > 0)
                {
                    panel southWest = gameBoard[tempPanel.position.x - 1, tempPanel.position.y - 1];


                    if (southWest.isValid)
                    {
                        if (southWest.visited == false || southWest.cost > tempPanel.cost + 1.414f + southWest.baseCost)
                        {
                            southWest.visited = true;
                            southWest.cost = southWest.baseCost + tempPanel.cost + 1.414f;
                            southWest.addParrent(tempPanel);
                            gameBoard[tempPanel.position.x - 1, tempPanel.position.y - 1] = southWest;

                            setTilemapSearched(southWest.position, southWest.cost);
                            southWest.calcTotalCost();
                            searchQueue.Enqueue(southWest);
                            if (southWest.position == finalPoint.position)
                            {
                                break;
                            }
                        }
                    }
                }
                if (tempPanel.position.x > 0 && tempPanel.position.y < 29)
                {
                    panel northWest = gameBoard[tempPanel.position.x - 1, tempPanel.position.y + 1];

                    if (northWest.isValid)
                    {
                        if (northWest.visited == false || northWest.cost > tempPanel.cost + 1.414f + northWest.baseCost)
                        {
                            northWest.visited = true;
                            northWest.cost = northWest.baseCost + tempPanel.cost + 1.414f;
                            northWest.addParrent(tempPanel);
                            gameBoard[tempPanel.position.x - 1, tempPanel.position.y + 1] = northWest;
                            setTilemapSearched(northWest.position, northWest.cost);
                            northWest.calcTotalCost();
                            searchQueue.Enqueue(northWest);
                            if (northWest.position == finalPoint.position)
                            {
                                break;
                            }
                        }
                    }
                }

                if (tempPanel.position.x < 29 && tempPanel.position.y > 0)
                {
                    panel southEast = gameBoard[tempPanel.position.x + 1, tempPanel.position.y - 1];

                    if (southEast.isValid)
                    {
                        if (southEast.visited == false || southEast.cost > tempPanel.cost + 1.414f + southEast.baseCost)
                        {
                            southEast.visited = true;
                            southEast.cost = southEast.baseCost + tempPanel.cost + 1.414f;
                            southEast.addParrent(tempPanel);
                            gameBoard[tempPanel.position.x + 1, tempPanel.position.y - 1] = southEast;
                            setTilemapSearched(southEast.position, southEast.cost);
                            southEast.calcTotalCost();
                            searchQueue.Enqueue(southEast);
                            if (southEast.position == finalPoint.position)
                            {
                                break;
                            }
                        }
                    }
                }

                if (tempPanel.position.x < 29 && tempPanel.position.y < 29)
                {
                    panel northEast = gameBoard[tempPanel.position.x + 1, tempPanel.position.y + 1];

                    if (northEast.isValid)
                    {
                        if (northEast.visited == false || northEast.cost > tempPanel.cost + 1.14f + northEast.baseCost)
                        {
                            northEast.visited = true;
                            northEast.cost = northEast.baseCost + tempPanel.cost + 1.14f;
                            northEast.addParrent(tempPanel);
                            gameBoard[tempPanel.position.x + 1, tempPanel.position.y + 1] = northEast;
                            setTilemapSearched(northEast.position, northEast.cost);
                            northEast.calcTotalCost();
                            searchQueue.Enqueue(northEast);
                            if (northEast.position == finalPoint.position)
                            {
                                break;
                            }
                        }
                    }



                }


            }
            */


            priorityQueue = searchQueue.ToList<panel>();

            priorityQueue.Sort((x, y) => x.totalCost.CompareTo(y.totalCost));
            count += 8;

            searchQueue = new Queue<panel>(priorityQueue);




        }



        
        searchQueue.Clear();

        panel currentPoint = gameBoard[point.x, point.y];
        int totalSteps = 0;

        pathPoints.Insert(0, currentPoint.getWorldPosition());
        while (currentPoint.position != panelPos)
        {
            panel parrent = gameBoard[currentPoint.position.x + currentPoint.parentDir.x, currentPoint.position.y + currentPoint.parentDir.y];

            Debug.DrawLine(currentPoint.getWorldPosition(), parrent.getWorldPosition(), Color.black, 100000);

            currentPoint = parrent;
            pathPoints.Insert(0, parrent.getWorldPosition());
            totalSteps++;
            if (totalSteps > 150) { break; }





        }
        
    }

    float distFromGoal(Vector3Int point, Vector3Int goal)
    {
        float distX = Mathf.Abs(goal.x - point.x);
        float distY = Mathf.Abs(goal.y - point.y);
        if (false)
        {
            if (distX > distY)
            {
                return distY * 1.41f + distX - distY;
            }
            return distX * 1.41f + distY - distX;

        }
        else return distX + distY;
    }
}
