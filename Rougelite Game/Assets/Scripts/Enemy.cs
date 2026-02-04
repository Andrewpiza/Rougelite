using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]private float sightRange = 2;
    private GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)FollowTarget(target);
        else
        {
            foreach (GameObject entity in GameObject.FindGameObjectsWithTag("Entity"))
            {
                if (entity.name == "Player" && Vector2.Distance(entity.transform.position,transform.position) < sightRange)
                {
                    target = entity;
                }
            }
        }
    }

    
}
