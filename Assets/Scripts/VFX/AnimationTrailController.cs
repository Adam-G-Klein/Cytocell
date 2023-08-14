using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrailController : MonoBehaviour
{
    public void destroyAfterTween(){
        StartCoroutine("destroyAfterTweenCorout");
    }

    private IEnumerator destroyAfterTweenCorout(){
        yield return new WaitUntil(() => !LeanTween.isTweening(gameObject));
        Destroy(gameObject);
    }

}
