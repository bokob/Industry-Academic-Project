using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public float timeLimit = 60.0f;
    private float currentTime;
    private TextMeshProUGUI timerText;
    void Start()
    {
        Debug.Log("시작");
        currentTime = timeLimit;
        timerText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("F0");
        }
        else
        {
            Debug.Log("시간 다 됨");
            // Time.timeScale = 0.0f; // 게임 멈춤
        }
    }
}
