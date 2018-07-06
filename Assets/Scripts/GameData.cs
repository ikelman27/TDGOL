using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Reads info from main JSON Files and parses them into seperate filepaths

[System.Serializable]
public class GameData
{
    public List<pathData> filePaths;
 }

[System.Serializable]
public class pathData
{
    public string filePath;
}