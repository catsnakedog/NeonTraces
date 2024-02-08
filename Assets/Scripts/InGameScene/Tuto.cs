using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto : MonoBehaviour
{
    [SerializeField] int num;

    Camera _mainCamera;
    PlayerAction _playerAction;
    Button _active;

    void Start()
    {
        _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = _mainCamera;
        gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
        gameObject.GetComponent<Canvas>().sortingOrder = 9;
        _playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        _active = transform.GetChild(1).GetComponent<Button>();

        _active.onClick.AddListener(Next);
    }

    void Next()
    {
        Time.timeScale = 1;
        if (num == 0)
            _playerAction.Attack();
        if (num == 1)
            _playerAction.Defence();
        if (num == 2)
            _playerAction.Defence();
        if (num == 3)
            _playerAction.Defence();

        Destroy(gameObject);
    }
}
