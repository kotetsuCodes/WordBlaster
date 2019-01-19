using System.Collections;
using UnityEngine;

public class SmallExplosion : MonoBehaviour
{
    Animator smallExplodeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        smallExplodeAnimator = GetComponent<Animator>();
        smallExplodeAnimator.Play("SmallExplode");

        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DestroySelf()
    {
        var animationLength = smallExplodeAnimator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        Destroy(gameObject);
    }
}
