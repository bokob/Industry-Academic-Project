using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도 조절
    public VariableJoystick joy;

    public LayerMask targetLayer; // 포착 대상 레이어

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
        /*
        // 조준 경향 위치 설정 (2D 화면에서는 Z 축 값이 중요하지 않음)
        Vector3 crosshairPosition = transform.position;

        // 레이캐스트 발사
        RaycastHit2D hit = Physics2D.Raycast(crosshairPosition, Vector2.zero, Mathf.Infinity, targetLayer);

        if (hit.collider != null)
        {
            // 레이캐스트가 포착 대상과 충돌한 경우
            // 여기서는 총알을 발사하고 포착 대상을 맞추는 로직을 추가
            Vector3 targetPosition = hit.collider.gameObject.transform.position;
            // 포착 대상을 맞추기 위한 추가 동작 수행
            // 예: 포착 대상 오브젝트에 대한 피해 적용
        }
        */
    }
}
