using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO; // 저장 등 파일 관리를 위해
using System;

// DB에서 퀴즈 가져올 때 사용하는 클래스
[System.Serializable]
public class Quiz
{
    public bool answer;
    public string problem;
    public string solution;
    public string animal;
}

[System.Serializable]
public class QuizList
{
    public List<Quiz> quizList;
}

// 퀴즈 저장에 사용할 클래스

[System.Serializable]
public class QuizResultItem
{
    public string quizDate;
    public int answer;
}

[System.Serializable]
public class QuizResultList
{
    public List<QuizResultItem> quizResultList;
}

// 퀴즈 관련 클래스
public class LoadQuizList : MonoBehaviour
{
    public Image quizImage;
    public TextMeshProUGUI questionText;
    public GameObject OBtn, XBtn, OkBtn;
    Button oBtn, xBtn,okBtn;
    public GameObject solutionPanel, solution;

    public Sprite[] imageList;
    public int currentQuizIndex = 0;
    int answer = 0;

    List<string> animalList = new List<string>(){
        "갈색날개매미충",
        "긴다리비틀개미",
        "꽃매미",
        "뉴트리아",
        "늑대거북",
        "등검은말벌",
        "리버쿠터",
        "미국가재",
        "미국선녀벌레",
        "배스",
        "붉은귀거북",
        "붉은불개미",
        "브라운송어",
        "블루길",
        "빗살무늬미주메뚜기",
        "아르헨티나개미",
        "악어거북",
        "열대불개미",
        "중국줄무늬목거북",
        "플로리다붉은배거북",
        "황소개구리"
    };

    string url = "http://127.0.0.1:8000/quiz";

    QuizList receivedQuizList = new QuizList();

    private IEnumerator Start()
    {
        oBtn = OBtn.GetComponent<Button>();
        xBtn = XBtn.GetComponent<Button>();
        okBtn = OkBtn.GetComponent<Button>();

        oBtn.onClick.AddListener(O);
        xBtn.onClick.AddListener(X);
        okBtn.onClick.AddListener(Save);

        yield return StartCoroutine(GetQuizList());
        Debug.Log("Start에서 출력");
        Debug.Log(receivedQuizList.quizList[0].problem);
        DisplayCurrentQuiz();
    }

    void O()
    {
        // Debug.Log("O 버튼 클릭");
        StartCoroutine(checkAnswer(true));
    }

    void X()
    {
        // Debug.Log("X 버튼 클릭");
        StartCoroutine(checkAnswer(false));
    }

    // 정답 맞추기
    IEnumerator checkAnswer(bool select)
    {

        Debug.Log("인덱스: " + currentQuizIndex);
        // 모달창 나오고 json으로 저장해야함

        Debug.Log("select : " + select);
        Debug.Log(receivedQuizList.quizList[currentQuizIndex].problem);
        Debug.Log(receivedQuizList.quizList[currentQuizIndex].answer);

        // bool result;

        if(select == receivedQuizList.quizList[currentQuizIndex].answer) // 정답
        {
            answer++;
            // result = true;
            Debug.Log("정답!");
            currentQuizIndex++;
        }
        else // 오답
        {
            // result = false;
            Debug.Log("오답!");
            currentQuizIndex++;
        }

        // 해설 5초간
        oBtn.interactable = false;
        xBtn.interactable = false;

        StartCoroutine(DisplaySolutionForSeconds(receivedQuizList.quizList[currentQuizIndex-1].solution, 4f));
        Debug.Log("현재 인덱스: " + currentQuizIndex);

        if(currentQuizIndex==20)
        {   
            yield return StartCoroutine(DisplaySolutionForSeconds(receivedQuizList.quizList[currentQuizIndex-1].solution, 4f));
            
            foreach(Transform child in transform)
            {
                Debug.Log("자식 객체 이름: " + child.gameObject.name);

                if(child.gameObject.name == "ResultPanel")
                {
                    child.gameObject.SetActive(true);

                    TextMeshProUGUI tmp;
                    tmp = child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                    tmp.text += answer.ToString();
                    // 문구를 표시합니다.
                    Debug.Log(tmp.text);

                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator GetQuizList() // 질문 리스트 가져오기
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            receivedQuizList = JsonUtility.FromJson<QuizList>(request.downloadHandler.text);
        }
        // Debug.Log(receivedQuizList.quizList);
    }

    void DisplayCurrentQuiz()
    {
        oBtn.interactable = true;
        xBtn.interactable = true;

        // 현재 퀴즈의 문제를 텍스트로 표시
        if (receivedQuizList != null && currentQuizIndex < receivedQuizList.quizList.Count)
        {
            Quiz currentQuiz = receivedQuizList.quizList[currentQuizIndex];

            int quizImageIdx = AnimalNameToInt(currentQuiz.animal);
            quizImage.sprite = imageList[quizImageIdx];
            questionText.text = currentQuiz.problem;
            // Debug.Log(currentQuiz.problem);
            // 여기에 다음 퀴즈로 넘어가는 로직 추가
        }
    }

    int AnimalNameToInt(string animalName) // 동물 이름 숫자로 매핑
    {
        int animalNum = animalList.IndexOf(animalName);

        return animalNum;
    }

    IEnumerator DisplaySolutionForSeconds(string message, float seconds)
    {
        TextMeshProUGUI tmp;
        tmp = solution.GetComponent<TextMeshProUGUI>();

        // 문구를 표시합니다.
        tmp.text = message;
        solutionPanel.gameObject.SetActive(true);

        // 일정 시간 동안 대기합니다.
        yield return new WaitForSeconds(seconds);

        // 대기 후 문구를 숨깁니다.
        solutionPanel.gameObject.SetActive(false);

        DisplayCurrentQuiz();
    }






    // 퀴즈 저장 부분
    private string filePath;
    private string quizResultFileName = "QuizResult.json";

    public void Save()
    {
        filePath = Path.Combine(Application.persistentDataPath, quizResultFileName); // 파일 경로   

        /*
        1. 유효성 검사
        최근 일주일 데이터가 있는지(개수가 7개가 아니면 추가, 이상이면 초기화 해서 새로 만든다.)
        CheckQuizResultJson();
        2. 저장
        */
        if (File.Exists(filePath)) // json이 존재하면
        {
            string jsonData = File.ReadAllText(filePath);
            QuizResultList quizResult = JsonUtility.FromJson<QuizResultList>(jsonData);

            // 1. 유효성 검사
            if (quizResult.quizResultList.Count >= 7)
            {
                // 개수가 7개 이상이면 초기화
                quizResult.quizResultList.Clear();
            }

            // 새로운 결과 추가
            QuizResultItem newQuizResultItem = new QuizResultItem();
            DateTime currentDate = DateTime.Today; // 현재 날짜
            string dateString = currentDate.ToString("MM-dd");
            newQuizResultItem.quizDate = dateString;
            newQuizResultItem.answer = answer;
            quizResult.quizResultList.Add(newQuizResultItem);

            // 2. 저장
            string updatedJsonData = JsonUtility.ToJson(quizResult, true); // json으로 변환
            File.WriteAllText(filePath, updatedJsonData);

            Debug.Log("저장 완료");
        }
        else // 저장된 파일이 없는 경우, 새로운 게임 정보 생성 후 저장
        {
            QuizResultList newQuizResultList = new QuizResultList();

            // 새로운 결과 추가
            QuizResultItem newQuizResultItem = new QuizResultItem();
            DateTime currentDate = DateTime.Today; // 현재 날짜
            string dateString = currentDate.ToString("MM-dd");
            newQuizResultItem.quizDate = dateString;
            newQuizResultItem.answer = answer;
            newQuizResultList.quizResultList = new List<QuizResultItem>();
            newQuizResultList.quizResultList.Add(newQuizResultItem);

            // 2. 저장
            string jsonData = JsonUtility.ToJson(newQuizResultList, true); // json으로 변환
            // 저장된 파일 있으면 덮어쓰고, 없으면 새로 만들어서 저장
            File.WriteAllText(filePath, jsonData);

            Debug.Log("저장 완료");
        }
    }
}