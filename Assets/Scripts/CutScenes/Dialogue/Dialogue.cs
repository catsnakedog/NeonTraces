using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 구조체가 인스펙터 창에 보이게 하기 위한 작업
public struct TalkData
{
    public string name; // 대사 치는 캐릭터 이름
    public string[] contexts; // 대사 내용
    public string speed; // 대사 타이핑 속도
    //public int fontSize; // 폰트 크기
}

public class Dialogue : MonoBehaviour
{
    // 대화 이벤트 이름
    [SerializeField] string eventPart = null; // 컷씬번호 + 파트 번호 (ex) 01: 컷씬0 파트1

    // 위에서 선언한 TalkData 배열 
    [SerializeField] TalkData[] talkDatas = null; // index 0번: 주인공 / 1번: 드론 / 2번: 스피커 / ...

    public TalkData[] GetObjectDialogue()
    {
        return DialogueParse.GetDialogue(eventPart);
    }
}