using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;

    public bool shakeRotate = false;

    private Vector3 originPos;

    private Quaternion originRot;
    // Start is called before the first frame update
    void Start()
    {
        shakeCamera = GameObject.Find("MainCamera").transform;
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
    }

    public IEnumerator Shakecamera(float duration = 0.05f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        float passTime = 0.0f;
        while (passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.localPosition = shakePos * magnitudePos;
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }

            passTime += Time.deltaTime;
            yield return null;
        }

        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }
}
// https://www.youtube.com/watch?v=99bgQ5WG6ok 참고자료,사용법