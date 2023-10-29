using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;

public class MyData // 딕셔너리 이용하면 오류 발생해서 리스트 2개로 해결
{
    public List<string> date;
    public List<int> count;
}


public class LoadGraphImage : MonoBehaviour
{

    // private string imageURL = "http://127.0.0.1:8000/image";
    private string imageURL = "http://127.0.0.1:8000/graph";
    public RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendQuizRecordJsonDataToServer());
    }

    IEnumerator SendQuizRecordJsonDataToServer()
    {
        /* (곧 작성 예정)
            json으로 저장된 퀴즈 결과를 불러와서
            key 값과 value 값을 각각의 리스트로 담는 부분
        */

        MyData jsonData = new MyData
        {
            date = new List<string>
            {
                "10-29",
                "10-30",
                "10-31",
                "11-01",
                "11-02",
                "11-03",
                "11-04"
            },
            count = new List<int>
            {
                3,6,9,20,9,6,3
            }
        };



        string json = JsonUtility.ToJson(jsonData);

        // JSON 데이터를 FastAPI 서버로 POST 요청으로 보내기
        UnityWebRequest request = new UnityWebRequest(imageURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // Debug.Log("전송 시작전");
        yield return request.SendWebRequest();
        // Debug.Log("전송완료");

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // 이미지 데이터를 byte 배열로 받음
            byte[] imageBytes = request.downloadHandler.data;
            
            // 바이트 배열을 Texture2D로 변환
            Texture2D texture = new Texture2D(2, 2); // 텍스처 크기를 조절
            texture.LoadImage(imageBytes);

            // RawImage에 이미지 설정
            img.texture = texture;
        }
        request.Dispose(); // 메모리 누수 막기 위해
    }
}