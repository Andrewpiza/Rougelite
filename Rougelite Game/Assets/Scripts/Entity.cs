using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]protected float maxHealth = 100; 
    [SerializeField]protected float currentHealth;

    [Header("Movement")]
    [SerializeField]protected float speed = 1000; 
    [SerializeField]protected float maxSpeed = 10; 
    protected Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 dir)
    {
        rb.AddForce(dir * speed* Time.deltaTime);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
