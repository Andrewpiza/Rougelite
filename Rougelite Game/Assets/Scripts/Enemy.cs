using UnityEngine;

public class Enemy : Entity
{
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetVelocity =target.GetComponent<Rigidbody2D>().linearVelocity.normalized;
        Vector3 targetPos = new Vector3(targetVelocity.x,targetVelocity.y,0) + target.transform.position;
        Move((targetPos-transform.position).normalized);
    }
}
