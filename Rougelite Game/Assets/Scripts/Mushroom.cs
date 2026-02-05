using UnityEngine;

public enum Type
{
    Red,
    Yellow,
    Blue,
    None
}
public class Mushroom : Entity
{
    public enum Task
    {
        Idle,
        FollowPlayer,
        Thrown,
        AttackEnemy
    }
    [SerializeField]private Type mushroomType;
    
    // Other
    private Vector3 throwDirection;
    private GameObject player;
    private GameObject target;
    [SerializeField]private Task task;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (task)
        {
            case Task.Idle:
                //
                break;
            case Task.FollowPlayer:
                FollowTarget(player,1.25f);
                break;
            case Task.Thrown:
                if (Vector2.Distance(transform.position,throwDirection) < 0.2)rb.linearVelocity = Vector2.zero;
                break;
            case Task.AttackEnemy:
                FollowTarget(target);

                attackTimer += Time.deltaTime;
                if (attackTimer >= attackRate && Vector2.Distance(transform.position,target.transform.position) < 1f)
                {
                    target.GetComponent<Enemy>().TakeDamage(attackDamage);
                    attackTimer = 0;
                    if (target ==null)task = Task.Idle;
                }
                break;
        }
    }

    public void SetThrow(Vector3 pos, float threwStrength)
    {
        task = Task.Thrown;
        throwDirection = pos;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce((pos-transform.position).normalized*threwStrength);
    }

    public void SetTask(Task t)
    {
        task = t;
    }

    public Task GetTask()
    {
        return task;
    }

    public Type GetMushroomType()
    {
        return mushroomType;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Enemy")
        {
            task = Task.AttackEnemy;
            target = coll.gameObject;
        }
    }
}
