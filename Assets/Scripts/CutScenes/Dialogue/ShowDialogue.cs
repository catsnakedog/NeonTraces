using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ShowDialogue : MonoBehaviour
{
    // scene 내 빈 오브젝트에 스크립트

    public PlayableDirector playableDirector;
    [Tooltip("0번은 주인공, 1번은 드론, 2번은 스피커, 3번은 AI\nText오브젝트")]
    public Text[] text;

    int talkDatasIndex = 0;
    int count = 0; // 배열 인덱스 숫자
    TalkData[] talkDatas;

    private int index = 0;
    private float typingSpeed;
    private string textString;
    private bool CoroutineEnd = true;


    GameObject tempObject;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ReceiveSignal()
    {
        Debug.Log("대화 시작");
        playableDirector.Pause();

        talkDatas = transform.GetComponent<Dialogue>().GetObjectDialogue();
        if (talkDatas != null) DebugDialogue(talkDatas); // 대사가 null이 아니면 대사 출력
        //Debug.Log(Text.transform.parent.parent.name);
    }
    public void ReceiveSignal_Play()
    {
        Debug.Log("대화 시작");

        talkDatas = transform.GetComponent<Dialogue>().GetObjectDialogue();
        if (talkDatas != null) DebugDialogue(talkDatas); // 대사가 null이 아니면 대사 출력
        //Debug.Log(Text.transform.parent.parent.name);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !CoroutineEnd)
        {
            //StopCoroutine(StartTyping());
            StopCoroutine(Typing());
            CoroutineEnd = true;
            text[index].text = null;
            text[index].text = textString;
            return;
        }
        if (Input.GetMouseButtonDown(0) && CoroutineEnd && AnimationInDialouge.AnimEnd) // 특정 시간 되면 일시정지 후 대화 시작
        {

            Debug.Log("클릭");
            InitDialogue(); //인덱스 초기화 및 유효성 검사

            if (talkDatasIndex >= talkDatas.Length) // 대화가 끝났다면
            {
                gameObject.SetActive(false); // 대화창이 나오는 Canvas 비활성화
                playableDirector.Resume(); //해당 대화가 종료되면 타임라인 재시작
            }

            //대화 중이라면 대사 띄우기
            else if (talkDatas[talkDatasIndex].name == "주인공") //주인공의 대사라면
            {
                Talk("주인공");
            }
            else if (talkDatas[talkDatasIndex].name == "드론") //드론의 대사라면
            {
                Talk("드론");
            }
            else if (talkDatas[talkDatasIndex].name == "스피커") //스피커의 대사라면
            {
                Talk("스피커");
            }
            else if (talkDatas[talkDatasIndex].name == "AI") //스피커의 대사라면
            {
                Talk("AI");
            }

            else //모두 아니라면
            {
                return;
            }
        }

    }

    void InitDialogue() //인덱스 초기화 및 유효성 검사
    {
        if (talkDatasIndex < talkDatas.Length && count >= talkDatas[talkDatasIndex].contexts.Length) // 대화 중이며 && 다른 캐릭터의 대사로 넘어간 것이라면
        {
            talkDatasIndex++;
            count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if(text[i] != null)
                    text[i].transform.parent.parent.gameObject.SetActive(false);
            }
        }

    }

    void Talk(string name)
    {
        
        switch(name)
        {
            case "주인공":
                index = 0;
                break;
            case "드론":
                index = 1;
                break;
            case "스피커":
                index = 2;
                break;
            case "AI":
                index = 3;
                break;
        }
        Debug.Log(name);
        Debug.Log(name + " count: " + count);
        Debug.Log(name + " talkDatasIndex: " + talkDatasIndex);
        

        text[index].transform.parent.parent.gameObject.SetActive(true);
        //text[index].text = talkDatas[talkDatasIndex].contexts[count].Replace("\\n", "\n");

        typingSpeed = float.Parse(talkDatas[talkDatasIndex].speed);
        textString = talkDatas[talkDatasIndex].contexts[count].Replace("\\n", "\n");
        textString = textString.Replace("`", ",");

        //StartCoroutine(StartTyping());
        StartCoroutine(Typing());

        count++;
    }

    IEnumerator StartTyping()
    {

        yield return StartCoroutine("Typing");
    }

    IEnumerator Typing()
    {
        CoroutineEnd = false;
        text[index].text = null;

        //폰트 사이즈
        if (talkDatas[talkDatasIndex].fontSize == "")
            text[index].fontSize = 60;
        else
            text[index].fontSize = int.Parse(talkDatas[talkDatasIndex].fontSize);

        //폰트 스타일
        if (talkDatas[talkDatasIndex].fontStyle == "")
            text[index].fontStyle = FontStyle.Normal;
        else if (talkDatas[talkDatasIndex].fontStyle == "b")
            text[index].fontStyle = FontStyle.Bold;
        else if (talkDatas[talkDatasIndex].fontStyle == "i")
            text[index].fontStyle = FontStyle.Italic;
        //타이핑

        for (int i = 0; i < textString.Length && !CoroutineEnd; i++)
        {
            text[index].text += textString[i];
            //속도
            yield return new WaitForSeconds(typingSpeed);

        }
        CoroutineEnd = true;
        //int i = 0;
        //do
        //{
        //    text[index].text += textString[i];
        //    //속도
        //    yield return new WaitForSeconds(typingSpeed);
        //    i++;
        //} while (i < textString.Length);


    }

    // 대화 정보 출력하는 함수
    void DebugDialogue(TalkData[] talkDatas)
    {
        for (int i = 0; i < talkDatas.Length; i++)
        {
            // 캐릭터 이름 출력
            Debug.Log(talkDatas[i].name);
            // 대사들 출력
            foreach (string context in talkDatas[i].contexts)
                Debug.Log(context);
        }
    }
}
