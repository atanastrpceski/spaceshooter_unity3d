using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void LoadCoOp()
    {
        SceneManager.LoadScene("CoOp");
    }
}
