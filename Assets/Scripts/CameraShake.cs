using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator ShakeCamera(float duration)
    {
        Vector3 initialPos = transform.localPosition;
        float timePassed = 0f;

        while (timePassed < duration)
        {
            transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), initialPos.z);

            timePassed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPos;
        
    }
}
