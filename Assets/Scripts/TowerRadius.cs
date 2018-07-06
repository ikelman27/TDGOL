using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRadius : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Enemy")
            transform.parent.gameObject.GetComponent<TurretScrypt>().addEnemy(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent.gameObject.GetComponent<TurretScrypt>().removeEnemy(collision.gameObject);
    }


}
