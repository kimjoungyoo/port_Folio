using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Shared Data", menuName = "Shared Data", order = 1)]
public class SharedData : ScriptableObject
{

    //플레이어 정보(누적되는 정보)
    public int coin_Main = 0;
    public int level = 1;//플레이어 레벨

    public int attackLevel = 0;
    public int attackSpeedLevel = 0;// 0.01f씩 150레벨증가
    public int moveSpeedLevel = 0; //최대 10.

    //로그라이크 한판의 데이터 or 알아서 연동되는 데이터.
    //스타트 컨트롤러로 이니셜라이징 할 것들

    public float expRatio = 1000f;//expRatio+(level*150);

    public int coin_Temp = 0;//한 로그라이크 턴에 얻은 코인, 추후에 플레이어 데이터에 추가.

    public float currentHp = 1000.0f;//PlayerController
    public float maxHp = 1000.0f;//PlayerController
    public float attackRatio = 1.0f;//공격력배수.   
    public float armorRatio = 1.0f;//최대 0.2f(데미지 80퍼센트 감소)
    public float attackSpeedRatio = 1.0f;//최대 2.66f(공속 4)   
    public float moveSpeedRatio = 1.0f;//최대 2.0f    
    
    //PlayerController

    public float attackForceRatio = 1.0f; //공격력 배율
    public float knockbackRatio = 1.0f;//넉백 파워 배율

    //wave관련
    public int waveLevel = 1;
    public int enemyLeft = 30;//1레벨 기준. waveController랑 spawnEnemy의 기준이 됨.
}