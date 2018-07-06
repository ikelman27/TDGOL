using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectData {
    //Price to buy item in game
    int price;
    
    public string name;

    //the number of tiles that the object will take up
    Vector2Int dimensions;
    //Can pawns walk over this object
    bool walkable;
    //how fast pawns walk over it
    //in json this is an Int, so I've made it a percent out of 100
    float movementModifier;


}
