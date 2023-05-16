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
    private AnimationCurve fadeCurve; //���̵� ȿ���� ����Ǵ� ���� ���� ��� ������ ����
    private Image image;
    private FadeState fadeState; //���̵� ȿ�� ����

    private void Awake()
    {
        image = GetComponent<Image>();
        FadeState num = (FadeState)FadeType;
        OnFade(num);
    }
    
    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:              //Fade In. ����� ���İ��� 1���� 0���� (�������)
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));     //Fade Out. ����� ���İ��� 0���� 1�� (���� ��Ÿ����)
                break;
            case FadeState.FadeInOut:           //Fade ȿ���� In -> Out 1ȸ �ݺ��Ѵ�.
            case FadeState.FadeLoop:            //Fade ȿ���� In -> Out ���� �ݺ��Ѵ�.
                StartCoroutine(FadeInOut());
                break;
        }
    }

    private IEnumerator FadeInOut()
    {
        while(true)
        {
            //�ڷ�ƾ ���ο��� �ڷ�ƾ �Լ� ȣ�� �� �ش� �ڷ�ƾ �Լ��� ����Ǿ�� ���� ���� ����
            yield return StartCoroutine(Fade(1, 0));    //Fade In
            yield return StartCoroutine(Fade(0, 1));    //Fade Out

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
