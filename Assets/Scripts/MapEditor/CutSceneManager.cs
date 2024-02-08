using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] Image fadeInOutPanel;
    [SerializeField] GameObject CutSceneObj;
    [SerializeField] GameObject[] TutoObj;

    [Header("CutScene0")]
    [SerializeField] GameObject Elevator;

    public void StartCutScene(int num)
    {
        StartCoroutine("CutScene" + num);
    }

    // 각각의 컷씬들의 동작들을 저장한다
    IEnumerator CutScene0()
    {
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FadeOut());
        Instantiate(Elevator, CutSceneObj.transform);
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator CutScene1()
    {
        yield return StartCoroutine(FadeOut());
        foreach (Transform child in CutSceneObj.transform.GetComponentsInChildren<Transform>())
        if(child.name != CutSceneObj.name)
                Destroy(child.gameObject);
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator CutScene2()
    {
        Time.timeScale = 0f;
        yield return null;
        Instantiate(TutoObj[0]);
    }
    IEnumerator CutScene3()
    {
        Time.timeScale = 0f;
        yield return null;
        Instantiate(TutoObj[1]);
    }
    IEnumerator CutScene4()
    {
        Time.timeScale = 0f;
        yield return null;
        Instantiate(TutoObj[2]);
    }
    IEnumerator CutScene5()
    {
        Time.timeScale = 0f;
        yield return null;
        Instantiate(TutoObj[3]);
    }

    public IEnumerator FadeIn()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
        while (fadeInOutPanel.color.a > 0)
        {
            fadeInOutPanel.color -= new Color(0, 0, 0, Time.deltaTime * 1);
            yield return null;
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        fadeInOutPanel.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        fadeInOutPanel.gameObject.SetActive(true);
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        while (fadeInOutPanel.color.a < 1)
        {
            fadeInOutPanel.color += new Color(0, 0, 0, Time.deltaTime * 1);
            yield return null;
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
    }
}
