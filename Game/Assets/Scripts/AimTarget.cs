using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도 조절
    public VariableJoystick joy;

    public LayerMask targetLayer; // 포착 대상 레이어
    public string targetTag = "Animal";

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
        }
        else
        {
            Debug.Log("포착한게 없음");
        }
    }
}