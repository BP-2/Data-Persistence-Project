using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuHandler : MonoBehaviour
{
    ///string input;
    [SerializeField] Text input;
     public void StartNew()
    {
        ///input = gameObject.GetComponent<InputField>().text;
        SceneManager.LoadScene(1);
    }
    public void setName(){
        MasterDataHandler.Instance.playerName = input.text;
    }
    public void Exit(){
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}
