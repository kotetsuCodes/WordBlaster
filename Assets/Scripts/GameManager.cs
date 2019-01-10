using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string GameType = "Addition";

    public static GameManager instance = null;
    readonly List<HashSet<string>> challengeWords = new List<HashSet<string>>()
    {
        new HashSet<string> { "if", "is", "him", "rip", "fit", "pin", "and", "be", "you", "an", "bad", "can", "had", "cat", "ran"},
        new HashSet<string> { "ox", "log", "dot", "top", "hot", "lot", "fox", "dog", "us", "sun", "but", "fun", "bus", "run", "eat", "put", "one" },
        new HashSet<string> { "with", "away", "find", "sing", "yet", "wet", "web", "leg", "pen", "hen", "me", "my", "add", "pass" } ,
        new HashSet<string> { "up", "bug", "mud", "nut", "hug", "tub", "full", "pull", "take", "small", "give", "every" } ,
        new HashSet<string> { "does", "here", "am", "at", "sat", "man", "dad", "mat", "ran", "sad", "van", "mad", "on", "got", "fox", "pop",  } ,
        new HashSet<string> { "not", "hop", "her", "now", "that", "then", "this", "them", "with", "bath", "blue", "water", "live" } ,
        new HashSet<string> { "where", "their", "good", "hold", "many", "friend", "little", "today", "hibernate", "classify", "block", "clock", "would", "musical" } ,
    };

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        var randomIndex = Random.Range(0, challengeWords.Count - 1);
        LevelManager.instance.InitializeLevelManager(challengeWords[randomIndex]);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GameOver()
    {
        // do some stuff here
    }
}
