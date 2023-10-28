using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Test
{
    public string message;
}

public class GetQuizList : MonoBehaviour
{

    string url = "http://127.0.0.1:8000/test";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadQuizList(url));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadQuizList(string url) // 질문 리스트 가져오기
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        Test tmp = new Test();
        tmp.message="";
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            tmp = JsonUtility.FromJson<Test>(request.downloadHandler.text);
        }

        Debug.Log("안에 담긴 것: " + tmp.message );
    }
    
}
