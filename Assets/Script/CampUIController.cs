using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampUIController : MonoBehaviour
{
    public GameObject mainUI;//카메라 ui
    public TextMeshProUGUI nextWaveInfo;
    public TextMeshProUGUI playerLevel;
    public TextMeshProUGUI coinMain;

    public SharedData sharedData;

    private float ui_playerLevel;
    private float ui_nextWaveLevel;
    // Start is called before the first frame update
    void Start()
    {
        sharedData.coin_Temp = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        level();
        nextWave();
    }

    private void Update()
    {
        playerCoin();//상점창에서도 보여야함.
    }

    private void nextWave()
    {
        ui_nextWaveLevel = sharedData.waveLevel;
        nextWaveInfo.text = "Next Wave : " + ui_nextWaveLevel;
    }

    private void level()
    {
        ui_playerLevel = sharedData.level;
        playerLevel.text = "Lv. " + ui_playerLevel;
    }

    private void playerCoin()
    {
        coinMain.text = "[ COIN : " + sharedData.coin_Main + " ]";
    }
}
