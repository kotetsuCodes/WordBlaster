using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    public bool LevelCompleted = false;

    HashSet<string> challengeWordsInPlay = new HashSet<string>();
    public int MaxWords;

    public WordAsteroid asteroid;
    public EnemyShip enemyShip;

    public float EnemyShipGenerationTime;
    public float AsteroidGenerationTime;

    public float PlanetGenerationTimeLowerBound;
    public float PlanetGenerationTimeUpperBound;
    float nextPlanetGenerationTime;

    public Sprite[] PlanetSprites;
    public Transform[] PlanetPrefabs;
    Transform currentPlanet;

    public GameObject PlayerShipObj;
    public SpaceshipScrollerController PlayerShip;

    Canvas uiCanvas;

    public Text FuelText;

    //public Transform TinyPlanet;
    //public Transform SmallPlanet;
    //public Transform MediumPlanet;
    //public Transform LargePlanet;
    //public Transform ExtraLargePlanet;
    //public Transform GiantPlanet;

    float nextAsteroidGenerationTime;
    float nextEnemyShipGenerationTime;
    public string CurrentEnemyShipWord;

    private bool asteroidBlasterIsReady;

    public Image FuelTankImage;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerShip = PlayerShipObj.GetComponent<SpaceshipScrollerController>();
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        setupUI();
    }

    private void setupUI()
    {
        GameObject imageContainerObj = new GameObject();
        Image imageContainer = imageContainerObj.AddComponent<Image>();
        imageContainer.type = Image.Type.Sliced;

        imageContainer.color = Color.black;

        var rectTransform = imageContainer.GetComponent<RectTransform>();
        rectTransform.SetParent(uiCanvas.transform);

        rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
        rectTransform.offsetMax = new Vector2(0.0f, 0.0f);

        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.localScale = new Vector2(1, 1);
        rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.5f, 1.0f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
        imageContainerObj.SetActive(true);

        GameObject fuelTankImageObj = new GameObject();
        FuelTankImage = fuelTankImageObj.AddComponent<Image>();
        FuelTankImage.sprite = GameManager.instance.FuelText100Sprite;
        var fuelTankImageRectTransform = FuelTankImage.GetComponent<RectTransform>();
        fuelTankImageRectTransform.SetParent(imageContainer.transform);

        fuelTankImageRectTransform.offsetMin = new Vector2(0.0f, 0.0f);
        fuelTankImageRectTransform.offsetMax = new Vector2(0.0f, 0.0f);

        fuelTankImageRectTransform.anchoredPosition = new Vector2(0, 0);
        fuelTankImageRectTransform.localScale = new Vector3(1, 1);
        fuelTankImageRectTransform.anchorMin = new Vector2(0.0f, 0.5f);
        fuelTankImageRectTransform.anchorMax = new Vector2(0.0f, 0.5f);
        fuelTankImageRectTransform.pivot = new Vector2(0.0f, 0.5f);
        fuelTankImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25);
        fuelTankImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 170);
        fuelTankImageObj.SetActive(true);

        GameObject fuelTextObj = new GameObject();
        FuelText = fuelTextObj.AddComponent<Text>();

        FuelText.fontSize = 24;
        FuelText.alignment = TextAnchor.MiddleCenter;
        FuelText.color = Color.white;

        var fuelTextRectTransform = FuelText.GetComponent<RectTransform>();

        fuelTextRectTransform.SetParent(imageContainer.transform);

        fuelTextRectTransform.offsetMin = new Vector2(0.25f, 0);
        fuelTextRectTransform.offsetMax = new Vector2(0.25f, 0);

        fuelTextRectTransform.anchoredPosition = new Vector2(62, 0);
        fuelTextRectTransform.localScale = new Vector3(1, 1);
        fuelTextRectTransform.anchorMin = new Vector2(0.0f, 0.5f);
        fuelTextRectTransform.anchorMax = new Vector2(0.0f, 0.5f);
        fuelTextRectTransform.pivot = new Vector2(0.0f, 0.5f);
        fuelTextRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 72);
        fuelTextRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 256);
        FuelText.font = GameManager.instance.MainFont;
    }

    public void InitializeLevelManager(HashSet<string> possibleWords)
    {
        nextEnemyShipGenerationTime = Time.time;
        nextAsteroidGenerationTime = Time.time;
        nextPlanetGenerationTime = Time.time + UnityEngine.Random.Range(3, 10);

        while (challengeWordsInPlay.Count < MaxWords)
        {
            var randomWordIndex = UnityEngine.Random.Range(0, possibleWords.Count - 1);
            var randomWord = possibleWords.ElementAt(randomWordIndex);

            if (!challengeWordsInPlay.Contains(randomWord))
            {
                challengeWordsInPlay.Add(randomWord);
            }
        }

        var randomIndex = UnityEngine.Random.Range(0, challengeWordsInPlay.Count - 1);
        CurrentEnemyShipWord = challengeWordsInPlay.ElementAt(randomIndex);

        WindowsVoice.speak($"Destroy the ships with the word {CurrentEnemyShipWord}");

        asteroidBlasterIsReady = true;
    }

    private void Update()
    {
        // get fuel value


        // generate planets
        if (currentPlanet == null && nextPlanetGenerationTime <= Time.time)
        {
            // select random planet prefab
            var randomPlanetPrefabIndex = UnityEngine.Random.Range(0, PlanetPrefabs.Length - 1);
            // select random planet sprite
            var randomPlanetSpriteIndex = UnityEngine.Random.Range(0, PlanetSprites.Length - 1);

            currentPlanet = Instantiate(PlanetPrefabs[randomPlanetPrefabIndex], new Vector3(transform.position.x + UnityEngine.Random.Range(-10.0f, 10.0f), transform.position.y + 5), transform.rotation);

            currentPlanet.GetComponent<SpriteRenderer>().sprite = PlanetSprites[randomPlanetSpriteIndex];

            nextPlanetGenerationTime = Time.time + UnityEngine.Random.Range(PlanetGenerationTimeLowerBound, PlanetGenerationTimeUpperBound);
        }

        // generate asteroids
        if (asteroidBlasterIsReady && nextAsteroidGenerationTime <= Time.time)
        {
            var clonedAsteroid = Instantiate<WordAsteroid>(asteroid, new Vector3(transform.position.x + UnityEngine.Random.Range(-5.0f, 5.0f), transform.position.y, -0.1f), transform.rotation);

            nextAsteroidGenerationTime = Time.time + AsteroidGenerationTime;
        }

        if (asteroidBlasterIsReady && nextEnemyShipGenerationTime <= Time.time)
        {
            var clonedEnemyShip = Instantiate<EnemyShip>(enemyShip, new Vector3(transform.position.x + UnityEngine.Random.Range(-5.0f, 5.0f), transform.position.y, -0.1f), transform.rotation);
            var randomSelectedWordIndex = UnityEngine.Random.Range(0, challengeWordsInPlay.Count - 1);
            string enemyShipWord = challengeWordsInPlay.ElementAt(randomSelectedWordIndex);

            clonedEnemyShip.SetShipText(enemyShipWord);

            nextEnemyShipGenerationTime = Time.time + EnemyShipGenerationTime;
        }
    }

    public bool CheckForWord(string word)
    {
        bool isCurrentWord = word.Equals(CurrentEnemyShipWord, StringComparison.OrdinalIgnoreCase);
        if (isCurrentWord)
        {
            // increase score
        }

        return isCurrentWord;
    }

    public void SpeakCurrentWord()
    {
        WindowsVoice.speak($"Destroy the enemy ships with the word {CurrentEnemyShipWord}");
    }

    private string getRandomWord(HashSet<string> wordList)
    {
        System.Random random = new System.Random();

        var randomWord = wordList.ElementAt(random.Next(0, wordList.Count));

        return randomWord;
    }

    public class ChallengeWord
    {
        public string Word { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCurrentWord { get; set; }
        public Text ChallengeModeWordText { get; set; }
    }
}
