using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("UI")]
    public Slider timerSlider;
    public float maxTime;
    public float timer;

    [Header("Health")]
    public Slider healthSlider;
    public int maxHealth;
    public int currentHealth;

    [Header("Shooting")]
    public Transform shootingPoint;
    public GameObject bullet;
    bool isFacingRight;

    [Header("Main")]
    public float moveSpeed;
    public float jumpForce;
    float inputs;
    public Rigidbody2D rb;
    public float groundDistance;
    public LayerMask layerMask;

    [Header("WallJump")]
    public float wallCheckDistance;
    public bool nextToLeftWall;
    public bool nextToRightWall;
    public float gravityLimit;
    public float currentGravity;
    public float wallGravity;
    public LayerMask wallLayer;


    RaycastHit2D hit;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        startPos = transform.position;

        currentHealth = maxHealth;
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {

        Timer();
        
        Movement();
        GravityLimit();

        Health();
        // Shoot();
        MovementDirection();
    }

    void Timer()
    {
        if (inputs == 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = maxTime;
        }

        timerSlider.value = timer;
        timerSlider.maxValue = maxTime;

        if (timer <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Movement()
    {
        inputs = Input.GetAxisRaw("Horizontal");
        rb.velocity = new UnityEngine.Vector2(inputs * moveSpeed, rb.velocity.y);

        hit = Physics2D.Raycast(transform.position, -transform.up, groundDistance, layerMask);
        Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.yellow);

        if (hit.collider)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        WallJump();
    }

    void WallJump()
    {
        nextToLeftWall = Physics2D.Raycast(transform.position, -transform.right, wallCheckDistance, wallLayer);
        nextToRightWall = Physics2D.Raycast(transform.position, transform.right, wallCheckDistance, wallLayer);
        
        if (nextToLeftWall || nextToRightWall)
        {
            currentGravity = wallGravity;

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            currentGravity = gravityLimit;
        }
    }

    void GravityLimit()
    {
        if (rb.velocity.y < currentGravity)
        {
            Vector2 newVelocity;
            newVelocity = rb.velocity;
            newVelocity.y = currentGravity;
            rb.velocity = newVelocity;
        }
    }

void Health()
{
    healthSlider.value = currentHealth;
    if (currentHealth <= 0)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

void Shoot()
{
    if (Input.GetKeyDown(KeyCode.Z))
    {
        Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
    }
}

     void MovementDirection()
     {
        if (isFacingRight && inputs < -.1f)
        {
            Flip();
        
        }
        else if (!isFacingRight && inputs > .1f)
        {
            Flip();
        }
     }

     void Flip()
     {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
     }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            transform.position = startPos;
        }
        if (other.gameObject.CompareTag("Exit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
            if (other.gameObject.CompareTag("Enemy"))
        {
            currentHealth--;
            Destroy(other.gameObject);
        }

    }
}
