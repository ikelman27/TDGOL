using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Tilemaps;
public class ObjectManager : MonoBehaviour {

    private string roomFileName = "objectData.json";
    public GameData loadedData;
    public List<ObjectData> createableObjects;
    public Dictionary<string, ObjectData> objectDict;

    // Use this for initialization
    void Start () {
        objectDict = new Dictionary<string, ObjectData>();
        loadFilePath();
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
                loadObjFilePath(loadedData.filePaths[i].filePath);
            }

        }
    }

    void loadObjFilePath(string roomPath)
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, roomPath);

        string jsonData = File.ReadAllText(filepath);
        ObjectData tempObj = JsonUtility.FromJson<ObjectData>(jsonData);

        
        createableObjects.Add(tempObj);
        Debug.Log(tempObj.name);
        objectDict.Add(tempObj.name, tempObj);


    }


    // Update is called once per frame
    void Update () {
		
	}
}
