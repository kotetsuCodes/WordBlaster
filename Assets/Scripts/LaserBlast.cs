using UnityEngine;

public class LaserBlast : MonoBehaviour
{
    readonly float laserBlastSpeed = 0.1f;

    // Use this for initialization
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

        transform.position += transform.up * laserBlastSpeed;
    }
}
