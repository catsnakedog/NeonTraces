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


}
// https://www.youtube.com/watch?v=99bgQ5WG6ok 참고자료,사용법