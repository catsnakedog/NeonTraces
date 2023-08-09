using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ShowDialogue : MonoBehaviour
{
    // scene 내 빈 오브젝트에 스크립트

    public PlayableDirector playableDirector;
    public Text Text;
    
    public void ReceiveSignal()
    {
        Debug.Log("대화 시작");
        playableDirector.Pause();
        //Debug.Log(Text.transform.parent.parent.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)) // 특정 시간 되면 일시정지 후 대화 시작
        {

            Text.transform.parent.parent.gameObject.SetActive(true); //해당 TextBox 활성화
            //Text.text = "시작";

            TalkData[] talkDatas = transform.GetComponent<Dialogue>().GetObjectDialogue();
            // 대사가 null이 아니면 대사 출력
            if (talkDatas != null) DebugDialogue(talkDatas); ;


        }
        if (Input.GetKeyDown(KeyCode.P)) // 대화 종료 후 이어서 하기
        {
            playableDirector.Resume();
        }
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
