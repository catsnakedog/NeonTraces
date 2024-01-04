using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodRandom : MonoBehaviour
{
    [SerializeField] Sprite[] Sprite;
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite[Random.Range(0, Sprite.Length)];
    }
}
