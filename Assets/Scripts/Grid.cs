using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Tilemaps;

//A script that manages all drawing calls made to the grid

    //TODO: Refactor and osrganize
public class Grid : MonoBehaviour
{

    public bool isActive;

    public List<Tile> drawTiles;
    public Tile grassTile;
    public Tilemap hoverTileMap;
    public Tilemap placeTileMap;
    public Tilemap RoomTileMap;
    public Tile hoverTile;

    public Tile setTile;
    public Tile errorTile;

    Vector3Int previous;
    Vector2 CamPosGrid;
    Vector3Int currentCell;
    public Dropdown typeDropdown;
    public int drawType;

    Vector3Int firstPoint;

    Vector2Int size;
    RoomManager roomController;
    public Tile RoomTile;
    private void Start()
    {

        roomController = FindObjectOfType<RoomManager>();
        size = new Vector2Int(50, 50);

        typeDropdown.onValueChanged.AddListener(delegate
        {
            changeDrawType(typeDropdown);
        });

        drawType = 0;
        
    }

    private void Update()
    {


        if (isActive)
        {
            CamPosGrid = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentCell = hoverTileMap.WorldToCell(CamPosGrid);


            hoverTileMap.ClearAllTiles();
            if (drawType == 0)
            {
                if (Input.GetButtonDown("Fire1") && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {

                    placeTileMap.SetTile(currentCell, setTile);

                }

                hoverTileMap.SetTile(currentCell, hoverTile);
                if (currentCell != previous)
                {
                    previous = currentCell;
                }
            }

            else if (drawType == 1)
            {
                if (Input.GetButton("Fire1") && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    placeTileMap.SetTile(currentCell, setTile);

                }
                hoverTileMap.SetTile(currentCell, hoverTile);
                if (currentCell != previous)
                {
                    previous = currentCell;
                }
            }
            else if (drawType == 2 || drawType == 4)
            {
                Vector2Int horiz = new Vector2Int();
                Vector2Int vert = new Vector2Int();

                if (Input.GetButtonDown("Fire1") && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    firstPoint = currentCell;

                }


                if (Input.GetButton("Fire1"))
                {

                    if (firstPoint.x < currentCell.x)
                    {
                        horiz.x = firstPoint.x;
                        horiz.y = currentCell.x;
                    }
                    else
                    {
                        horiz.x = currentCell.x;
                        horiz.y = firstPoint.x;
                    }

                    if (firstPoint.y < currentCell.y)
                    {
                        vert.x = firstPoint.y;
                        vert.y = currentCell.y;
                    }
                    else
                    {
                        vert.x = currentCell.y;
                        vert.y = firstPoint.y;
                    }

                    Tile tempTile;
                    if (drawType == 2 || roomController.isValidRoom(new Vector2Int(horiz.y - horiz.x, vert.y - vert.x), new Vector2Int(horiz.x, vert.x)))
                    {
                        tempTile = hoverTile;
                    }
                    else
                    {
                        tempTile = errorTile;
                    }


                    for (int i = horiz.x; i <= horiz.y; i++)
                    {
                        for (int j = vert.x; j <= vert.y; j++)
                        {
                            hoverTileMap.SetTile(new Vector3Int(i, j, 0), tempTile);
                        }
                    }

                }
                else if (currentCell != previous)
                {
                    previous = currentCell;
                }
                hoverTileMap.SetTile(currentCell, hoverTile);



                if (Input.GetButtonUp("Fire1") && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    if (firstPoint.x < currentCell.x)
                    {
                        horiz.x = firstPoint.x;
                        horiz.y = currentCell.x;
                    }
                    else
                    {
                        horiz.x = currentCell.x;
                        horiz.y = firstPoint.x;
                    }

                    if (firstPoint.y < currentCell.y)
                    {
                        vert.x = firstPoint.y;
                        vert.y = currentCell.y;
                    }
                    else
                    {
                        vert.x = currentCell.y;
                        vert.y = firstPoint.y;
                    }
                    if (drawType == 2 || roomController.isValidRoom(new Vector2Int(horiz.y - horiz.x, vert.y - vert.x), new Vector2Int(horiz.x, vert.x)))
                    {
                        for (int i = horiz.x; i <= horiz.y; i++)
                        {

                            for (int j = vert.x; j <= vert.y; j++)
                            {
                                if (drawType == 2) { placeTileMap.SetTile(new Vector3Int(i, j, 0), setTile); }

                                else
                                {
                                    RoomTileMap.SetTile(new Vector3Int(i, j, 0), null);
                                    RoomTileMap.SetTile(new Vector3Int(i, j, 0), RoomTile);

                                }


                            }
                          
                        }
                        if (drawType == 4)
                            roomController.createRoom(new Vector2Int(horiz.y - horiz.x, vert.y - vert.x), new Vector2Int(horiz.x, vert.x));
                    }

                    //gameRoom.createRoom(new Vector2Int(3, 3), new Vector2Int(3, 2));

                }



            }
            else if (drawType == 3)
            {
                if (Input.GetButton("Fire1") && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    GetComponent<SceneManager>().createTurret(currentCell);

                }
                hoverTileMap.SetTile(currentCell, hoverTile);
                if (currentCell != previous)
                {
                    previous = currentCell;
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {

                isActive = false;
                hoverTileMap.SetTile(currentCell, null);
                hoverTileMap.ClearAllTiles();
            }
        }





    }





    public void setActive()
    {
        isActive = true;
    }


    public void chooseObject(int num)
    {
        setTile = drawTiles[num];
        //placeTileMap.SetTile(currentCell, null);
    }

    public void eraseTile()
    {
        setTile = null;
    }


    public void changeDrawType(Dropdown value)
    {
        drawType = value.value;

    }

    public void resetUI()
    {
        typeDropdown.value = 0;
        changeDrawType(typeDropdown);
        isActive = false;
    }

    public void setRoomDrawType()
    {
        drawType = 3;
    }

    /*
    
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            PaintActive = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            PaintActive = false;
        }
        if (PaintActive == true)
        {
            fireLaser();
        }
    }
    (*/
}
