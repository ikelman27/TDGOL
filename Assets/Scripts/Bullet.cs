using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public Vector3 direction;
    bool isFired = false;
    float power;
    float startTime;
    // Use this for initialization
    void Start () {
		
	}

    

    public void Fire(float newspeed, Vector3 newDir, float dammage)
    {
        startTime = Time.time;
        speed = newspeed;
        direction = newDir;
        isFired = true;
        power = dammage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().takeDammage();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (isFired)
        {
            Vector3 pos = transform.position;
            pos += speed * direction * Time.deltaTime;
            transform.position = pos;

            if(Time.time -startTime > 5)
            {
                Destroy(this.gameObject);
            }

        }
	}
}
