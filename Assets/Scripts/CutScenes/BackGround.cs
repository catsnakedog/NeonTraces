using UnityEngine;

public class BackGround : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] [Range(1f, 20f)] private float speed = 3f;
    [SerializeField] private float posValue;
    private Vector2 startPos;
    private float newPos;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startPos + Vector2.right * newPos;
    }
}
