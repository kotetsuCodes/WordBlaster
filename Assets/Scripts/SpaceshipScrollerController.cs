using UnityEngine;

public class SpaceshipScrollerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float MoveSpeed;
    public float MaxSpeed;
    public float SpeedIncrement;
    AudioSource audioSource;

    public AudioClip FuelCellClip;
    public AudioClip ThrusterClip;

    public Transform LaserBlast;

    public int MaxFuel = 100;
    public int StartingFuel = 100;
    public int CurrentFuel;

    float nextFuelUsageTime;

    // Use this for initialization
    void Start()
    {
        CurrentFuel = StartingFuel;

        nextFuelUsageTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CurrentFuel);

        if (nextFuelUsageTime <= Time.time)
        {
            CurrentFuel = Mathf.Clamp(CurrentFuel - 1, 0, MaxFuel);
            nextFuelUsageTime = Time.time + 2;
        }

        if (CurrentFuel > 0)
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");

            if ((horizontalAxis != 0 || verticalAxis != 0) && audioSource.isPlaying == false)
            {
                audioSource.clip = ThrusterClip;
                audioSource.Play();
            }

            if (Input.GetButtonUp("Submit"))
            {
                Instantiate(LaserBlast, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation);
                audioSource.Play();
            }

            if (Input.GetButtonDown("Fire3"))
            {
                LevelManager.instance.SpeakCurrentWord();
            }

            var cameraViewportPointX = Camera.main.WorldToViewportPoint(transform.position).x;
            var cameraViewportPointY = Camera.main.WorldToViewportPoint(transform.position).y;

            if (cameraViewportPointX > 1)
            {
                transform.position = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x, transform.position.y, transform.position.z);
            }
            else if (cameraViewportPointX < 0)
            {
                transform.position = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x, transform.position.y, transform.position.z);
            }
            else
            {
                // left movement
                if (horizontalAxis < 0.0f && Mathf.Abs(rigidbody2d.velocity.x) < MaxSpeed)
                {
                    if (rigidbody2d.velocity.x > 0.0f)
                        rigidbody2d.velocity = new Vector2(0.0f, rigidbody2d.velocity.y);

                    rigidbody2d.AddForce(-transform.right * SpeedIncrement);
                }
                // right movement
                else if (horizontalAxis > 0.0f && Mathf.Abs(rigidbody2d.velocity.x) < MaxSpeed)
                {
                    if (rigidbody2d.velocity.x < 0.0f)
                        rigidbody2d.velocity = new Vector2(0.0f, rigidbody2d.velocity.y);

                    rigidbody2d.AddForce(transform.right * SpeedIncrement);
                }
                else
                {
                    rigidbody2d.velocity = new Vector2(Mathf.Lerp(rigidbody2d.velocity.x, 0.0f, Time.deltaTime), rigidbody2d.velocity.y);
                }
            }

            // down movement
            if (verticalAxis < 0.0f)
            {
                if (rigidbody2d.velocity.y > 0.0f)
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0.0f);

                rigidbody2d.AddForce(-transform.up * MaxSpeed);

            }
            // up movement
            else if (verticalAxis > 0.0f)
            {
                if (rigidbody2d.velocity.y < 0.0f)
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0.0f);

                rigidbody2d.AddForce(transform.up * MaxSpeed);
            }
            else
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, Mathf.Lerp(rigidbody2d.velocity.y, 0.0f, Time.deltaTime));
            }

            if (cameraViewportPointY > 1)
            {
                transform.position = new Vector3(transform.position.x, Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y, transform.position.z);
            }
            else if (cameraViewportPointY < 0)
            {
                transform.position = new Vector3(transform.position.x, Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).y, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FuelCell")
        {
            audioSource.clip = FuelCellClip;
            audioSource.Play();
            CurrentFuel = Mathf.Clamp(CurrentFuel + 10, 0, MaxFuel);
            Destroy(collision.gameObject);
        }
    }
}
