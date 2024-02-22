using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public GameObject smallSkeletonPrefab; // smallSkeleton ������
    public GameObject bigSkeletonPrefab; // bigSkeleton ������

    public SharedData sharedData;

    private GameObject[] spawnPoints;
    private int spawnIndex = 0;

    public int spawnCount; // ��ȯ�� �� ��
    public float spawnTime;//��ȯ�Ǵ� �ð�
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

                    spawnIndex = (spawnIndex + 1) % spawnPoints.Length; // ���� ���� ����Ʈ �ε����� ������Ʈ
                    spawnCount--;

                    isSpawning = false;
                }
                else
                {
                    Debug.LogError("Spawn point not found!");
                }
            }

            yield return null; // ���� �����ӱ��� ���
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
