using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
씬 이름을 미리 설정하고 누르면 그 씬으로 이동하는 스크립트
*/

public class SwitchScene : MonoBehaviour
{
    public string sceneName;
    public void NextSceneWithString()
    {
        SceneManager.LoadScene(sceneName);
    }
}