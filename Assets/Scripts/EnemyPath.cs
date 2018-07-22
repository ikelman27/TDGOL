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
    public Tilemap terrainMap;
    public Tile searchedTile;
    public GameObject targetPoint;
    panel[,] gameBoard;
    public Vector3Int startPos;
    List<List<Vector3Int>> pathPoints;
    public int enemyNum;
    List<GameObject> Enemys;
    public int enemyStart;
    public int EnemyRate;

    public GameObject sceneManager;

    // Use this for initialization
    void Start()
    {
        Enemys = new List<GameObject>();
        pathPoints = new List<List<Vector3Int>>();
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
                    if (terrainMap.GetTile(new Vector3Int(i - 15, j - 15, 0)) != null)
                    {
                        if (terrainMap.GetTile(new Vector3Int(i - 15, j - 15, 0)).name == "TerrainSlow")
                        {
                            gameBoard[i, j].baseCost = .5f;
                        }
                        else if (terrainMap.GetTile(new Vector3Int(i - 15, j - 15, 0)).name == "TerrainFast")
                        {
                            gameBoard[i, j].baseCost = -.5f;
                        }
                    }
                }
            }
        }

        //startRound(0);


    }
    private IEnumerator timedSpawn(int[] enemySpawn)
    {
        int i = 0;

        while (i < enemySpawn.Length)
        {

            yield return new WaitForSeconds(0.5f); // wait half a second

            GameObject tempObj = Instantiate(enemyPrefabs[i], startPoint.transform.position, Quaternion.identity);
            tempObj.GetComponent<EnemyScript>().sceneManager = sceneManager;
            tempObj.GetComponent<EnemyScript>().terrainTileMap = terrainMap;
            tempObj.GetComponent<EnemyScript>().setPath(pathPoints[i]);
            tempObj.name = "enemy " + i + "  " + enemyNum;
            Enemys.Add(tempObj);
            enemyNum--;
            enemySpawn[i]--;
            if (enemySpawn[i] == 0)
            {
                i++;
            }


        }
    }



    public void startRound(int roundNum, int[] enemySpawns)
    {
        pathPoints = new List<List<Vector3Int>>();
        //Debug.Log(pathPoints.Count);
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyMap.ClearAllTiles();
            resetTileMap();
            pathPoints.Add(new List<Vector3Int>());
            // Debug.Log(pathPoints.Count);
            getDirectionsToPoint(Vector2Int.FloorToInt((Vector2)(targetPoint.transform.position)), i);

        }

        Enemys = new List<GameObject>();
        StopCoroutine(timedSpawn(enemySpawns));



        enemyNum = enemyStart + roundNum * EnemyRate;
        StartCoroutine(timedSpawn(enemySpawns));

    }

    private void resetTileMap()
    {

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                // float cost = 0;// Random.Range(0, 0);
                gameBoard[i, j].visited = false;
                gameBoard[i, j].cost = 0;
                gameBoard[i, j].parentDir = Vector2Int.zero;
                //setTilemapSearched(new Vector2Int(i, j), 0);
                if (wallMap.GetTile(new Vector3Int(i - 15, j - 15, 0)) != null)
                {
                    gameBoard[i, j].isValid = false;
                }
                else
                {
                    gameBoard[i, j].isValid = true;
                    //drawSpot(new Vector2Int( i, j));
                    if (terrainMap.GetTile(new Vector3Int(i - 15, j - 15, 0)) != null)
                    {
                        if (terrainMap.GetTile(new Vector3Int(i - 15, j - 15, 0)).name == "TerrainSlow")
                        {
                            gameBoard[i, j].baseCost = .5f;
                        }
                        else if (terrainMap.GetTile(new Vector3Int(i - 15, j - 15, 0)).name == "TerrainFast")
                        {
                            gameBoard[i, j].baseCost = -.5f;
                        }
                    }
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
        if (Enemys.Count == 0)
        {
            return true;
        }
        for (int i = 0; i < Enemys.Count; i++)
        {
            if (Enemys[i] != null)
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
        // enemyMap.SetTile(new Vector3Int(position.x - 15, position.y - 15, 0), searchedTile);
    }

    void drawSpot(Vector2Int position)
    {
        enemyMap.SetTile(new Vector3Int(position.x - 15, position.y - 15, 0), null);
        enemyMap.SetTile(new Vector3Int(position.x - 15, position.y - 15, 0), searchedTile);
    }


    bool addPannel(panel newPanel, panel previousPanel, float moveCost, Queue<panel> searchQueue, Vector2Int dirFromOriginal, int enemyIndex)
    {
        if (newPanel.isValid)
        {
            if (newPanel.visited == false || newPanel.cost > previousPanel.cost + moveCost + newPanel.baseCost)
            {
                newPanel.visited = true;
                if (enemyPrefabs[enemyIndex].GetComponent<EnemyScript>().fly)
                {
                    newPanel.cost = previousPanel.cost + moveCost;
                }
                else
                {
                    newPanel.cost = newPanel.baseCost + previousPanel.cost + moveCost;
                }

                newPanel.addParrent(previousPanel);
                gameBoard[previousPanel.position.x + dirFromOriginal.x, previousPanel.position.y + dirFromOriginal.y] = newPanel;
                setTilemapSearched(newPanel.position, newPanel.cost);
                newPanel.calcTotalCost();

                searchQueue.Enqueue(newPanel);
                return true;
            }
        }
        return false;
    }

    bool validDiagonalMove(Vector2Int start, Vector2Int direction)
    {
        if (gameBoard[start.x + direction.x, start.y].isValid && gameBoard[start.x, start.y + direction.y].isValid)
        {
            return true;
        }
        return false;
    }

    void getDirectionsToPoint(Vector2Int point, int index)
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

            if (enemyPrefabs[index].GetComponent<EnemyScript>().hop)
            {
                int movecost = 2;
                if (tempPanel.position.x > 1)
                {

                    panel westPanel = gameBoard[tempPanel.position.x - 2, tempPanel.position.y];
                    if (addPannel(westPanel, tempPanel, movecost, searchQueue, new Vector2Int(-2, 0), index))
                    {
                        if (westPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }
                }

                if (tempPanel.position.x < 28)
                {
                    panel eastPanel = gameBoard[tempPanel.position.x + 2, tempPanel.position.y];
                    if (addPannel(eastPanel, tempPanel, movecost, searchQueue, new Vector2Int(2, 0), index))
                    {
                        if (eastPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }

                }
                if (tempPanel.position.y > 1)
                {
                    panel southPanel = gameBoard[tempPanel.position.x, tempPanel.position.y - 2];
                    if (addPannel(southPanel, tempPanel, movecost, searchQueue, new Vector2Int(0, -2), index))
                    {
                        if (southPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }

                }
                if (tempPanel.position.y < 28)
                {
                    panel northPanel = gameBoard[tempPanel.position.x, tempPanel.position.y + 2];

                    if (addPannel(northPanel, tempPanel, movecost, searchQueue, new Vector2Int(0, 2), index))
                    {
                        if (northPanel.position == finalPoint.position)
                        {
                            break;
                        }
                    }


                }
            }
            if (tempPanel.position.x > 0)
            {

                panel westPanel = gameBoard[tempPanel.position.x - 1, tempPanel.position.y];

                if (addPannel(westPanel, tempPanel, 1, searchQueue, new Vector2Int(-1, 0), index))
                {
                    if (westPanel.position == finalPoint.position)
                    {
                        break;
                    }
                }

            }


            if (tempPanel.position.x < 29)
            {
                panel eastPanel = gameBoard[tempPanel.position.x + 1, tempPanel.position.y];

                if (addPannel(eastPanel, tempPanel, 1, searchQueue, new Vector2Int(1, 0), index))
                {
                    if (eastPanel.position == finalPoint.position)
                    {
                        break;
                    }
                }


            }
            if (tempPanel.position.y > 0)
            {
                panel southPanel = gameBoard[tempPanel.position.x, tempPanel.position.y - 1];

                if (addPannel(southPanel, tempPanel, 1, searchQueue, new Vector2Int(0, -1), index))
                {
                    if (southPanel.position == finalPoint.position)
                    {
                        break;
                    }
                }

            }
            if (tempPanel.position.y < 29)
            {
                panel northPanel = gameBoard[tempPanel.position.x, tempPanel.position.y + 1];

                if (addPannel(northPanel, tempPanel, 1, searchQueue, new Vector2Int(0, 1), index))
                {
                    if (northPanel.position == finalPoint.position)
                    {
                        break;
                    }
                }


            }

            if (enemyPrefabs[index].GetComponent<EnemyScript>().diagonalMove)
            {
                if (tempPanel.position.x > 0 && tempPanel.position.y > 0)
                {
                    panel southWest = gameBoard[tempPanel.position.x - 1, tempPanel.position.y - 1];

                    if (validDiagonalMove(tempPanel.position, new Vector2Int(-1, -1)))
                    {

                        if (addPannel(southWest, tempPanel, 1.414f, searchQueue, new Vector2Int(-1, -1), index))
                        {
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

                    if (validDiagonalMove(tempPanel.position, new Vector2Int(-1, 1)))
                    {
                        if (addPannel(northWest, tempPanel, 1.414f, searchQueue, new Vector2Int(-1, 1), index))
                        {
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


                    if (validDiagonalMove(tempPanel.position, new Vector2Int(1, -1)))
                    {
                        if (addPannel(southEast, tempPanel, 1.414f, searchQueue, new Vector2Int(1, -1), index))
                        {
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


                    if (validDiagonalMove(tempPanel.position, new Vector2Int(1, 1)))
                    {
                        if (addPannel(northEast, tempPanel, 1.414f, searchQueue, new Vector2Int(1, 1), index))
                        {
                            if (northEast.position == finalPoint.position)
                            {
                                break;
                            }
                        }
                    }

                }

            }



            priorityQueue = searchQueue.ToList<panel>();

            priorityQueue.Sort((x, y) => x.totalCost.CompareTo(y.totalCost));
            count += 8;

            searchQueue = new Queue<panel>(priorityQueue);




        }




        searchQueue.Clear();

        panel currentPoint = gameBoard[point.x, point.y];
        int totalSteps = 0;
        Color col = Random.ColorHSV();

        pathPoints[index].Insert(0, currentPoint.getWorldPosition());

        while (currentPoint.position != panelPos)
        {
            panel parrent = gameBoard[currentPoint.position.x + currentPoint.parentDir.x, currentPoint.position.y + currentPoint.parentDir.y];

            Debug.DrawLine(currentPoint.getWorldPosition(), parrent.getWorldPosition(), col, 100000);

            currentPoint = parrent;
            pathPoints[index].Insert(0, parrent.getWorldPosition());
            totalSteps++;
            if (totalSteps > 150) { break; }





        }

        //pathPoints.Insert(0, currentPoint.getWorldPosition());

    }

    public bool isValidPath()
    {
        enemyMap.ClearAllTiles();
        resetTileMap();
        pathPoints = new List<List<Vector3Int>>();
        pathPoints.Add(new List<Vector3Int>());
        //Debug.Log(pathPoints.Count);
        getDirectionsToPoint(Vector2Int.FloorToInt((Vector2)(targetPoint.transform.position)), 0);

        for (int i = 1; i < pathPoints[0].Count; i++)
        {
            // Debug.Log(pathPoints[i]);
            if (pathPoints[0][i] - pathPoints[0][i - 1] == Vector3Int.zero)
                return false;
        }
        return true;
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
