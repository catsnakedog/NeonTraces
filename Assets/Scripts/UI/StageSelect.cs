using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    
    DataManager Data;

    Button stageButton;
    Button NextStageButton;
    public int stageNumber; //스테이지 번호 0번부터
    public GameObject cutSceneButton;
    public bool isClear;

    void Start()
    {
        Data = DataManager.data; //static data
        stageButton = this.transform.GetComponent<Button>();
        NextStageButton = this.transform.parent.GetChild(stageNumber + 2).GetComponent<Button>();
        //print(stageButton +"'s next stage is "+ NextStageButton);

        isClear = Data.saveData.gameData.stageClearInfo[stageNumber];

    }
    private void FixedUpdate()
    {
        if (isClear == true) //해당 스테이지 클리어 완료 시
        {
            cutSceneButton.SetActive(true); //컷씬 버튼 활성화

            if (NextStageButton.ToString() != "end" /* && 컷씬 시청 완료시?*/) //마지막 스테이지 아니라면
            {
                NextStageButton.interactable = true; //다음 스테이지 클릭 활성화
            }
        }
        else //스테이지 클리어 X
        {
            NextStageButton.interactable = false; //다음 스테이지 클릭 비활성화
            cutSceneButton.SetActive(false); //컷씬 버튼 비활성화
        }
    }
}
