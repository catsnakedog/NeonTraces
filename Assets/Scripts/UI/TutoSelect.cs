using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoSelect : MonoBehaviour
{

    DataManager Data;

    Button stageButton;
    Button NextStageButton;
    public int stageNumber; //스테이지 번호 0번부터
    public GameObject cutSceneButton;
    public bool isClear;
    public bool isActive;
    public Sprite Open;

    void Start()
    {
        Data = DataManager.data; //static data
        stageButton = this.transform.GetComponent<Button>();
        NextStageButton = this.transform.parent.GetChild(2).GetComponent<Button>();
        isActive = true;
        //print(stageButton +"'s next stage is "+ NextStageButton);

        isClear = Data.saveData.gameData.stageClearInfo[stageNumber];
    }
    private void FixedUpdate()
    {
        if (!isActive)
            return;

        if (isClear == true) //해당 스테이지 클리어 완료 시
        {
            cutSceneButton.SetActive(true); //컷씬 버튼 활성화
            NextStageButton.interactable = true; //다음 스테이지 클릭 활성화
            NextStageButton.GetComponent<Image>().sprite = Open;
            isActive = false;
        }
        else //스테이지 클리어 X
        {
            NextStageButton.interactable = false; //다음 스테이지 클릭 비활성화
            cutSceneButton.SetActive(false);
        }
    }
}
