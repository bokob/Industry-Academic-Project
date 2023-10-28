using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetImage : MonoBehaviour
{
    // private string imageURL = "http://127.0.0.1:8000/image";
    private string imageURL = "http://127.0.0.1:8000/graph";
    public RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTexture(img));
    }

    IEnumerator GetTexture(RawImage img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}