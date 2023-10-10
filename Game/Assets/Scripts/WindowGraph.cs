using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    public float xSize = 150f;
    private void Awake()
    {
        // 필요한 UI 요소를 찾아서 변수에 할당
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();

        // 그래프에 표시할 데이터 초기화
        List<int> valueList = new List<int>(){5,15,10,14,3,20,7};
        
        ShowGraph(valueList);
    }

    // 원 모양 생성
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        // 원 오브젝트 생성 후 추가
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer,false);
        gameObject.GetComponent<Image>().sprite=circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        
        // 위치, 크기 및 앵커 설정
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11,11);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        
        return gameObject;
    }


    private void ShowGraph(List<int> valueList)
    {
        // 그래프 높이, 최대 y값 설정
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 20f;

        GameObject lastCircleGameObject = null;

        for(int i=0;i<valueList.Count;i++)
        {
            // x와 y 위치 계산 후 원과 라벨 생성하여 연결
            float xPosition = xSize/4 + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition,yPosition));
            if(lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            // x 라벨 생성과 설정 
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -40f);
            labelX.GetComponent<Text>().text = "Day " + (i+1).ToString();

            labelX.localScale = Vector3.one; // 스케일 1로 강제 조정 
        }

        // y 라벨 생성과 설정
        int separatorCount = 10;
        for(int i=0;i<=separatorCount;i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i / 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-30f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();

            labelY.localScale = Vector3.one; // 스케일 1로 강제 조정
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        // 두 점 연결하는 선 생성 후 추가
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer,false);

        // 선 스타일 및 위치 지정
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir)); // 선 기울이기
    }
}