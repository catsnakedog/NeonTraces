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
        afterImageColor.a = 0.5f;
    }
    public IEnumerator AfterImageSetting(GameObject target)
    {
        GameObject temp = defaultObject;
        while (true)
        {
            temp.GetComponent<SpriteRenderer>().sprite = target.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(ImageSet(Instantiate(temp, target.transform.position, target.transform.localRotation), target));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ImageSet(GameObject target, GameObject parent)
    {
        target.GetComponent<SpriteRenderer>().color = afterImageColor;
        target.transform.localScale = parent.transform.localScale;
        yield return new WaitForSeconds(0.4f);
        Destroy(target);
    }
}
