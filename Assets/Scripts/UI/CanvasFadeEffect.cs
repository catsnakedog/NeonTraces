using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeTime;


    private RectTransform targetRT;
    private RectTransform LogoRT;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        targetRT = GameObject.Find("Target").GetComponent<RectTransform>();
        LogoRT = GameObject.Find("GameLogo").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LogoRT.localPosition == targetRT.localPosition) //로고 제자리 시
        {
            StartCoroutine("FadeOut");
            if (canvasGroup.alpha < 0.1f)
            {
                StopCoroutine("FadeOut");
                gameObject.SetActive(false);
            }
        }
    }
    public IEnumerator FadeOut()
    {
        //yield return new WaitForSeconds(3.0f);
        float accumTime = 0f;
        while (accumTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime ;
        }
        canvasGroup.alpha = 0f;
    }
}
