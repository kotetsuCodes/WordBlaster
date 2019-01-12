using UnityEngine;

public class WordAsteroid : MonoBehaviour
{

    public int BoulderHitPoints;

    AudioSource audioSource;
    public AudioClip[] DamageSounds;
    public AudioClip[] DestroySounds;
    TextMesh textMesh;
    SpriteRenderer selfSpriteRenderer;
    readonly float boulderSpeed = -0.01f;

    string asteroidText;
    readonly bool isCurrentWord;

    Renderer selfRenderer;
    public Transform SmallExplosionPrefabObj;

    bool _isDestroyed = false;

    // Use this for initialization
    void Start()
    {
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
        selfRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        textMesh.text = asteroidText;
        textMesh.transform.localPosition = new Vector3(textMesh.transform.localPosition.x, -0.61f, textMesh.transform.localPosition.z);

        // selfSpriteRenderer.transform.localScale = new Vector3(Random.Range( ,  , 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, transform.position) > 15)
        {
            Destroy(gameObject);
        }

        if (isDestroyed() == false)
        {
            // we need to destroy the boulder
            if (BoulderHitPoints <= 0)
            {
                var randomSoundIndex = Random.Range(0, 2);
                audioSource.clip = DestroySounds[randomSoundIndex];
                audioSource.Play();
                _isDestroyed = true;
                selfRenderer.enabled = false;
                textMesh.text = "";
            }
            // the boulder needs to move down
            else
            {
                transform.position += transform.up * boulderSpeed;
            }
        }

        if (audioSource.isPlaying == false && isDestroyed())
        {
            Destroy(gameObject);
        }
    }

    public void SetAsteroidText(string text)
    {
        asteroidText = text;
        BoulderHitPoints = text.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed() == false && collision.gameObject.tag == "LaserBlast")
        {
            Destroy(collision.gameObject);

            var smallExplosion = Instantiate(SmallExplosionPrefabObj, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.position.z - 1), transform.rotation);

            var randomScale = Random.Range(1, 3);

            smallExplosion.transform.localScale = new Vector3(randomScale, randomScale, 1);

            var randomSoundIndex = Random.Range(0, 2);

            audioSource.clip = DamageSounds[randomSoundIndex];
            audioSource.Play();
            BoulderHitPoints -= 1;
        }
    }

    private bool isDestroyed()
    {
        return _isDestroyed == true;
    }
}
