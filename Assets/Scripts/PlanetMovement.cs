using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public float PlanetSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PlanetSpeed = -Random.Range(0.002f, 0.008f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, transform.position) > 15)
        {
            Destroy(gameObject);
        }

        transform.position += transform.up * PlanetSpeed;
    }
}
