using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class waveController : MonoBehaviour
{
    //sharedData���� ������ �޾ƿͼ�, sharedData�� �ǵ帮���ʰ� �� ������ ó���ϴ� ��ũ��Ʈ.

    public SharedData sharedData;

    public int waveKillLeft;

    public bool isClear;//Ŭ�����ϸ� ķ�������� ���ư��� ���̺� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        sharedData.enemyLeft = 10 + (sharedData.waveLevel * 10);
        waveKillLeft = sharedData.enemyLeft;

        sharedData.coin_Temp = 0;//�ӽ����� 0���� �ʱ�ȭ
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
