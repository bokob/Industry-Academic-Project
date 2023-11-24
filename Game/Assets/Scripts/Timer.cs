using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    public AimTarget aimTarget;
    private bool isScriptEnabled = true;
    public float timeLimit = 60.0f;
    private float currentTime;
    private TextMeshProUGUI timerText;

    public GameObject resultPanel;
    public Image resultImage;
    public Sprite[] successImageList;
    public Sprite[] failImageList;

    void Start()
    {
        Debug.Log("시작");
        currentTime = timeLimit;
        timerText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isScriptEnabled)
        {
            if(aimTarget.progressbar.value == 100)
            {
                Debug.Log("성공");
                resultPanel.SetActive(true);
                
                System.Random random = new System.Random();
                int randomIdx = random.Next(0, successImageList.Length);
                resultImage.sprite = successImageList[randomIdx];

                isScriptEnabled = false;
            }
            else if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                timerText.text = currentTime.ToString("F0");
            }
            else
            {
                Debug.Log("실패");
                resultPanel.SetActive(true);

                System.Random random = new System.Random();
                int randomIdx = random.Next(0, failImageList.Length);
                resultImage.sprite = failImageList[randomIdx];

                isScriptEnabled = false;
                // Time.timeScale = 0.0f; // 게임 멈춤
            }
        }
    }
}