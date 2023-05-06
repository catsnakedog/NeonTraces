using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    GameObject stage;
    //GameObject cutScene;
 
    DataManager Data;
    GameData gamedata;

    Image image;
    public int stageNum; //스테이지 번호 0번부터
    
    // Start is called before the first frame update
    void Start()
    {
        image = this.transform.GetComponent<Image>();
        //cutScene = transform.FindChild("cutSceneButton");
        
        Data.Load();
        gamedata = Data.saveData.gameData;
        if (gamedata.stageClearInfo[stageNum] == true) //클리어 완료 시
        {
            //gameObject.SetActive(true); //컷씬 버튼 활성화
        }
        else
        {
            image.color = Color.gray;
            //gameObject.SetActive(false); //컷씬 버튼 비활성화
        }
    }

}
