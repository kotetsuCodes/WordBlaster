using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public int EnemyShipHitPoints;

    AudioSource audioSource;
    public AudioClip[] DamageSounds;
    public AudioClip[] DestroySounds;
    TextMesh textMesh;
    SpriteRenderer selfSpriteRenderer;
    readonly float boulderSpeed = -0.01f;

    string shipText;
    readonly bool isCurrentWord;

    Renderer selfRenderer;

    public Transform FuelCellPrefabObj;

    bool _isDestroyed = false;

    // Use this for initialization
    void Start()
    {
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
        selfRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        textMesh.text = shipText;
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
            if (EnemyShipHitPoints <= 0)
            {
                var randomSoundIndex = Random.Range(0, 2);
                audioSource.clip = DestroySounds[randomSoundIndex];
                audioSource.Play();
                _isDestroyed = true;
                selfRenderer.enabled = false;
                textMesh.text = "";

                if (LevelManager.instance.CheckForWord(shipText))
                    Instantiate(FuelCellPrefabObj, transform.position, transform.rotation);
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

    public void SetShipText(string text)
    {
        shipText = text;
        EnemyShipHitPoints = text.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed() == false && collision.gameObject.tag == "LaserBlast")
        {
            Destroy(collision.gameObject);

            var randomSoundIndex = Random.Range(0, 2);

            audioSource.clip = DamageSounds[randomSoundIndex];
            audioSource.Play();
            EnemyShipHitPoints -= 1;
        }
    }

    private bool isDestroyed()
    {
        return _isDestroyed == true;
    }
}
