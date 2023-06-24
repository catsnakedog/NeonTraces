using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.CodeDom.Compiler;
using System;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager sound;

    AudioClip[] BGMs = new AudioClip[(int)BGM.MaxCount];
    AudioClip[] SFXs = new AudioClip[(int)SFX.MaxCount];

    HashSet<string> SFXNames = new HashSet<string>();
    HashSet<string> BGMNames = new HashSet<string>();

    AudioSource BGMSource;
    AudioSource SFXSource;

    public Slider BGMSlider;
    public Slider SFXSlider;

    float BGMVolume = 0.5f;
    float SFXVolume = 0.5f;

    public GameObject optionCanvas; //버튼에서 사용

    // 6월22일 수정
    enum BGM
    {
        BGM_01,
        Test_BGM,
        MaxCount //삭제 금지
    }

    enum SFX
    {
        Test_ClickSFX,
        // UI - 6개
        Door_exit,
        Door_play,
        Execute,
        NeonLogo,
        Select,
        Setting,

        // 컷씬/드론 - 5개
        Drone_applaud,
        Drone_explode,
        Drone_joke,
        Drone_joke2,
        Drone_scan,
        // 컷씬/차량 - 4개
        Car_break,
        Car_break_crash,
        Car_crash,
        Car_passby,
        // 컷씬 - 1개
        Talk,

        // 전투/메인 - 12개
        Main_attack1,
        Main_attack2,
        Main_attack3,
        Main_drag,
        Main_footstep,
        Main_hit,
        Main_jump,
        Main_landing,
        Main_parry,
        Main_parry_hammer1,
        Main_parry_hammer2,
        Main_parry_hammer3,
        // 전투 - 11개
        A1_attack,
        B2_shield,
        C1_attack1,
        C1_attack2,
        C1_move,
        C1_ready,
        C2_attack,
        C2_bolt,
        C2_case,
        Main_parry1,
        Main_parry3,

        MaxCount //삭제 금지
    }
    #region Singleton
    void Awake()
    {
        if (sound == null)
        {
            sound = this;
            OptionSetting();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    void Start()
    {
        SoundPooling(); // 사운드 파일들을 풀링 해온다
        SetAudioSource(); // 오디오 소스 세팅
        //Play("Test_BGM"); // 시작 브금
    }


    void OptionSetting()
    {
        GameObject optionCanvas = Resources.Load<GameObject>("Sound/OptionCanvas"); // 옵션 프리펩 불러오기
        this.optionCanvas = Instantiate(optionCanvas);
        optionCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>(); // 옵션 켄버스에 카메라 연결
        BGMSlider = this.optionCanvas.transform.GetChild(1).GetChild(0).GetComponent<Slider>(); //BGMSlider 위치 
        SFXSlider = this.optionCanvas.transform.GetChild(1).GetChild(1).GetComponent<Slider>(); //SFXSlider 위치
        // 제이슨에서 저장된 BGM, SFX 벨류 읽어오기 코드 추가 필요
        BGMSlider.value = BGMVolume; //= Data.saveData.ui.bgm;
        SFXSlider.value = SFXVolume; //= Data.saveData.ui.sfx;
        BGMSlider.onValueChanged.AddListener(ChangeBGMValue);
        SFXSlider.onValueChanged.AddListener(ChangeSFXValue);
        this.optionCanvas.transform.SetParent(gameObject.transform);
        //onClick()
        this.optionCanvas.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { this.optionCanvas.SetActive(false); });


        this.optionCanvas.SetActive(false);//
    }

    void ChangeBGMValue(float value)
    {
        BGMVolume = value; //slider 위치 값
        BGMSource.volume = BGMVolume;
    }

    void ChangeSFXValue(float value)
    {
        SFXVolume = value; //slider 위치 값
        SFXSource.volume = SFXVolume;
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
        BGMSource = temp[0];
        SFXSource = temp[1];
        BGMSource.loop = true;
        BGMSource.volume = BGMVolume;
        SFXSource.volume = SFXVolume;
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
            BGMSource.clip = null;
            SFXSource.clip = null;
            Debug.Log("해당 사운드는 존재하지 않습니다");
        }
    }
    public void Stop(string soundName) // 전달받은 soundName을 찾아서 정지시킨다.
    {
        if (BGMNames.Contains(soundName))
        {
            BGMSource.clip = BGMs[(int)((BGM)Enum.Parse(typeof(BGM), soundName))];
            BGMSource.Stop();
        }
        else if (SFXNames.Contains(soundName))
        {
            SFXSource.Stop();
        }
        else
        {
            Debug.Log("해당 사운드는 존재하지 않습니다");
        }
    }

}