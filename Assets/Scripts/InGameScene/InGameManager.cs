using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    MapMake mapMake;
    DataManager Data;
    PlayerMove playerMove;
    PlayerAction playerAction;
    OptimizeEnemy optimizeEnemy;
    GameObject player;
    [SerializeField] Image fadeInOutPanel;
    [SerializeField] GameObject stageAttemptObj;

    int stage;
    // Start is called before the first frame update
    void Start()
    {
        fadeInOutPanel.gameObject.SetActive(true);
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();
        playerMove = gameObject.GetComponent<PlayerMove>();
        playerAction = gameObject.GetComponent<PlayerAction>();
        optimizeEnemy = gameObject.GetComponent<OptimizeEnemy>();
        player = GameObject.Find("Player");
        Data = DataManager.data;
        stage = Data.saveData.gameData.stage;
        Data.saveData.gameData.isInGame = true;
        StartCoroutine(SettingGame());
    }

    IEnumerator SettingGame() // 게임 시작 함수, 맵생성 플레이어 움직임 등등을 한다 / 아마도 맵생성과 움직임은 분리해서 실행할거같다 (수정 필요)
    {
        Resources.UnloadUnusedAssets();
        yield return new WaitForFixedUpdate();
        Data.saveData.gameData.isCameraFollow = false;
        mapMake.isMapEditor = false;
        mapMake.MapSetting();
        optimizeEnemy.OptimizeStart();
        SoundManager.sound.Stop("BG");
        GameObject.Find("MainCamera").transform.position = Data.saveData.mapData[stage].moveDots[0].v3 + new Vector3(14, 5, -10);
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3 + new Vector3(-10, 0, 0);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeIn());
        StartCoroutine(ShowStageAttempt());
        yield return StartCoroutine(playerMove.LeftInMove());
        playerMove.GameStart();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverEffect());
    }

    IEnumerator GameOverEffect()
    {
        DataManager.data.saveData.gameData.stageAttemptCount[DataManager.data.saveData.gameData.stage]++;
        DataManager.data.Save();
        SoundManager.sound.Stop("BG");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("InGameScene");
    }

    public void GameEnd()
    {
        playerAction.PlayerActionReset();
        Resources.UnloadUnusedAssets();
        DataManager.data.Save();
        StartCoroutine(GameEndEffect());
    }

    IEnumerator GameEndEffect()
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        DataManager.data.saveData.gameData.stageAttemptCount[DataManager.data.saveData.gameData.stage] = 0;
        DataManager.data.Save();
        if (DataManager.data.saveData.gameData.stage == 4)
        {
            DataManager.data.saveData.gameData.isFirstGame = false;
            DataManager.data.saveData.gameData.stage = 0;
            DataManager.data.Save();
            SceneManager.LoadScene("InGameScene");
        }
        else if (DataManager.data.saveData.gameData.isFirstCutScene[stage])
        {
            SceneManager.LoadScene("CutScene0" + (stage + 1).ToString());
        }
        else
        {
            SceneManager.LoadScene("UI");
        }
        SoundManager.sound.BGMLoopSet(true);
    }

    public IEnumerator FadeIn()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
        while (fadeInOutPanel.color.a > 0)
        {
            fadeInOutPanel.color -= new Color(0, 0, 0, Time.deltaTime * 1);
            yield return null;
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        fadeInOutPanel.gameObject.SetActive(false);
    }

   public IEnumerator FadeOut()
    {
        fadeInOutPanel.gameObject.SetActive(true);
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        while (fadeInOutPanel.color.a < 1)
        {
            fadeInOutPanel.color += new Color(0, 0, 0, Time.deltaTime * 1);
            yield return null;
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator ShowStageAttempt()
    {
        yield return new WaitForSeconds(0.5f);
        stage = Data.saveData.gameData.stage;
        stageAttemptObj.transform.GetChild(2).GetComponent<Text>().text = $"{Data.saveData.gameData.stageAttemptCount[stage]}회";
        if(stage == 4)
            stageAttemptObj.transform.GetChild(3).GetComponent<Text>().text = $"튜토리얼";
        else
            stageAttemptObj.transform.GetChild(3).GetComponent<Text>().text = $"스테이지 {stage + 1}";
        stageAttemptObj.transform.localPosition = new Vector3(-410, 0, 0);
        float moveAmount = 0;
        while (moveAmount < 410f)
        {
            float move = Time.deltaTime * 650;
            stageAttemptObj.transform.localPosition += new Vector3(move, 0, 0);
            moveAmount += move;
            yield return null;
        }
        stageAttemptObj.transform.localPosition = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(3f);
        StartCoroutine(BlindStageAttempt());
    }

    public IEnumerator BlindStageAttempt()
    {
        stageAttemptObj.transform.localPosition = new Vector3(0, 0, 0);
        float moveAmount = 0;
        while (moveAmount < 410f)
        {
            float move = Time.deltaTime * 650;
            stageAttemptObj.transform.localPosition -= new Vector3(move, 0, 0);
            moveAmount += move;
            yield return null;
        }
        stageAttemptObj.transform.localPosition = new Vector3(-410, 0, 0);
    }
}