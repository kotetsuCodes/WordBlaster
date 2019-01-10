using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public float moveSpeed = -0.01f;
    public bool MoveDown = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, transform.position) > 15)
        {
            Destroy(gameObject);
        }

        // we need to destroy the boulder
        if (MoveDown)
            transform.position += transform.up * moveSpeed;
        else
            transform.position += -transform.up * moveSpeed;
    }
}
