using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Dialogue
{
    public int scene { get;}
    public List<Line> lines { get;}
    
}

public class Line
{
    public float speed { get;}
    public string name { get;}
    public string line { get;}

    public Line()
    {
        this.speed = 0.05f; //속도 초기화
    }
}

/*
public class Root
{
    public List<Dialogue> dialogue { get; }
}
*/



public class DialogueJsonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
