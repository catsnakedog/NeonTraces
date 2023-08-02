using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ShowDialogue : MonoBehaviour
{
    // scene �� �� ������Ʈ�� ��ũ��Ʈ

    public PlayableDirector playableDirector;
    public Text Text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReceiveSignal()
    {
        Debug.Log("��ȭ ����");
        playableDirector.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // Ư�� �ð� �Ǹ� �Ͻ����� �� ��ȭ ����
        {
            playableDirector.Pause();

        }
        if (Input.GetKeyDown(KeyCode.P)) // ��ȭ ���� �� �̾ �ϱ�
        {
            playableDirector.Resume();
        }
    }
}
