using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeath;

    [SerializeField]
    private BulletPool bulletPool;

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private Transform bulletSpawn;

    [SerializeField]
    private float shootDelay = 1;

    private bool canShoot = false;
    private float timeSinceShot = 0;

	void Update ()
    {
        Vector3 currentPos = this.transform.position;

        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        if(xMove > 0)
        {
            this.transform.localEulerAngles = new Vector3(0, -25, 0);
        }
        else if(xMove < 0)
        {
            this.transform.localEulerAngles = new Vector3(0, 25, 0);
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        Vector3 move = new Vector3(xMove, yMove);
        move.Normalize();

        currentPos += move * speed * Time.deltaTime;

        if (!canShoot)
        {
            timeSinceShot += Time.deltaTime;
        }

        if (timeSinceShot >= shootDelay)
        {
            //timeSinceShot -= shootDelay;
            timeSinceShot = 0;
            canShoot = true;
        }

        if (Input.GetButton("Fire1") && canShoot)
        {
            bulletPool.CreateBullet(bulletSpawn.position);

            canShoot = false;
        }

        this.transform.position = currentPos;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnDeath();
        }
    }
}
