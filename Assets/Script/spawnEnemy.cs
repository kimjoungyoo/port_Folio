using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public GameObject smallSkeletonPrefab; // smallSkeleton 프리팹
    public GameObject bigSkeletonPrefab; // bigSkeleton 프리팹

    public SharedData sharedData;

    private GameObject[] spawnPoints;
    private int spawnIndex = 0;

    public int spawnCount; // 소환할 적 수
    public float spawnTime;//소환되는 시간
    private bool isSpawning = false;

    private void Start()
    {
        sharedData.enemyLeft = 10 + (sharedData.waveLevel * 10);
        spawnCount = sharedData.enemyLeft;
        spawnTime = 0.65f - (0.01f * sharedData.waveLevel);
        StartCoroutine(SpawnSkeletons());
    }

    private void FixedUpdate()
    {
       
    }
    private IEnumerator SpawnSkeletons()
    {
        while (spawnCount > 0)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                yield return new WaitForSeconds(spawnTime);

                spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoint");
                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    float randomValue = Random.Range(0f, 1f);
                    GameObject skeletonPrefab = (randomValue <= 0.85f) ? smallSkeletonPrefab : bigSkeletonPrefab;

                    Instantiate(skeletonPrefab, spawnPoints[spawnIndex].transform.position, Quaternion.identity);

                    spawnIndex = (spawnIndex + 1) % spawnPoints.Length; // 다음 스폰 포인트 인덱스로 업데이트
                    spawnCount--;

                    isSpawning = false;
                }
                else
                {
                    Debug.LogError("Spawn point not found!");
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
    /*
    private void SpawnSkeleton()
    {       
        while (spawnCount > 0)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoint");
            if (spawnPoints != null)
            {
                StartCoroutine(SpawnSkeletons());
                isSpawn = false;
                Debug.Log("spawn");
            }
            else
            {
                Debug.LogError("Spawn point not found!");
            }
        }
        
    }
    */
}
