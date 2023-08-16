
using UnityEngine;

[System.Serializable] // 구조체가 인스펙터 창에 보이게 하기 위한 작업
public struct TalkData //캐릭터 한명의 대사
{
    public string name; // 대사 치는 캐릭터 이름
    public string[] contexts; // 대사 내용
    public string speed; // 대사 타이핑 속도
    //public int fontSize; // 폰트 크기
}

public class Dialogue : MonoBehaviour
{
    // 대화 이벤트 이름
    [Tooltip("컷씬번호+대화번호\n(ex) 컷씬0의 첫번째 대화: 01")]
    [SerializeField] string eventPart = null; // 컷씬번호 + 파트 번호 (ex) 01: 컷씬0 파트1

    // 위에서 선언한 TalkData 배열 
    [SerializeField] TalkData[] talkDatas; // index 0번: 주인공 / 1번: 드론 / 2번: 스피커 / ...

    public TalkData[] GetObjectDialogue()
    {
        return DialogueParse.GetDialogue(eventPart);
    }
}