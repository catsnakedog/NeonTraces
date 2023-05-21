using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }

public class FadeEffect : MonoBehaviour
{
    [Range(0f, 3f)]
    public int FadeType; //FadeState ����- 0: FadeIn, 1: FadeOut, 2: FadeInOut, 3: FadeLoop
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime; //fadeSpeed ���� 10�̸� 1�� (���� Ŭ���� ����)
    [SerializeField]
    private float delayTime; //���� ȿ�� ��� �ð�
    [SerializeField]
    private AnimationCurve fadeCurve; //���̵� ȿ���� ����Ǵ� ���� ���� ��� ������ ����

    private Image image;
    private FadeState fadeState; //���̵� ȿ�� ����


    FadeState num;

    private void Awake()
    {
        image = GetComponent<Image>();
        num = (FadeState)FadeType;
        StartCoroutine(WaitTime());
        
    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(delayTime);
        OnFade(num);
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:              //Fade In. ����� ���İ��� 0���� 1���� (��Ÿ����)
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(1, 0));     //Fade Out. ����� ���İ��� 1���� 0�� (�������)
                break;
            case FadeState.FadeInOut:           //Fade ȿ���� In -> Out 1ȸ �ݺ��Ѵ�.
            case FadeState.FadeLoop:            //Fade ȿ���� In -> Out ���� �ݺ��Ѵ�.
                StartCoroutine(FadeInOut());
                break;
        }
    }
    /*
    public void OnFade(FadeState state, float delaytime) //�ռ� GameObject�� alpha ���� ���� fadeȿ�� ����
    {
        delayObject = trigger;
        afterAlpha = alpha;
        fadeState = state;
        if (delayObject.GetComponent<Image>().color.a > afterAlpha)
        {
            OnFade(fadeState);
        }

        delayTime = delaytime;
        timer += Time.deltaTime;
        if (timer > delayTime) //���� �ð��� delayTime���� ������ ���
        {
        }
    }
    */

    private IEnumerator FadeInOut()
    {
        while(true)
        {
            //�ڷ�ƾ ���ο��� �ڷ�ƾ �Լ� ȣ�� �� �ش� �ڷ�ƾ �Լ��� ����Ǿ�� ���� ���� ����
            yield return StartCoroutine(Fade(0.1f, 1));    //Fade In
            yield return StartCoroutine(Fade(1, 0));    //Fade Out

            //1ȸ�� ����ϴ� ������ �� break
            if(fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime  = 0.0f;
        float percent      = 0.0f;

        while(percent < 1)
        {
            // fadeTime ���� ����� fadeTime �ð� ����
            // percent ���� 0���� 1�� ����
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            //���İ��� start���� end���� fadeTime �ð� ���� ��ȭ
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;

        }
    }
}
