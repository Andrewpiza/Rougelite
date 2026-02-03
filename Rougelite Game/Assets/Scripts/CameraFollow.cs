using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private GameObject follow;

    [SerializeField]private float speed = 0.25f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, follow.transform.position, speed);

        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
