using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadAnimalImage : MonoBehaviour
{
    private string imageURL = "http://3.35.4.66/image";
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