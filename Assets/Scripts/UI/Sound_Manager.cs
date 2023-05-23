using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.CodeDom.Compiler;
using System;

public class Sound_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Sound_Manager sound;
    DataManager Data;
    public AudioMixer master;
    public Slider Bgm, Sfx;

    AudioClip[] BGMs = new AudioClip[(int)BGM.MaxCount];
    AudioClip[] SFXs = new AudioClip[(int)SFX.MaxCount];

    HashSet<string> SFXNames = new HashSet<string>();
    HashSet<string> BGMNames = new HashSet<string>();

    AudioSource BGMSource;
    AudioSource SFXSource;

    enum BGM
    {
        BGM_01,
        Start_BGM,
        MaxCount// 삭제 금지
    }

    enum SFX
    {
        Test_ClickSFX,
       
        MaxCount //삭제금지
    }

    void Awake() // 사운드 싱글톤
    {
        if (sound == null)
        {
            sound = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Data = DataManager.data;
        master = Resources.Load<AudioMixer>("Sound/Master");
        Find_Sliders();
        Load_Value();
        SoundPooling(); // 사운드 파일들을 풀링 해온다
        SetAudioSource(); // 오디오 소스 세팅
        Play("Start_BGM"); // 테스트용 BGM
        //Play("Clear"); // 테스트용 SFX
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Find_Sliders()
    {
        if (Bgm == null || Sfx == null) 
        {
            //GameObject Canvas = GameObject.Find("SettingCanvas");
            /*GameObject Setting = null;
            for (int i = 0; i < Canvas.transform.childCount; i++) // StartCanvas의 자식의 SettingPanel 탐색
            {
                if (Canvas.transform.GetChild(i).name == "SettingPanel")
                {
                    Setting = Canvas.transform.GetChild(i).gameObject;
                    break;
                }
            }
            */
            GameObject Setting = GameObject.Find("SettingPanel");
            if (Setting == null)
            {
                Debug.Log("Setting패널이 존재하지 않음");
            }
            else
            {
                for (int i = 0; i < Setting.transform.childCount; i++)// SettingPanel 자식의 Slider 탐색
                {
                    if (Setting.transform.GetChild(i).name == "SFX_Slider")
                        Sfx = Setting.transform.GetChild(i).GetComponent<Slider>();
                    else if (Setting.transform.GetChild(i).name == "BGM_Slider")
                        Bgm = Setting.transform.GetChild(i).GetComponent<Slider>();
                    if (Bgm != null && Sfx != null)
                        break;
                }
            }
        }
    }
    public void Sound_Control()
    {
        master.SetFloat("BGM", Bgm.value);
        master.SetFloat("SFX", Sfx.value);
        if (Bgm.value == -40)
        {
            master.SetFloat("BGM", -80);
        }
        if (Sfx.value == -40)
        {
            master.SetFloat("SFX", -80);
        }

    }
    public void Load_Value()
    {
        master.SetFloat("BGM", Data.saveData.ui.bgm);
        master.SetFloat("SFX", Data.saveData.ui.sfx);
        if (Bgm != null && Sfx != null)
        {
            Bgm.value = Data.saveData.ui.bgm;
            Sfx.value = Data.saveData.ui.sfx;
        }
    }
    public void Save_Value()
    {
        if (Bgm != null && Sfx != null)
        {
            Data.saveData.ui.bgm = Bgm.value;
            Data.saveData.ui.sfx = Sfx.value;
        }
    }

    void SoundPooling() // enum에서 사운드 이름을 읽어와서 해당하는 사운드 파일을 로드 시킨다
    {
        string[] BGMNames = System.Enum.GetNames(typeof(BGM));
        string[] SFXNames = System.Enum.GetNames(typeof(SFX));
        for (int i = 0; i < BGMNames.Length - 1; i++)
        {
            BGMs[i] = Resources.Load<AudioClip>("Sound/BGM/" + BGMNames[i]);
            this.BGMNames.Add(BGMNames[i]);
        }
        for (int i = 0; i < SFXNames.Length - 1; i++)
        {
            SFXs[i] = Resources.Load<AudioClip>("Sound/SFX/" + SFXNames[i]);
            this.SFXNames.Add(SFXNames[i]);
        }
    }

    void SetAudioSource() // 오디오소스를 할당한다
    {
        AudioSource[] temp;
        temp = gameObject.GetComponents<AudioSource>();
        if (temp.Length >= 2) // check that array has at least two elements
        {
            BGMSource = temp[0];
            SFXSource = temp[1];
            BGMSource.loop = true;
        }
        else
        {
            Debug.LogError("Not enough audio sources on game object!"); // output an error message
        }
    }

    public void Play(string soundName, float pitch = 1.0f) // 전달받은 soundName을 찾아서 실행시킨다
    {
        if (BGMNames.Contains(soundName))
        {
            if (BGMSource.isPlaying)
            {
                BGMSource.Stop();
            }
            BGMSource.pitch = pitch;
            BGMSource.clip = BGMs[(int)((BGM)Enum.Parse(typeof(BGM), soundName))];
            BGMSource.Play();
        }
        else if (SFXNames.Contains(soundName))
        {
            SFXSource.pitch = pitch;
            SFXSource.PlayOneShot(SFXs[(int)((SFX)Enum.Parse(typeof(SFX), soundName))]);
        }
        else
        {
            Debug.Log("해당 사운드는 존재하지 않습니다");
        }
    }
}