using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도 조절
    public VariableJoystick joy;

    public LayerMask targetLayer; // 포착 대상 레이어
    public string targetTag = "Animal";


    Dictionary<string, Action> animalActions = new Dictionary<string, Action>
    {
        {"뉴트리아", () => DataManager.Instance.gameData.MyocastorCoypusCnt++},
        {"늑대거북", () => DataManager.Instance.gameData.ChelydraSerpentinaCnt++},
        {"악어거북", () => DataManager.Instance.gameData.MacrochelysTemminckiiCnt++},
        {"플로리다붉은배거북", () => DataManager.Instance.gameData.PseudemysNelsoniCnt++},
        {"리버쿠터", () => DataManager.Instance.gameData.PseudemysConcinnaCnt++},
        {"중국줄무늬목거북", () => DataManager.Instance.gameData.MauremysSinensisCnt++},
        {"붉은귀거북", () => DataManager.Instance.gameData.TrachemysSppCnt++},
        {"황소개구리", () => DataManager.Instance.gameData.LithobatesCatesbeianusCnt++},
        {"배스", () => DataManager.Instance.gameData.MicropterusSalmoidesCnt++},
        {"브라운송어", () => DataManager.Instance.gameData.SalmoTruttaCnt++},
        {"블루길", () => DataManager.Instance.gameData.LepomisMacrochirusCnt++},
        {"갈색날개매미충", () => DataManager.Instance.gameData.PochaziaShantungensisCnt++},
        {"긴다리비틀개미", () => DataManager.Instance.gameData.AnoplolepisGracilipesCnt++},
        {"꽃매미", () => DataManager.Instance.gameData.LycormaDelicatulaCnt++},
        {"등검은말벌", () => DataManager.Instance.gameData.VespaVelutinaNigrithoraxCnt++},
        {"미국선녀벌레", () => DataManager.Instance.gameData.MetcalfaPruinosaCnt++},
        {"붉은불개미", () => DataManager.Instance.gameData.SolenopsisInvictaCnt++},
        {"빗살무늬미주메뚜기", () => DataManager.Instance.gameData.MelanoplusDifferentialisCnt++},
        {"아르헨티나개미", () => DataManager.Instance.gameData.LinepithemaHumileCnt++},
        {"열대불개미", () => DataManager.Instance.gameData.SolenopsisGeminataCnt++},
        {"미국가재", () => DataManager.Instance.gameData.ProcambarusClarkiiCnt++}
    };

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        else
        {
            float x = joy.Horizontal;
            float y = joy.Vertical;

            Vector3 moveDirection = new Vector3(x, y, 0);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        Debug.Log("뒤져랏!");
        
        // 조준 경향 위치 설정 (2D 화면에서는 Z 축 값이 중요하지 않음)
        Vector3 crosshairPosition = transform.position;

        // 레이캐스트 발사
        RaycastHit2D hit = Physics2D.Raycast(crosshairPosition, Vector2.zero, Mathf.Infinity, targetLayer);

        if (hit.collider != null)
        {
            hit.collider.gameObject.SetActive(false);

            // 포착 대상의 이름 출력
            Debug.Log("포착한 대상의 이름: " + hit.collider.gameObject.name);
            RecordingHuntingAnimal(hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("포착한게 없음");
        }
    }

    void RecordingHuntingAnimal(string animalName)
    {
        foreach(string animal in animalActions.Keys)
        {
            if(animalName.Contains(animal))
            {
                Debug.Log("이 동물은 " + animal +" 이군요");
                Debug.Log("이제" + animalName + " 을 처리할겁니닷!");

                if (animalActions.TryGetValue(animal, out Action action))
                {
                    action.Invoke();
                    Debug.Log("크큭 성공");
                    Debug.Log("저장된 뉴트리아 수: " + DataManager.Instance.gameData.MyocastorCoypusCnt);
                }
                else
                {
                    // 매핑된 동물이 없을 때의 처리
                    Debug.Log("띠용");
                }
            }
        }

    }
}