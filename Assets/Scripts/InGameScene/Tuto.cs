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
    GameObject _click;

    void Start()
    {
        _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = _mainCamera;
        gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
        gameObject.GetComponent<Canvas>().sortingOrder = 9;
        _playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        _active = transform.GetChild(1).GetComponent<Button>();
        _click = transform.GetChild(2).gameObject;

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        _active.onClick.AddListener(Next);
        _click.SetActive(true);
        StartCoroutine(ClickEffect(_click));
    }

    void Next()
    {
        Time.timeScale = 1;
        switch(num)
        {
            case 0:
                _playerAction.Attack();
                break;
            case 1:
                _playerAction.Defence();
                break;
            case 2:
                _playerAction.Defence();
                break;
            case 3:
                _playerAction.Defence();
                break;
            case 4:
                break;
            case 5:
                _playerAction.Attack();
                break;
        }

        Destroy(gameObject);
    }

    IEnumerator ClickEffect(GameObject obj)
    {
        float fillAmount = 1f;
        float time = 0f;
        Image image = obj.GetComponent<Image>();
        time = Time.realtimeSinceStartup * 1.25f;
        while (fillAmount > 0f)
        {
            fillAmount -= Time.realtimeSinceStartup * 1.25f - time;
            image.color = new Color(1f, 1f, 1f, fillAmount);
            time = Time.realtimeSinceStartup * 1.25f;
            yield return null;
        }

        fillAmount = 0f;
        while (fillAmount < 1f)
        {
            fillAmount += Time.realtimeSinceStartup * 1.25f - time;
            image.color = new Color(1f, 1f, 1f, fillAmount);
            time = Time.realtimeSinceStartup * 1.25f;
            yield return null;
        }
        StartCoroutine(ClickEffect(_click));
    }
}
