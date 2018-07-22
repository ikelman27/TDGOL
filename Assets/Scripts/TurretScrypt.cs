using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScrypt : MonoBehaviour {

    public float radius;
    public GameObject attackCircle;
    public GameObject bullet;
    public float reloadTime;
    public float bulletSpeed;
    public float dammage;
    public float price;
    float startReload;
    public List<GameObject> enemys;

	// Use this for initialization
	void Start () {
        startReload = -reloadTime;
        enemys = new List<GameObject>();
        attackCircle.transform.localScale = Vector3.one;
        attackCircle.transform.localScale = new Vector3(radius / transform.lossyScale.x, radius / transform.lossyScale.y);

        

    }

    public void addEnemy(GameObject enemy)
    {
       
        enemys.Add(enemy);
    }

    public void removeEnemy(GameObject enemy)
    {
        enemys.Remove(enemy);
    }
	
    public void aimAndFire()
    {
        Vector3 aim = enemys[0].transform.position - transform.position;
        float dist = Vector3.Magnitude(aim);
        aim = Vector3.Normalize(aim);

        float time = dist * (1.0f / bulletSpeed);

        Vector3 enemyPos = enemys[0].transform.position + enemys[0].GetComponent<EnemyScript>().dir * time * enemys[0].GetComponent<EnemyScript>().speed + new Vector3(.5f, .5f);
        Debug.DrawLine(transform.position, enemyPos, Color.black, 10000);
        aim = Vector3.Normalize(enemyPos - transform.position);
        GameObject tempObj = Instantiate(bullet, transform.position, Quaternion.identity);
        tempObj.GetComponent<Bullet>().Fire(bulletSpeed, aim, 1);
    }

	// Update is called once per frame
	void Update () {
		if(enemys.Count > 0)
        {
            if(Time.time > startReload + reloadTime)
            {
                aimAndFire();
                startReload = Time.time;
            }
            
        }
        if(enemys.Count > 1)
        {

            enemys.Sort((x, y) => y.GetComponent<EnemyScript>().percentToGoal.CompareTo(x.GetComponent<EnemyScript>().percentToGoal) );
            
        }
	}
}
