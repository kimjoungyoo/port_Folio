using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class waveController : MonoBehaviour
{
    //sharedData에서 정보를 받아와서, sharedData를 건드리지않고 씬 내에서 처리하는 스크립트.

    public SharedData sharedData;

    public int waveKillLeft;

    public bool isClear;//클리어하면 캠프씬으로 돌아가고 웨이브 레벨 증가

    // Start is called before the first frame update
    void Start()
    {
        sharedData.enemyLeft = 10 + (sharedData.waveLevel * 10);
        waveKillLeft = sharedData.enemyLeft;

        sharedData.coin_Temp = 0;//임시코인 0으로 초기화
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ClearWave();
    }

    private void ClearWave()
    {       
        if(waveKillLeft == 0 && !isClear)
        {
            sharedData.waveLevel++;
            isClear = true;
            sharedData.coin_Main = sharedData.coin_Temp;
            StartCoroutine(goCampScene());
        }
    }

    private IEnumerator goCampScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("CampScene");
    }
}
