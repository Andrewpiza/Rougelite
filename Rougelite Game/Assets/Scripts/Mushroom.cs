using UnityEngine;

public enum Type
{
    Red,
    Yellow,
    Blue,
    COUNT
}
public class Mushroom : Entity
{
    public enum Task
    {
        Idle,
        FollowPlayer
    }
    [SerializeField]private Type mushroomType;

    // Other
    private GameObject player;
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
                FollowTarget(player);
                break;
        }
    }

    public void SetTask(Task task_)
    {
        task = task_;
    }

    public Task GetTask()
    {
        return task;
    }

    public Type GetMushroomType()
    {
        return mushroomType;
    }
}
