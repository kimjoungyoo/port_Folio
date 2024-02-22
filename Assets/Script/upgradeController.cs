using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class upgradeController : MonoBehaviour
{

    public SharedData sharedData;

    //level
    public TextMeshProUGUI ui_level;
    public TextMeshProUGUI coin_level;
    private int levelCoinValue;
    //attack
    public TextMeshProUGUI ui_attack;
    public TextMeshProUGUI coin_attack;
    private int attackCoinValue;
    //attackSpeed
    public TextMeshProUGUI ui_attackSpeed;
    public TextMeshProUGUI coin_AS;
    private int asCoinValue;
    //moveSpeed
    public TextMeshProUGUI ui_moveSpeed;
    public TextMeshProUGUI coin_MS;
    private int msCoinValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textManage();
    }
    public void textManage()
    {
        //레벨업
        ui_level.text = sharedData.level.ToString();
        levelCoinValue = (sharedData.level <= 10) ? (100 + (sharedData.level * 15)) : (sharedData.level * 35);
        coin_level.text = levelCoinValue.ToString();
        //공격업
        ui_attack.text = (1.0f + (sharedData.attackLevel * 0.05f)).ToString();
        attackCoinValue = (sharedData.attackLevel <= 30) ? (30 + sharedData.attackLevel*3) : (sharedData.attackLevel<=100) ? (sharedData.attackLevel * 4):(sharedData.attackLevel*5);
        coin_attack.text = attackCoinValue.ToString();
        //공격속도
        ui_attackSpeed.text = (1.0f + (sharedData.attackSpeedLevel * 0.01f)).ToString();
        asCoinValue = (sharedData.attackSpeedLevel <= 50) ? (20 + sharedData.attackSpeedLevel * 20) : (sharedData.attackSpeedLevel * 25);
        coin_AS.text = asCoinValue.ToString();
        //이동속도
        ui_moveSpeed.text = (1.0f + (sharedData.moveSpeedLevel * 0.2f)).ToString();
        msCoinValue = (150 + sharedData.moveSpeedLevel * 350);
        coin_MS.text = msCoinValue.ToString();

    }

    public void up_Level()
    {
        if(sharedData.coin_Main >= levelCoinValue)
        {
            sharedData.coin_Main -= levelCoinValue;
            sharedData.level++;
        }
    }//레벨업 버튼

    public void up_Attack()
    {
        if(sharedData.coin_Main >= attackCoinValue)
        {
            sharedData.coin_Main -= attackCoinValue;
            sharedData.attackLevel += 1;
            sharedData.attackRatio = 1.0f + (sharedData.attackLevel * 0.05f);
            
        }
    }//공격업 버튼

    public void up_AttackSpeed()
    {
        if (sharedData.attackSpeedLevel <= 150 && (sharedData.coin_Main >= asCoinValue))
        {
            sharedData.coin_Main -= asCoinValue;
            sharedData.attackSpeedLevel += 1;
            sharedData.attackSpeedRatio = 1.0f + (sharedData.attackSpeedLevel * 0.01f);
            
        }
    }//공격속도업 버튼

    public void up_MoveSpeed()
    {
        if(sharedData.moveSpeedLevel<=10 && sharedData.coin_Main >= msCoinValue)
        {
            sharedData.coin_Main -= msCoinValue;
            sharedData.moveSpeedLevel += 1;
            sharedData.moveSpeedRatio = 1.0f + (sharedData.moveSpeedLevel * 0.2f);
            
        }
    }//이동속도업 버튼
}
