using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public float shootCooldown;
    float shootDelay;
    bool playerInRange;

    public GameObject bullet;
    public GameObject sp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            shootDelay -= Time.deltaTime;
        }

        if (shootDelay <= 0f)
        {
            Instantiate(bullet, sp.transform.position, sp.transform.rotation);
            shootDelay = shootCooldown;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
     void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
