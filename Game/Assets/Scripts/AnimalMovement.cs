using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalMovement : MonoBehaviour
{
    public float speed = 2;
    Vector2 hs;   //half screen 
    Vector2 dir;  //direction

    private Vector3 originalScale;

    public float scaleDuration = 1f; // 크기 변화에 걸리는 시간
    public float waitDuration = 1f; // 크기 변화 후 대기 시간

    public float scaleRatio = 0.5f;
    public float randomScaleProbability = 0.3f; // 크기 변화가 발동될 확률 (0.0 ~ 1.0)
    
    // Start is called before the first frame update
    void Start()
    {
        hs.x = Camera.main.orthographicSize;
        hs.y = Camera.main.aspect * hs.x;
        StartCoroutine(ChangeDir(3f));

        originalScale = gameObject.transform.localScale;
        StartCoroutine(AnimateAnimal());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = dir * speed * Time.deltaTime;
        transform.Translate(delta); //자동으로 z가 0으로 됨.
    }

    IEnumerator ChangeDir(float delta){
        Vector2 goal;
        while(true){
            goal.x = Random.Range(-hs.x,hs.x);
            goal.y = Random.Range(-hs.y,hs.y);

            dir = goal - (Vector2) transform.position;   //이동방향 = 목표지점 - 현재위치(Vector3)
            dir.Normalize();

            float delay = Random.Range(1f,delta);
            yield return new WaitForSeconds(delta);
        }
    }

    IEnumerator AnimateAnimal()
    {
        while (true)
        {

            if(Random.value <= randomScaleProbability)
            {
                // 이미지 축소
                LeanTween.scale(gameObject, originalScale*scaleRatio, scaleDuration).setEase(LeanTweenType.easeInOutQuad);

                yield return new WaitForSeconds(scaleDuration + waitDuration); // 대기

                // 이미지 복구
                LeanTween.scale(gameObject, originalScale, scaleDuration).setEase(LeanTweenType.easeInOutQuad);

                yield return new WaitForSeconds(scaleDuration + waitDuration); // 대기
            }
            else
            {
                // 크기 변화가 발생하지 않으면 대기만 함
                yield return new WaitForSeconds(waitDuration);
            }
        }
    }
}
