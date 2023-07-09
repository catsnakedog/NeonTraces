using UnityEngine;
using System.Collections;

public class CutScene1Cam : MonoBehaviour {
    public GameObject A;  
    Transform AT;
    void Start ()
    {
        AT=A.transform;
    }
    void LateUpdate () {
        transform.position = new Vector3 (AT.position.x,AT.position.y,transform.position.z);
    }
}