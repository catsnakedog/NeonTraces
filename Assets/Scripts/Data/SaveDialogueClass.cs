using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public int scene { get; set; }
    public List<Line> lines { get; set; }

}

public class Line
{
    public float speed { get; set; }
    public string name { get; set; }
    public string line { get; set; }

    public Line()
    {
        this.speed = 0.05f; //속도 초기화
    }
}

[System.Serializable]
public class SaveDialogueClass
{
    public List<Dialogue> dialogue { get; set; }
}

