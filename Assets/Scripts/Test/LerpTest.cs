using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    float timeElapsed;
    float lerpDuration = 3;

    Vector3 startPosition;
    Vector3 endPosition;

    void Start(){
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z + 5);
    }

    void Update()
    {
        Lerp();
    }

    void Lerp()
    {
        if(transform.position == endPosition)
            return;
        float t = timeElapsed / lerpDuration;
        t = t * t * (3f - 2f * t);

        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        timeElapsed += Time.deltaTime;

        if(transform.position == endPosition)
            transform.position = endPosition;
    }

    /*public Vector3 positionToMoveTo;

    void Start()
    {
        StartCoroutine(LerpPosition(positionToMoveTo, 1));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }*/
}
