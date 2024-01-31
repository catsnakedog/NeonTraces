using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using JetBrains.Annotations;

[System.Serializable]
public class MoveDot
{
    public Vector3 v3;
    public float speed;                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
    public MoveDot(Vector3 v3, float speed)
    {
        this.v3 = v3;
        this.speed = speed;
    }
}

[System.Serializable]
public class Enemy
{
    public Vector3 v3;
    public Vector3 moveV3;
    public int type;
    public Enemy(Vector3 v3, int type, Vector3 moveV3)
    {
        this.v3 = v3;
        this.type = type;
        this.moveV3 = moveV3;
    }
}

[System.Serializable]
public class EventDot
{
    public Vector3 v3;
    public int type;
    public EventTypeInfo eventTypeInfo;
    public EventDot(Vector3 v3, int type, EventTypeInfo eventTypeInfo)
    {
        this.v3 = v3;
        this.type = type;
        this.eventTypeInfo = eventTypeInfo;
    }
}
[System.Serializable]
public class EventTypeInfo
{
    public EventType0 type0;
    public EventType1 type1;
    public EventType2 type2;
    public EventType3 type3;
    public EventType4 type4;
    public EventType5 type5;
    public EventType6 type6;
    public EventType7 type7;
    public EventType8 type8;
    public EventType9 type9;
    public EventType10 type10;
    public EventTypeInfo(EventType0 type0, EventType1 type1, EventType2 type2, EventType3 type3, EventType4 type4, EventType5 type5, EventType6 type6, EventType7 type7, EventType8 type8, EventType9 type9,EventType10 type10)
    {
        this.type0 = type0;
        this.type1 = type1;
        this.type2 = type2;
        this.type3 = type3;
        this.type4 = type4;
        this.type5 = type5;
        this.type6 = type6;
        this.type7 = type7;
        this.type8 = type8;
        this.type9 = type9;
        this.type10 = type10;
    }
}

#region EventTypes
[System.Serializable]
public class EventType0
{
    public Vector3 pointDot1;
    public Vector3 pointDot2;
    public int nextMoveDot;
    public float time;
    public bool isStop;
    public float stopTime;
    public EventType0(Vector3 pointDot1, Vector3 pointDot2,int nextMoveDot, float time, bool isStop, float stopTime)
    {
        this.pointDot1 = pointDot1;
        this.pointDot2 = pointDot2;
        this.nextMoveDot = nextMoveDot;
        this.time = time;
        this.isStop = isStop;
        this.stopTime = stopTime;
    }
}
[System.Serializable]
public class EventType1
{
    public int upSpeed;
    public float changeTime;
    public EventType1()
    {
        upSpeed = 0;
        changeTime = 0;
    }
    public EventType1(int upSpeed, float changeTime)
    {
        this.upSpeed = upSpeed;
        this.changeTime = changeTime;
    }
}
[System.Serializable]
public class EventType2
{
    public int cutSceneNumber;
    public EventType2()
    {
        cutSceneNumber = 0;
    }
    public EventType2(int cutSceneNumber)
    {
        this.cutSceneNumber = cutSceneNumber;
    }
}
[System.Serializable]
public class EventType3
{
    public int speed;
    public int index;
    public int startDot;
    public int endDot;
    public EventType3()
    {
        speed = 0;
        index = 0;
        startDot = 0;
        endDot = 0;
    }
    public EventType3(int speed, int index, int startDot, int endDot)
    {
        this.speed = speed;
        this.index = index;
        this.startDot = startDot;
        this.endDot = endDot;
    }
}
[System.Serializable]
public class EventType4
{
    public string soundName;
    public EventType4()
    {
        soundName = "";
    }
    public EventType4(string soundName)
    {
        this.soundName = soundName;
    }
}
[System.Serializable]
public class EventType5
{
    public string cameraActionName;
    public EventType5()
    {
        cameraActionName = "";
    }
    public EventType5(string cameraActionName)
    {
        this.cameraActionName = cameraActionName;
    }
}
[System.Serializable]
public class EventType6
{
    public int power;
    public int speed;
    public EventType6()
    {
        power = 0;
        speed = 0;
    }
    public EventType6(int power, int speed)
    {
        this.power = power;
        this.speed = speed;
    }
}
[System.Serializable]
public class EventType7
{
    public float timeScale;
    public EventType7()
    {
        timeScale = 0;
    }
    public EventType7(float timeScale)
    {
        this.timeScale = timeScale;
    }
}
[System.Serializable]
public class EventType8
{
    public string animationName;
    public EventType8()
    {
        animationName = "";
    }
    public EventType8(string animationName)
    {
        this.animationName = animationName;
    }
}
[System.Serializable]
public class EventType9
{
    public float stopTime;
    public EventType9()
    {
        stopTime = 0;
    }
    public EventType9(float stopTime)
    {
        this.stopTime = stopTime;
    }
}

public class EventType10
{
    public float shakeP;
    public EventType10()
    {
        shakeP = 0;
    }

    public EventType10(float shakeP)
    {
        this.shakeP = shakeP;
    }
}
#endregion

[System.Serializable]
public class MapInfo
{
    public List<MoveDot> moveDots;
    public List<Enemy> enemys;
    public List<EventDot> eventDots;
    public MapInfo(List<MoveDot> moveDots, List<Enemy> enemys, List<EventDot> eventDots)
    {
        this.moveDots = moveDots;
        this.enemys = enemys;
        this.eventDots = eventDots;
    }
}

[System.Serializable]
public class EnemyInfo
{
    public GameObject enemy;
    public float x;
    public int index;

    public EnemyInfo(GameObject enemy, float x,int index)
    {
        this.enemy = enemy;
        this.x = x;
        this.index = index;
    }
}

[System.Serializable]
public class GameData
{
    public int stage;
    public List<bool> stageClearInfo;
    public GameObject player;
    public GameObject blood;
    public GameObject bloodBoom;
    public GameObject map;
    public List<EnemyInfo> enemyInfo;
    public float camsize;
    public bool isGameStart;
    public List<GameObject> enemyPoint;
    public bool isInGame;
    public bool isCameraFollow;
    public List<bool> isFirstCutScene;
    public string crruentScene;
    public bool isFirstGame;

    public GameData()
    {
        this.stage = 0;
        this.stageClearInfo = new List<bool>();
        this.camsize = 0;
        this.isGameStart = false;
        this.enemyPoint = new List<GameObject>();
        this.isInGame = false;
        this.isCameraFollow = false;
        this.isFirstCutScene = new List<bool> { true, true, true, true };
        this.crruentScene = "";
        this.isFirstGame = true;
    }
    public GameData(int stage, List<bool> stageClearInfo, float camsize, bool isGameStart, List<GameObject> enemyPoint, bool isInGame, bool isCameraFollow, List<bool> isFirstCutScene, string crruentScene, bool isFirstGame)
    {
        this.stage = stage;
        this.stageClearInfo = stageClearInfo;
        this.camsize = camsize;
        this.isGameStart = isGameStart;
        this.enemyPoint = enemyPoint;
        this.isInGame= isInGame; 
        this.isCameraFollow = isCameraFollow;
        this.isFirstCutScene = isFirstCutScene;
        this.crruentScene= crruentScene;
        this.isFirstGame= isFirstGame;
    }
}

[System.Serializable]
public class Ui
{
    public float bgm;
    public float sfx;
    public float delay;

    public Ui()
    {
        this.bgm = 0.5f; //-40
        this.sfx = 0.5f;
        this.delay = 0f;
    }
    public Ui(float bgm, float sfx, float delay)
    {
        this.bgm = bgm;
        this.sfx = sfx;
        this.delay = delay;
    }
}

    [System.Serializable]
public class SaveDataClass
{
    public List<MapInfo> mapData;
    public GameData gameData;
    public Ui ui;
    public SaveDataClass()
    {
        mapData = new List<MapInfo>();
        gameData = new GameData();
        ui = new Ui();
    }
}