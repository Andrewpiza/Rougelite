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

        Vector3 targetPos = (Vector2.one+target.GetComponent<Rigidbody2D>().linearVelocity.normalized) * target.transform.position;
        Move((targetPos-transform.position).normalized);
    }
}
