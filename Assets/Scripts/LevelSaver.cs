using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSaver : MonoBehaviour
{
    private int lvl;


    private void Start()
    {
        lvl = PlayerPrefs.GetInt("Level");

        
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(lvl);
    }
}
