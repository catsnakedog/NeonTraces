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
    public int type;
    public Enemy(Vector3 v3, int type)
    {
        this.v3 = v3;
        this.type = type;
    }
}

[System.Serializable]
public class EventDot
{
    public Vector3 v3;
    public int type;
    public EventDot(Vector3 v3, int type)
    {
        this.v3 = v3;
        this.type = type;
    }
}

[System.Serializable]
public class MapInfo
{
    public List<MoveDot> moveDots;
    public List<Enemy> enemys;
    public List<EventDot> eventDots;
    public MapInfo()
    {
        moveDots = new List<MoveDot>();
        enemys = new List<Enemy>();
        eventDots = new List<EventDot>();
    }
    public MapInfo(List<MoveDot> moveDots, List<Enemy> enemys, List<EventDot> eventDots)
    {
        this.moveDots = moveDots;
        this.enemys = enemys;
        this.eventDots = eventDots;
    }
}
[System.Serializable]
public class GameData
{
    public int stage;
    public List<bool> stageClearInfo;
    public GameObject player;

    public GameData()
    {
        this.stage = 0;
        this.stageClearInfo = new List<bool> { };
    }
    public GameData(int stage, List<bool> stageClearInfo)
    {
        this.stage = stage;
        this.stageClearInfo = stageClearInfo;
    }
}

[System.Serializable]
public class SaveDataClass
{
    public List<MapInfo> mapData;
    public GameData gameData;
    public SaveDataClass()
    {
        mapData = new List<MapInfo>();
        gameData = new GameData();
    }
}