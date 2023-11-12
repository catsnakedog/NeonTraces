using UnityEngine;

public class BackGround : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private bool once = false;
    private BackGround background;
    [SerializeField] private float delayTime = 0f;
    [SerializeField] [Range(1f, 20f)] private float speed = 3f;
    [SerializeField] private float posValue;
    private Vector2 startPos;
    private float newPos;
    void Start()
    {
        startPos = transform.position;
        background = gameObject.GetComponent<BackGround>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.activeSelf && Time.time > delayTime)
        {
            newPos = Mathf.Repeat((Time.time - delayTime) * speed, posValue);
            transform.position = startPos + Vector2.left * newPos;
            if (once && (startPos.x + Vector2.left.x * posValue + 0.1 > transform.position.x)) //목표 위치 + 오차 > 실제 위치
                background.enabled = false;
        }
    }
}
