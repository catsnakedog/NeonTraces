using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // ����ü�� �ν����� â�� ���̰� �ϱ� ���� �۾�
public struct TalkData
{
    public string name; // ��� ġ�� ĳ���� �̸�
    public string[] contexts; // ��� ����
    public string speed; // ��� Ÿ���� �ӵ�
    //public int fontSize; // ��Ʈ ũ��
}

public class Dialogue : MonoBehaviour
{
    // ��ȭ �̺�Ʈ �̸�
    [SerializeField] string eventPart = null; // �ƾ���ȣ + ��Ʈ ��ȣ (ex) 01: �ƾ�0 ��Ʈ1

    // ������ ������ TalkData �迭 
    [SerializeField] TalkData[] talkDatas = null; // index 0��: ���ΰ� / 1��: ��� / 2��: ����Ŀ / ...

    public TalkData[] GetObjectDialogue()
    {
        return DialogueParse.GetDialogue(eventPart);
    }
}