using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] AnimalPrefabs; // 다양한 종류의 몬스터 프리팹
    public float spawnRate = 3.0f;     // 몬스터 생성 간격

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            // 랜덤한 위치 선택
            Vector3 randomSpawnPosition = new Vector3(
                Random.Range(-2f, 2f), // x 좌표 범위 설정
                Random.Range(-5f, 5f),  // y 좌표 범위 설정
                0f                      // z 좌표는 2D 게임에서 0으로 설정
            );

            // 랜덤한 몬스터 선택
            int randomMonsterIndex = Random.Range(0, AnimalPrefabs.Length);
            GameObject selectedMonsterPrefab = AnimalPrefabs[randomMonsterIndex];

            // 몬스터 생성
            Instantiate(selectedMonsterPrefab, randomSpawnPosition, Quaternion.identity);

            // 다음 몬스터 생성까지의 시간 간격 설정
            nextSpawnTime = Time.time + spawnRate;
        }
    }
}
