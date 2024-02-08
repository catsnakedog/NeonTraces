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
    Button DelayPlus;
    Button DelayMinus;
    Text DelayText;

    public Slider BGMSlider;
    public Slider SFXSlider;

    public GameObject optionCanvas; //버튼에서 사용

    
    enum BGM // Update 0904
    {
        BGM_01,
        BGM_02,
        BGM_03,
        BGM_04,
        BGM_05,
        mainscreen,
        Test_BGM,
        BG,

        MaxCount //삭제 금지
    }

    // 8월19일 업데이트
    enum SFX
    {
        Test_ClickSFX,
        // UI - 6개 @Update 0904
        Door_exit,
        Door_play,
        Execute,
        Neon,
        Select,
        Setting,

        //환경음 - 1개 (mainscreen은 BGM) @Update 0904
        mainscreen_screen_flashing,

        // 컷씬/드론 - 6개 @Update 0904
        Drone_applaud,
        Drone_error,
        Drone_explode,
        Drone_joke,
        Drone_joke2,
        Drone_scan,

        // 컷씬/차량 - 4개 @Update 0904
        Car_break,
        Car_break_crash,
        Car_crash,
        Car_passby,
        // 컷씬 - 1개 @Update 0904
        Talk,

        // 전투/메인 - 12개 @Update 0904
        A,
        ABCBAC_2_ACB,
        B,
        Block,
        C,
        Rmain_attack1,
        Rmain_attack2,
        Rmain_hit,
        Rmain_jump,
        Rmain_landing,
        Rmain_parry1,
        Rmain_parry2,
        Rmain_parry_A2,
        Rmain_parry_B2,
        Rmaindrag_new,
        Rmaindrag_original,

        //??? @Update 0823
        maindrag_nospace,
        maindrag_space,

        // 전투 - 11개 @Update 0904
        A1_attack,
        A2_attack,
        B1_attack,
        B2_attack,
        B2_shield,
        C1_attack,
        C1_move,
        C1_ready,
        C2_attack,
        C2_bolt,
        C2_case,
        Drag_new,
        Drag_new_loop,


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
    }

    public void OpenOption()
    {
        optionCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>(); // 옵션 켄버스에 카메라 연결
        optionCanvas.GetComponent<Canvas>().sortingLayerName = "UI";
        optionCanvas.GetComponent<Canvas>().sortingOrder = 100;
        optionCanvas.SetActive(true);
    }

    public void CloseOption()
    {
        optionCanvas.GetComponent<Canvas>().worldCamera = null;
        optionCanvas.SetActive(false);
    }

    void OptionSetting()
    {
        GameObject optionCanvas = Resources.Load<GameObject>("Sound/MainSetting"); // 옵션 프리펩 불러오기
        this.optionCanvas = Instantiate(optionCanvas);
        optionCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>(); // 옵션 켄버스에 카메라 연결
        DelayMinus = this.optionCanvas.transform.GetChild(1).GetChild(6).GetComponent<Button>(); //Delay_Btn_Left 위치
        DelayPlus = this.optionCanvas.transform.GetChild(1).GetChild(7).GetComponent<Button>(); //Delay_Btn_Right 위치
        DelayText = this.optionCanvas.transform.GetChild(1).GetChild(8).GetComponent<Text>(); //DelayValue_Text 위치
        BGMSlider = this.optionCanvas.transform.GetChild(1).GetChild(0).GetComponent<Slider>(); //BGMSlider 위치
        SFXSlider = this.optionCanvas.transform.GetChild(1).GetChild(1).GetComponent<Slider>(); //SFXSlider 위치
        BGMSlider.onValueChanged.AddListener(ChangeBGMValue);
        SFXSlider.onValueChanged.AddListener(ChangeSFXValue);
        DelayMinus.onClick.AddListener(MinusDelayValue);
        DelayPlus.onClick.AddListener(PlusDelayValue);

        this.optionCanvas.transform.SetParent(gameObject.transform);
        //onClick()
        this.optionCanvas.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { CloseOption(); DataManager.data.Save(); Play("Select"); });
        this.optionCanvas.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { CloseOption(); DataManager.data.Save(); Play("Select"); });


        this.optionCanvas.SetActive(false);//
    }

    void ChangeBGMValue(float value)
    {
        DataManager.data.saveData.ui.bgm = value; //slider 위치 값
        BGMSource.volume = DataManager.data.saveData.ui.bgm;
    }

    void ChangeSFXValue(float value)
    {
        DataManager.data.saveData.ui.sfx = value; //slider 위치 값
        SFXSource.volume = DataManager.data.saveData.ui.sfx;
    }

    void PlusDelayValue()
    {
        Play("Select");
        DataManager.data.saveData.ui.delay += 0.1f;
        DataManager.data.saveData.ui.delay = (float)Math.Round(DataManager.data.saveData.ui.delay, 1);
        DelayText.text = DataManager.data.saveData.ui.delay.ToString();
        if(DataManager.data.saveData.gameData.crruentScene == "InGame")
        {
            float time = BGMSource.time += 0.1f;
            if (time >= BGMSource.clip.length)
            {
                time = BGMSource.clip.length;
            }
            BGMTimeSet(time);
        }
    }

    void MinusDelayValue()
    {
        Play("Select");
        DataManager.data.saveData.ui.delay -= 0.1f;
        DataManager.data.saveData.ui.delay = (float)Math.Round(DataManager.data.saveData.ui.delay, 1);
        DelayText.text = DataManager.data.saveData.ui.delay.ToString();
        if (DataManager.data.saveData.gameData.crruentScene == "InGame")
        {
            float time = BGMSource.time -= 0.1f;
            if(time <= 0)
            {
                time = 0;
            }
            BGMTimeSet(time);
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
        BGMSource = temp[0];
        SFXSource = temp[1];
        BGMSource.loop = true;
        BGMSource.volume = DataManager.data.saveData.ui.bgm;
        SFXSource.volume = DataManager.data.saveData.ui.sfx;
        BGMSlider.value = DataManager.data.saveData.ui.bgm;
        SFXSlider.value = DataManager.data.saveData.ui.sfx;
        DelayText.text = DataManager.data.saveData.ui.delay.ToString();
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
            Debug.Log(soundName);
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
    public void Stop()
    {
        BGMSource.Stop();
        SFXSource.Stop();
    }

    public void BGMTimeSet(float value)
    {
        BGMSource.time = value;
        Debug.Log(BGMSource.time);
    }

    public void BGMLoopSet(bool value)
    {
        BGMSource.loop = value;
    }

    public void BGMPause(bool value)
    {
        if (value) BGMSource.Pause();
        else BGMSource.Play();
    }
}