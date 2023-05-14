using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public Color afterImageColor;
    DataManager Data;

    public GameObject defaultObject;

    void Start()
    {
        Data = DataManager.data;
        afterImageColor = Color.blue;
        afterImageColor.a = 0.3f;
    }
    public IEnumerator AfterImageSetting(GameObject target)
    {
        GameObject temp = defaultObject;
        temp.GetComponent<SpriteRenderer>().sprite = target.GetComponent<SpriteRenderer>().sprite;
        while (true)
        {
            StartCoroutine(ImageSet(Instantiate(temp, target.transform.position, target.transform.localRotation), target));
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator ImageSet(GameObject target, GameObject parent)
    {
        target.GetComponent<SpriteRenderer>().color = afterImageColor;
        yield return new WaitForSeconds(0.3f);
        Destroy(target);
    }
}
