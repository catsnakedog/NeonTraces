using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{
    private void Awake()
    {

        SetTalkDictionary();
        SetDebugTalkData(); //인스펙터 창에서 대화데이터 보이도록하기
    }


    private static Dictionary<string, TalkData[]> DialogueDictionary = // Key: string, Value: TalkData[]
                 new Dictionary<string, TalkData[]>();
    [SerializeField] private TextAsset csvFile = null;

    public void SetTalkDictionary()
    {
        // 아래 한 줄 빼기
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1);
        // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음
        string[] rows = csvText.Split(new char[] { '\n' }); //row[0]: 컷씬번호+파트, row[1]: 이름, row[2]: 대사 row[3]: 속도 row[4]: 폰트 크기


        // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        for (int i = 1; i < rows.Length; i++)
        {
            // A, B, C, D, E열을 쪼개서 배열에 담음
            string[] rowValues = rows[i].Split(new char[] { ',' }); // rowValues : ["컷씬번호+파트", "이름", "대사", "속도"]

            // 유효한 이벤트 이름이 나올때까지 반복
            if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end") continue; //첫번째 row가 빈칸이나 end 일때 무시

            List<TalkData> talkDataList = new List<TalkData>(); //딕셔너리 벨류에 넣을 TalkData의 List ********
            string eventPart = rowValues[0];

            while (rowValues[0].Trim() != "end") // talkDataList 하나를 만드는 반복문
            {
                // 캐릭터가 한번에 치는 대사의 길이를 모르므로 리스트로 선언
                List<string> contextList = new List<string>();

                TalkData talkData;
                talkData.name = rowValues[1]; // 캐릭터 이름이 있는 B열
                talkData.speed = rowValues[3]; // 대사 속도가 있는 D열
                

                do // talkData 하나를 만드는 반복문
                {
                    contextList.Add(rowValues[2].ToString()); //대사 --> contextList로
                    if (++i < rows.Length) rowValues = rows[i].Split(new char[] { ',' });
                    else break;
                } while (rowValues[1] == "" && rowValues[0] != "end"); //동일한 캐릭터가 대사 && end가 아닐때

                talkData.contexts = contextList.ToArray(); // contextList에 있는 대사들을 배열로 변경한 뒤, talkData의 context로
                talkDataList.Add(talkData);
            }

            DialogueDictionary.Add(eventPart, talkDataList.ToArray());
        }
    }

        public static TalkData[] GetDialogue(string eventPart)
    {
        // 키에 매칭되는 값이 있으면 true 없으면 false
        if (DialogueDictionary.ContainsKey(eventPart))
        {
            return DialogueDictionary[eventPart];
        }
        else
        {
            // 경고 출력하고 null 반환
            Debug.LogWarning("찾을 수 없는 이벤트 이름 : " + eventPart);
            return null;
        }
    }


    [System.Serializable]
    public class DebugTalkData
    {
        public string eventPart;
        public TalkData[] talkDatas;

        public DebugTalkData(string part, TalkData[] td)
        {
            eventPart = part;
            talkDatas = td;
        }
    }

    [SerializeField]
    List<DebugTalkData> DebugTalkDataList =
                        new List<DebugTalkData>();

    void SetDebugTalkData()
    {
        // 딕셔너리의 키 값들을 가진 리스트
        List<string> eventNames =
                    new List<string>(DialogueDictionary.Keys);
        // 딕셔너리의 밸류 값들을 가진 리스트
        List<TalkData[]> talkDatasList =
                    new List<TalkData[]>(DialogueDictionary.Values);

        // 딕셔너리의 크기만큼 추가
        for (int i = 0; i < eventNames.Count; i++)
        {
            DebugTalkData debugTalk =
                new DebugTalkData(eventNames[i], talkDatasList[i]);

            DebugTalkDataList.Add(debugTalk);
        }
    }


}