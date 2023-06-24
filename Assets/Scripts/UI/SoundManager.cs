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

    public GameObject optionCanvas; //��ư���� ���

    // 6��22�� ����
    enum BGM
    {
        BGM_01,
        Test_BGM,
        MaxCount //���� ����
    }

    enum SFX
    {
        Test_ClickSFX,
        // UI - 6��
        Door_exit,
        Door_play,
        Execute,
        NeonLogo,
        Select,
        Setting,

        // �ƾ�/��� - 5��
        Drone_applaud,
        Drone_explode,
        Drone_joke,
        Drone_joke2,
        Drone_scan,
        // �ƾ�/���� - 4��
        Car_break,
        Car_break_crash,
        Car_crash,
        Car_passby,
        // �ƾ� - 1��
        Talk,

        // ����/���� - 12��
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
        // ���� - 11��
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

        MaxCount //���� ����
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
        SoundPooling(); // ���� ���ϵ��� Ǯ�� �ؿ´�
        SetAudioSource(); // ����� �ҽ� ����
        //Play("Test_BGM"); // ���� ���
    }


    void OptionSetting()
    {
        GameObject optionCanvas = Resources.Load<GameObject>("Sound/OptionCanvas"); // �ɼ� ������ �ҷ�����
        this.optionCanvas = Instantiate(optionCanvas);
        optionCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>(); // �ɼ� �˹����� ī�޶� ����
        BGMSlider = this.optionCanvas.transform.GetChild(1).GetChild(0).GetComponent<Slider>(); //BGMSlider ��ġ 
        SFXSlider = this.optionCanvas.transform.GetChild(1).GetChild(1).GetComponent<Slider>(); //SFXSlider ��ġ
        // ���̽����� ����� BGM, SFX ���� �о���� �ڵ� �߰� �ʿ�
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
        BGMVolume = value; //slider ��ġ ��
        BGMSource.volume = BGMVolume;
    }

    void ChangeSFXValue(float value)
    {
        SFXVolume = value; //slider ��ġ ��
        SFXSource.volume = SFXVolume;
    }

    void SoundPooling() // enum���� ���� �̸��� �о�ͼ� �ش��ϴ� ���� ������ �ε� ��Ų��
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

    void SetAudioSource() // ������ҽ��� �Ҵ��Ѵ�
    {
        AudioSource[] temp;
        temp = gameObject.GetComponents<AudioSource>();
        BGMSource = temp[0];
        SFXSource = temp[1];
        BGMSource.loop = true;
        BGMSource.volume = BGMVolume;
        SFXSource.volume = SFXVolume;
    }

    public void Play(string soundName, float pitch = 1.0f) // ���޹��� soundName�� ã�Ƽ� �����Ų��
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
            Debug.Log("�ش� ����� �������� �ʽ��ϴ�");
        }
    }
    public void Stop(string soundName) // ���޹��� soundName�� ã�Ƽ� ������Ų��.
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
            Debug.Log("�ش� ����� �������� �ʽ��ϴ�");
        }
    }

}