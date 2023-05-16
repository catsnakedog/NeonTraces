using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }

public class FadeEffect : MonoBehaviour
{

    [Range(0f, 3f)]
    public int FadeType; //FadeState 결정- 0: FadeIn, 1: FadeOut, 2: FadeInOut, 3: FadeLoop
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime; //fadeSpeed 값이 10이면 1초 (값이 클수록 빠름)
    [SerializeField]
    private AnimationCurve fadeCurve; //페이드 효과가 적용되는 알파 값을 곡선의 값으로 설정
    private Image image;
    private FadeState fadeState; //페이드 효과 상태

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
            case FadeState.FadeIn:              //Fade In. 배경의 알파값이 1에서 0으로 (사라지게)
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));     //Fade Out. 배경의 알파값이 0에서 1로 (점점 나타나게)
                break;
            case FadeState.FadeInOut:           //Fade 효과를 In -> Out 1회 반복한다.
            case FadeState.FadeLoop:            //Fade 효과를 In -> Out 무한 반복한다.
                StartCoroutine(FadeInOut());
                break;
        }
    }

    private IEnumerator FadeInOut()
    {
        while(true)
        {
            //코루틴 내부에서 코루틴 함수 호출 시 해당 코루틴 함수가 종료되어야 다음 문장 실행
            yield return StartCoroutine(Fade(1, 0));    //Fade In
            yield return StartCoroutine(Fade(0, 1));    //Fade Out

            //1회만 재생하는 상태일 때 break
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
            // fadeTime 으로 나누어서 fadeTime 시간 동안
            // percent 값이 0에서 1로 증가
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            //알파값을 start부터 end까지 fadeTime 시간 동안 변화
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;

        }
    }
}
