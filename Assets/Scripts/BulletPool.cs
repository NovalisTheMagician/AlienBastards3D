using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    [SerializeField]
    private int poolSize = 50;

    [SerializeField]
    private GameObject bulletPrefab;

    private GameObject[] bullets;

    private Bullet.OutOfBoundsHandler bulletCallback;
	
    public void SetBoundsCallback(Bullet.OutOfBoundsHandler callback)
    {
        bulletCallback = callback;
    }

	public void InitPool()
    {
        bullets = new GameObject[poolSize];
        for(int i = 0; i < poolSize; ++i)
        {
            GameObject bullet = Instantiate(bulletPrefab, this.transform);
            bullet.SetActive(false);
            bullet.GetComponent<Bullet>().OnOutOfBounds += bulletCallback;
            bullets[i] = bullet;
        }
	}

    public void CreateBullet(Vector3 position)
    {
        foreach(GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.GetComponent<Bullet>().ResetBullet();
                bullet.transform.position = position;
                bullet.SetActive(true);
                break;
            }
        }
    }
}
