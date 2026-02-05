using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]protected float maxHealth = 100; 
    [SerializeField]protected float currentHealth;

    [Header("Movement")]
    [SerializeField]protected float speed = 1000; 
    [SerializeField]protected float maxSpeed = 10; 

    [Header("Attack")]
    [SerializeField]protected float attackDamage = 1;
    [SerializeField]protected float attackRate = 1;
    protected float attackTimer;

    protected Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 dir)
    {
        rb.AddForce(dir * speed* Time.deltaTime);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public void MoveTo(Vector3 pos)
    {
        Move((pos-transform.position).normalized);
    }

    public void FollowTarget(GameObject target, float bonusSpeed = 1)
    {
        Vector2 targetVelocity =target.GetComponent<Rigidbody2D>().linearVelocity.normalized;
        Vector3 targetPos = new Vector3(targetVelocity.x,targetVelocity.y,0) + target.transform.position;
        Move((targetPos-transform.position).normalized *bonusSpeed);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)Destroy(gameObject);
    }
}
