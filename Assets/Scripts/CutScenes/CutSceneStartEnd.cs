using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneStartEnd : MonoBehaviour
{
    /* 0. Playable Director에 부착 */
    /* 2. Timeline 끝에 CutSceneEND Emitter */
    /* 2-1. CutSceneEnd.endScene*/
    SoundManager soundmanager;
    AudioListener _listener;
    public GameObject[] MainObject;

    private void Awake()
    {
        soundmanager = SoundManager.sound;
        _listener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
        MainObject = GameObject.FindGameObjectsWithTag("Main"); // UI씬의 Light와 EventSystem


        Debug.Log(_listener);
        Debug.Log(MainObject);

        for (int i = 0; i < MainObject.Length; i++) // UI씬의 Light와 EventSystem 비활성화
        {
            MainObject[i].SetActive(false);
        }
        GetCurrentAudioListener(); //오디오리스너 유일성 보장
    }

    private void Start() // 컷씬 시작 시
    {

        SceneManager.SetActiveScene(gameObject.scene);
    }

    public void endScene() // 컷씬 종료 시 // InGame -> 컷씬 , UI -> 컷씬 분리가 필요해서 따로 제작함
    {
        if(DataManager.data.saveData.gameData.crruentScene == "UI") // UI에서 호출한 경우
        {
            for (int i = 0; i < MainObject.Length; i++) // UI씬의 Light와 EventSystem 활성화
            {
                MainObject[i].SetActive(true);
            }
            SceneManager.UnloadSceneAsync(gameObject.scene);
            soundmanager.Play("mainscreen");
        }
        else if(DataManager.data.saveData.gameData.crruentScene == "InGame")
        {
            DataManager.data.saveData.gameData.isFirstCutScene[DataManager.data.saveData.gameData.stage] = false;
            DataManager.data.Save();
            SceneManager.LoadScene("UI");
        }
    }


    public void GetCurrentAudioListener()
    {
        AudioListener[] listeners = GameObject.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];

        if (listeners != null)
        {
            if (!_listener.enabled) //있는데 꺼져있으면
                _listener.enabled = !_listener.enabled;

            int i;
            for (i = 0; i < listeners.Length; ++i)
            {


                if (listeners[i] && listeners[i].enabled && listeners[i].gameObject.activeInHierarchy)
                {
                    Debug.Log(listeners[i]);
                    _listener = listeners[i];

                    break;
                }
            }
            //나머지 오디오리스너 off
            for (int n = i + 1; n < listeners.Length; ++n)
                if (listeners[n] && listeners[n].enabled && listeners[n].gameObject.activeInHierarchy)
                    listeners[n].enabled = !listeners[n].enabled;
        }

        if (_listener == null) //오디오리스너 없거나 꺼져있을때
        {
            Camera cam = Camera.main;
            if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
            if (cam != null) _listener = cam.gameObject.AddComponent<AudioListener>();
        }


    }
}
