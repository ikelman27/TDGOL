using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Tilemaps;


//Creates and manages the Room Objects in the Game

    //TODO: Make setValid method that turns a rooms color red and shows allert if all criterea aren't met
public class RoomManager : MonoBehaviour {

    public GameObject roomPrefab;
    private string roomFileName = "roomData.json";
    public  GameData loadedData;
    public List<RoomData> creatableRooms;
    public List<GameObject> rooms;
    RoomData currentRoom;
    public Tile RoomTile;
    public  Dictionary<string, RoomData> roomDict;
    

	// Use this for initialization
	void Start () {
        
        roomDict = new Dictionary<string, RoomData>();
        loadFilePath();
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void loadFilePath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, roomFileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            loadedData = JsonUtility.FromJson<GameData>(jsonData);
            for (int i = 0; i < loadedData.filePaths.Count; i++)
            {
                loadRoomFilePath(loadedData.filePaths[i].filePath);
            }
          
        }
    }

    void loadRoomFilePath(string roomPath)
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, roomPath);

        string jsonData = File.ReadAllText(filepath);
        RoomData tempRoom = JsonUtility.FromJson<RoomData>(jsonData);
      
        tempRoom.makeColor();
        creatableRooms.Add(tempRoom);
        Debug.Log(tempRoom.name);
        roomDict.Add(tempRoom.name, tempRoom);


    }

    public void setCurrentRoom(string name)
    {
        currentRoom = roomDict[name];
        RoomTile.color = currentRoom.color;
    }

    public void createRoom(Vector2Int size, Vector2Int pos)
    {
        
        GameObject tempRoom = Instantiate(roomPrefab);
        tempRoom.GetComponent<Room>().setRoom(currentRoom, size, pos);
        rooms.Add(tempRoom);
        
    }

    public bool isValidRoom(Vector2Int size, Vector2Int pos)
    {
        if(size.x > currentRoom.maxSize.x || size.y > currentRoom.maxSize.y || size.x < currentRoom.minSize.x || size.y < currentRoom.minSize.y)
        {
           
            return false;
        }
        for(int i = 0; i < rooms.Count; i++)
        {
            Room testRoom = rooms[i].GetComponent<Room>();
            if(pos.x >= testRoom.position.x && pos.x <= testRoom.position.x + testRoom.size.x)
            {
                if ((pos.y >= testRoom.position.y && pos.y <= testRoom.position.y + testRoom.size.y) || (testRoom.position.y >= pos.y && testRoom.position.y <= pos.y + size.y))
                {

                    return false; 
                }
            }
            else if(testRoom.position.x >= pos.x && testRoom.position.x <= pos.x + size.x)
            {
                if ((pos.y >= testRoom.position.y && pos.y <= testRoom.position.y + testRoom.size.y) || (testRoom.position.y >= pos.y && testRoom.position.y <= pos.y + size.y))
                {
                    
                    return false;
                }
            }
        }
        
        return true;
    }


    /*
    public void createRoom(Vector2Int size, Vector2Int position)
    {
        loadedData.position = position;
        loadedData.size = size;
        Debug.Log("test");
    }*/
}
