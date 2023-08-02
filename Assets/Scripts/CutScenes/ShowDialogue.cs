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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReceiveSignal()
    {
        Debug.Log("대화 시작");
        playableDirector.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // 특정 시간 되면 일시정지 후 대화 시작
        {
            playableDirector.Pause();

        }
        if (Input.GetKeyDown(KeyCode.P)) // 대화 종료 후 이어서 하기
        {
            playableDirector.Resume();
        }
    }
}
