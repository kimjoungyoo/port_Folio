using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mainUICtroller : MonoBehaviour
{
    public GameObject mainUI;//카메라 ui
    public TextMeshProUGUI waveInfo;
    public TextMeshProUGUI playerLevel;
    public TextMeshProUGUI coinTemp;
    public GameObject Player;

    //스크립트
    public spawnEnemy spawnEnemy;
    public waveController waveController;
    public SharedData sharedData;
    public PlayerController playerCtrl;

    private float ui_waveLevel;
    private float ui_enemyLeft;
    private float ui_playerLevel;
    private float ui_counTemp;

    public GameObject ui_skill_explosionCoolDown;
    public TextMeshProUGUI ui_explosionCoolTime;
    public GameObject ui_skill_turnUndeadCoolDown;
    public TextMeshProUGUI ui_turnUndeadCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemy = this.gameObject.GetComponent<spawnEnemy>();
        waveController = this.gameObject.GetComponent<waveController>();
        playerCtrl = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        waveCount();
        level();
        coinCount();
        coolDown();
    }

    private void coolDown()
    {
        if (playerCtrl.isExplosionCoolTime)
        {
            ui_skill_explosionCoolDown.SetActive(true);
            float expCT = playerCtrl.explosionCoolTime;
            ui_explosionCoolTime.text = expCT.ToString("F1");//소수점 1자리

        }else if (!playerCtrl.isExplosionCoolTime)
        {
            ui_skill_explosionCoolDown.SetActive(false);
        }

        if (playerCtrl.isturnUndeadCoolTime)
        {
            ui_skill_turnUndeadCoolDown.SetActive(true);
            float turnundeadCT = playerCtrl.turnUndeadCoolTime;
            ui_turnUndeadCoolTime.text = turnundeadCT.ToString("F1");

        }
        else if (!playerCtrl.isturnUndeadCoolTime)
        {
            ui_skill_turnUndeadCoolDown.SetActive(false);
        }


        
    }

    private void waveCount()
    {
        ui_waveLevel = sharedData.waveLevel;
        ui_enemyLeft = waveController.waveKillLeft;

        waveInfo.text = "[ Wave: "+ ui_waveLevel.ToString() +" ] [ Enemy Left: "+ ui_enemyLeft.ToString() +" ]";
    }

    private void level()
    {
        ui_playerLevel = sharedData.level;
        playerLevel.text = "Lv. " + ui_playerLevel;
    }

    private void coinCount()
    {
        ui_counTemp = sharedData.coin_Temp;
        coinTemp.text = "[ COIN : " + ui_counTemp + " ]";
    }
}
