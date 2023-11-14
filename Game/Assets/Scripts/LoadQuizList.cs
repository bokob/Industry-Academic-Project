using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

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

public class LoadQuizList : MonoBehaviour
{
    public Image quizImage;
    public TextMeshProUGUI questionText;
    public GameObject OBtn, XBtn;
    Button oBtn, xBtn;

    public Sprite[] imageList;
    private int currentQuizIndex = 0;
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

        oBtn.onClick.AddListener(O);
        xBtn.onClick.AddListener(X);

        yield return StartCoroutine(GetQuizList());
        Debug.Log("Start에서 출력");
        Debug.Log(receivedQuizList.quizList[0].problem);
        DisplayCurrentQuiz();
    }

    void O()
    {
        Debug.Log("O 버튼 클릭");
        checkAnswer(true);
    }

    void X()
    {
        Debug.Log("X 버튼 클릭");
        checkAnswer(false);
    }

    // 정답 맞추기
    bool checkAnswer(bool select)
    {
        Debug.Log("select : " + select);
        Debug.Log(receivedQuizList.quizList[currentQuizIndex].problem);
        Debug.Log(receivedQuizList.quizList[currentQuizIndex].answer);

        bool result;

        if(select == receivedQuizList.quizList[currentQuizIndex].answer)
        {
            answer++;
            result = true;
            Debug.Log("정답!");
            currentQuizIndex++;
            DisplayCurrentQuiz();
        }
        else
        {
            result = false;
            Debug.Log("오답!");
            currentQuizIndex++;
            DisplayCurrentQuiz();
        }

        return result;
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
}