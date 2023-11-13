using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 저장 등 파일 관리를 위해

public class DataManager : MonoBehaviour
{
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    
    // 저장할 파일의 경로 담을 변수
    private string filePath; 

    // 게임 데이터 저장 json 이름
    private string fileName = "GameData.json"; 
    
    // 게임 데이터 열어보기 위해 저장하는 변수
    public GameData gameData;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            Load();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Load() // 게임 정보 불러오기
    {
        if (File.Exists(filePath)) // json이 존재하면
        {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
            Debug.Log("게임 정보를 불러왔어요.");
        }
        else // 저장된 파일이 없는 경우, 새로운 게임 정보 생성 후 저장
        {
            gameData = CreateNewGameData();
            Save();
        }
    }

    public GameData CreateNewGameData() // 새 게임 정보 샏성
    {
        GameData newGameData = new GameData();

        // 포유류 1종
        newGameData.MyocastorCoypusCnt=0;

        // 파충류 6종
        newGameData.ChelydraSerpentinaCnt=0; // 늑대거북
        newGameData.MacrochelysTemminckiiCnt=0; // 악어거북
        newGameData.PseudemysNelsoniCnt=0; // 플로리다붉은배거북
        newGameData.PseudemysConcinnaCnt=0; // 리버쿠터
        newGameData.MauremysSinensisCnt=0; // 중국줄무늬목거북
        newGameData.TrachemysSppCnt=0; // 붉은귀거북

        // 양서류 1종
        newGameData.LithobatesCatesbeianusCnt=0; // 황소개구리

        // 어류 3종
        newGameData.MicropterusSalmoidesCnt=0; // 배스
        newGameData.SalmoTruttaCnt=0; // 브라운송어
        newGameData.LepomisMacrochirusCnt=0; // 블루길

        // 곤충 9종
        newGameData.PochaziaShantungensisCnt=0;// 갈색날개매미충
        newGameData.AnoplolepisGracilipesCnt=0;// 긴다리비틀개미
        newGameData.LycormaDelicatulaCnt=0;// 꽃매미
        newGameData.VespaVelutinaNigrithoraxCnt=0;// 등검은말벌
        newGameData.MetcalfaPruinosaCnt=0;// 미국선녀벌레
        newGameData.SolenopsisInvictaCnt=0;// 붉은불개미
        newGameData.MelanoplusDifferentialisCnt=0;// 빗살무늬미주메뚜기
        newGameData.LinepithemaHumileCnt=0;// 아르헨티나개미
        newGameData.SolenopsisGeminataCnt=0;// 열대불개미

        // 무척추동물 1종
        newGameData.ProcambarusClarkiiCnt=0;// 미국가재

        return newGameData;
    }

    public void UpdateGameData(int action) // 게임 정보 업데이트
    {
        
        // switch(action)
        // {
        //     case 0: 
        //         break;
        //     case 1: // bgm 소리 크기
        //         gameData.bgmVolume = SoundManager.Instance.musicSource.volume;
        //         Debug.Log("브금 크기가 " + gameData.bgmVolume + " 로 저장됩니다.");
        //         break;
        //     case 2: // 효과음 소리 크기
        //         gameData.sfxVolume = SoundManager.Instance.sfxSource.volume;
        //         Debug.Log("효과음 크기가 " + gameData.sfxVolume + " 로 저장됩니다.");
        //         break;
        //     case 3: // 최초 엔딩 시에만 보여주게끔
        //         gameData.last = false;
        //         break;
        //     default:
        //         break;
        // }

        Save();
    }

    public void Save() // 게임 정보 저장
    {
        string jsonData = JsonUtility.ToJson(gameData, true); // json으로 변환
        
        // 저장된 파일 있으면 덮어쓰고, 없으면 새로 만들어서 저장
        File.WriteAllText(filePath, jsonData);

        Debug.Log("저장 완료");
    }
}