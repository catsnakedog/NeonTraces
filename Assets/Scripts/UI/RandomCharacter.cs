using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacter : MonoBehaviour
{
    public GameObject[] CharacterSet;
    private int RandomInt; //랜덤 변수

    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        Debug.Log("Random");
        for (int i = 0; i < CharacterSet.Length; i++) // 캐릭터Set 모두 비활성화
        {
            CharacterSet[i].SetActive(false);
        }

        RandomInt = Random.Range(0, CharacterSet.Length); //랜덤 범위 설정

        CharacterSet[RandomInt].SetActive(true); //해당 캐릭터Set 활성화
    }
}
