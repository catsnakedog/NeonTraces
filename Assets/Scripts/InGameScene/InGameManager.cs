using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    MapMake mapMake;
    DataManager Data;
    PlayerMove playerMove;
    OptimizeEnemy optimizeEnemy;
    GameObject player;
    [SerializeField] Image fadeInOutPanel;
    int stage;
    // Start is called before the first frame update
    void Start()
    {
        fadeInOutPanel.gameObject.SetActive(true);
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();
        playerMove = gameObject.GetComponent<PlayerMove>();
        optimizeEnemy = gameObject.GetComponent<OptimizeEnemy>();
        player = GameObject.Find("Player");
        Data = DataManager.data;
        stage = Data.saveData.gameData.stage;
        Data.saveData.gameData.isInGame = true;
        StartCoroutine(SettingGame());
    }

    IEnumerator SettingGame() // 게임 시작 함수, 맵생성 플레이어 움직임 등등을 한다 / 아마도 맵생성과 움직임은 분리해서 실행할거같다 (수정 필요)
    {
        yield return new WaitForFixedUpdate();
        Data.saveData.gameData.isCameraFollow = false;
        mapMake.isMapEditor = false;
        mapMake.MapSetting();
        optimizeEnemy.OptimizeStart();
        GameObject.Find("MainCamera").transform.position = Data.saveData.mapData[stage].moveDots[0].v3 + new Vector3(14, 5, -10);
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3 + new Vector3(-10, 0, 0);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(playerMove.LeftInMove());
        playerMove.GameStart();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverEffect());
    }

    IEnumerator GameOverEffect()
    {
        SoundManager.sound.Stop("BG");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("InGameScene");
    }

    public void GameEnd()
    {
        DataManager.data.Save();
        StartCoroutine(GameEndEffect());
    }

    IEnumerator GameEndEffect()
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        if (DataManager.data.saveData.gameData.isFirstCutScene[stage])
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
}