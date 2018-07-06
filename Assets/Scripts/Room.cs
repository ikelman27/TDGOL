using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //size of the room
    public Vector2Int size;
    //position in 2d Space
    public Vector2Int position;
    public string name;
    //color over the room
    public Color color;
    //the Serializable object that the room is made from
    RoomData rootObj;

    //list of objects needed to complete the room
    public List<GameObject> neededObjects;
    //list of objects in the room
    public List<GameObject> addedObjects;

    bool hasAllObjects;

    //TODO: Connect To Room manager for tile color based off completion (Turn hover Color Red and add alert if all room needs aren't met

    


    // Use this for initialization
    void Start()
    {

    }
    //sets the room based off the positon and serializable object that it is based on
    public void setRoom(RoomData roomClass, Vector2Int newSize, Vector2Int newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y);
        size = newSize;
        position = newPosition;

        name = roomClass.name + " " + roomClass.roomCount.ToString();
        gameObject.name = name;

        roomClass.roomCount++;
        color = roomClass.color;
        rootObj = roomClass;
    }

    //Searches for all pawns and Objects in the scene and checks if they're in the room.
    //TODO: Optomize
    void findGameObjInRoom()
    {
        List<GameObject> allObj = new List<GameObject>();
        allObj.AddRange( GameObject.FindGameObjectsWithTag("Pawn"));
        allObj.AddRange(GameObject.FindGameObjectsWithTag("Object"));
        for (int i = 0; i < allObj.Count; i++)
        {
            if(allObj[i].transform.position.x > position.x && allObj[i].transform.position.x < position.x + size.x)
            {
                if (allObj[i].transform.position.y > position.y && allObj[i].transform.position.y < position.y + size.y)
                {
                    Debug.Log(allObj[i].name);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        findGameObjInRoom();
    }
}
