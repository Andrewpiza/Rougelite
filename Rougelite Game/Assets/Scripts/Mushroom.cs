using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private float minDistanceFromPlayer = 1f;
    [SerializeField]private float defaultDistanceFromPlayer = 1f;
    [SerializeField]private float minDistanceFromMushrooms = 0.6f;
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
                MoveTowardsPlayer();
                break;
            case Task.Thrown:
                if (Vector2.Distance(transform.position,throwDirection) < 0.2)rb.linearVelocity = Vector2.zero;
                break;
            case Task.AttackEnemy:
                Move(GetDirectionToTarget(target));
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

    public void MoveTowardsPlayer()
    {
        List<GameObject> allMushrooms = player.GetComponent<Player>().GetMushroomsInSquad();

        minDistanceFromPlayer = defaultDistanceFromPlayer * (1+(allMushrooms.Count/10));

        Vector2 moveDirection = Vector2.zero;
        float distanceToPlayer = Vector2.Distance(transform.position,player.transform.position);
        if ( distanceToPlayer > minDistanceFromPlayer || IsSelectedType() && distanceToPlayer > minDistanceFromPlayer/2) moveDirection = GetDirectionToTarget(player);

        Vector2 directionToMushroom;
        

        foreach (GameObject mushroom in allMushrooms)
        {
            if (mushroom == gameObject)continue;
            directionToMushroom = transform.position - mushroom.transform.position;

            float distance = directionToMushroom.magnitude;

            if (distance < minDistanceFromMushrooms && distance > 0 ){
                moveDirection += directionToMushroom.normalized * (1- (distance/minDistanceFromMushrooms));
            }
        }

        moveDirection = moveDirection.normalized;

        Move(moveDirection);
    }

    public bool IsSelectedType()
    {
        return mushroomType == player.GetComponent<Player>().GetSelectedType();
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
        if (coll.gameObject.name == "Enemy" && task != Task.Idle)
        {
            task = Task.AttackEnemy;
            target = coll.gameObject;
        }
    }
}
