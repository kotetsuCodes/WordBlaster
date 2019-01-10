using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    readonly HashSet<string> currentWords = new HashSet<string>();
    readonly string selectedWord;

    // Use this for initialization
    void Start()
    {
        //nextAsteroidGenerationTime = Time.time;

        //var randomIndex = Random.Range(0, GameManager.instance.challengeWords.Count - 1);

        //foreach (var word in GameManager.instance.challengeWords[randomIndex])
        //    currentWords.Add(word);

        //var randomSelectedWordIndex = Random.Range(0, currentWords.Count - 1);

        //selectedWord = currentWords.ElementAt(randomSelectedWordIndex);


    }

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (nextAsteroidGenerationTime <= Time.time)
    //    {
    //        var clonedAsteroid = Instantiate<WordAsteroid>(asteroid, transform.position, transform.rotation);
    //        var randomSelectedWordIndex = Random.Range(0, currentWords.Count - 1);
    //        string asteroidWord = currentWords.ElementAt(randomSelectedWordIndex);

    //        clonedAsteroid.SetAsteroidText(asteroidWord);

    //        nextAsteroidGenerationTime = Time.time + 10.0f;
    //    }
    //}
}
