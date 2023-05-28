using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeTime;


    private RectTransform targetRT;
    private RectTransform LogoRT;
    private void Awake()
    {
        gameObject.SetActive(true);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        targetRT = GameObject.Find("Target").GetComponent<RectTransform>();
        LogoRT = GameObject.Find("GameLogo").GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (LogoRT.localPosition == targetRT.localPosition) //�ΰ� ���ڸ� ��
        {
            FadeoutSkip();
        }
    }

    public void FadeoutSkip()
    {
        StartCoroutine("FadeOut");
        if (canvasGroup.alpha < 0.2f)
        {
            StopCoroutine("FadeOut");
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
        gameObject.SetActive(false);
    }
}
