using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlls : MonoBehaviour
{
    //size of the level
    public Vector2 size;
    Camera mainCamera;
    public float maxZoom = 1;
    public float minZoom = 10;

    // Use this for initialization
    void Start()
    {
        size = new Vector2(50, 50);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        //get scroll data
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //if the scroll wheel is moving zoom in or out
        if (scroll > 0f)
        {
            zoomCamera(mainCamera.ScreenToWorldPoint(Input.mousePosition), 1f);
        }
        else if (scroll < 0f)
        {
            zoomCamera(mainCamera.ScreenToWorldPoint(Input.mousePosition), -1f);
        }

        //move camera based off of input
        Vector3 camPos = mainCamera.transform.position;
        if (Input.GetKey(KeyCode.W))
            camPos.y += .05f * mainCamera.orthographicSize/2;
        if (Input.GetKey(KeyCode.S))
            camPos.y -= .05f * mainCamera.orthographicSize/2;
        if (Input.GetKey(KeyCode.A))
            camPos.x -= .05f * mainCamera.orthographicSize/2;
        if (Input.GetKey(KeyCode.D))
            camPos.x += .05f * mainCamera.orthographicSize/2;


        //clamp camera position
        if (camPos.x > size.x / 2)
        {
            camPos.x = size.x / 2;
        }
        else if (camPos.x < -size.x / 2)
        {
            camPos.x = -size.x / 2;
        }

        if (camPos.y > size.y / 2)
        {
            camPos.y = size.y / 2;
        }
        else if (camPos.y < -size.y / 2)
        {
            camPos.y = -size.y / 2;
        }

        mainCamera.transform.position = camPos;

    }


    //zoom based off the mouse position, so the mouse stays hovered over the same object
    //basic equation is (mousePos-CamPos)/CamSize * zoomAmmount
    void zoomCamera(Vector3 mousePos, float ammount)
    {


        Vector3 camPos = mainCamera.transform.position;

        float multiplier = (1.0f / mainCamera.orthographicSize * ammount);


        camPos += (mousePos - mainCamera.transform.position) * multiplier;

        if (mainCamera.orthographicSize != maxZoom && mainCamera.orthographicSize != minZoom)
        {
            mainCamera.transform.position = camPos;
        }
        mainCamera.orthographicSize -= ammount;


        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, maxZoom, minZoom);

    }
}
