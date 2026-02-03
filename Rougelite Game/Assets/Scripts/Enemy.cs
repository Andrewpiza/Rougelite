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
        Move((target.transform.position-transform.position).normalized);
    }
}
