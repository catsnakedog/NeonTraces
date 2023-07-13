using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange0 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        SceneManager.LoadScene("CutScene0-1", LoadSceneMode.Single);
    }
}
