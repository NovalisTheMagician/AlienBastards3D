using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public delegate void OutOfBoundsHandler(GameObject bullet);
    public event OutOfBoundsHandler OnOutOfBounds;

    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float maxLifeTime = 1;

    private float curLifeTime = 0;
	
	void Update ()
    {
        Vector3 currentPos = this.transform.position;
        currentPos.y += speed * Time.deltaTime;
        this.transform.position = currentPos;

        curLifeTime += Time.deltaTime;

        if(curLifeTime >= maxLifeTime)
        {
            OnOutOfBounds(this.gameObject);
            this.gameObject.SetActive(false);
        }
	}

    public void ResetBullet()
    {
        curLifeTime = 0;
    }
}
