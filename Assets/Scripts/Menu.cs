using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameContinue()
    {
//        string save = PlayerPrefs.GetString("save");
//        JsonUtility.FromJson<PlayerController>(save);
//        Scene scene = SceneManager.GetSceneAt(1);
//        var objs = scene.GetRootGameObjects();
        
        SceneManager.LoadScene(1);
        // TODO load previous game data
    }

    public void GameExit()
    {
        Application.Quit();
    }
}