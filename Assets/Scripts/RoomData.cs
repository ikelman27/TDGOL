using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class RoomData {
    //Serializable info based off the ROom
    //pulled from JSON File
    public Vector2 minSize;
    public Vector2 maxSize;
    public string name;
    public colorData background;
    public Color color;
    public int roomCount = 1;


    public void makeColor()
    {
        color = new Color(background.r/255f, background.g/255f, background.b/255f, .6f);
    }

    //public GameObject[] neededObjects;
}

[System.Serializable]
public class colorData
{
    public float r;
    public float g;
    public float b;
    public float a;
}
