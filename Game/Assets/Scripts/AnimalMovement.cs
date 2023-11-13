using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float speed = 2;
    Vector2 hs;   //half screen 
    Vector2 dir;  //direction
    
    // Start is called before the first frame update
    void Start()
    {
        hs.x = Camera.main.orthographicSize;
        hs.y = Camera.main.aspect * hs.x;

        StartCoroutine(ChangeDir(3f));
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
}
