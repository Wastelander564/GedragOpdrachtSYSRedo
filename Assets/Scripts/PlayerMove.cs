using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float horizontalRadius = 5f;
    public float verticalRadius = 3f;
    public float speed = 1f;
    public Vector3 center;

    private float angle = 0f;

    void Start()
    {
        // If no center specified, use current position
        if (center == Vector3.zero)
            center = transform.position;
    }

    void Update()
    {
        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * horizontalRadius;
        float z = Mathf.Sin(angle) * verticalRadius;

        transform.position = center + new Vector3(x, 0f, z);
    }
}
