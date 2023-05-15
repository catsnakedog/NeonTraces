using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loading : MonoBehaviour
{
    public GameObject TeamName;
    public GameObject GameLogo;

    // Start is called before the first frame update
    void Start()
    {
        GameLogo.SetActive(false);
        TeamName.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
