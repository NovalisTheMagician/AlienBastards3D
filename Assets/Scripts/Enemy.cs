using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public delegate void DeathEventHandler(GameObject e);
    public event DeathEventHandler OnDeath;

    public delegate void DespawnEventHandler(GameObject e);
    public event DespawnEventHandler OnDespawn;

    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private Material dissolveMatSource;

    [SerializeField]
    private Renderer renderer;

    private Material dissolveMat;

    private bool dead;

    [SerializeField]
    private float timeToDelete = 1;

    private float curDeathTime = 0;

    private void Start()
    {
        dissolveMat = new Material(dissolveMatSource);
    }

    void Update ()
    {
        Vector3 currentPos = this.transform.position;
        currentPos.y -= speed * Time.deltaTime;
        this.transform.position = currentPos;

        if(dead)
        {
            renderer.material = dissolveMat;

            curDeathTime += Time.deltaTime;
            if(curDeathTime >= timeToDelete)
            {
                OnDespawn(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                float factor = 1.0f - (curDeathTime / timeToDelete);
                dissolveMat.SetFloat("_DissolveFactor", factor);
            }
        }
    }

    private void LateUpdate()
    {
        if (transform.position.y < -20)
        {
            OnDespawn(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Bullet") && !dead)
        {
            OnDeath(this.gameObject);
            other.gameObject.SetActive(false);
            dead = true;
            Destroy(this.GetComponent<Rigidbody>());
        }
    }
}
