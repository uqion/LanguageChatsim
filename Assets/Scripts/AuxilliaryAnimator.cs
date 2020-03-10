using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AuxilliaryAnimator : MonoBehaviour
{
    public void CallAnimation(PlayableDirector director, float offset)
    {
        StartCoroutine(Animate(director, offset));
    }

    public IEnumerator Animate(PlayableDirector director, float offset)
    {
        yield return new WaitForSeconds(offset);
        director.Play();
    }
}
