using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Shared Data", menuName = "Shared Data", order = 1)]
public class SharedData : ScriptableObject
{

    //�÷��̾� ����(�����Ǵ� ����)
    public int coin_Main = 0;
    public int level = 1;//�÷��̾� ����

    public int attackLevel = 0;
    public int attackSpeedLevel = 0;// 0.01f�� 150��������
    public int moveSpeedLevel = 0; //�ִ� 10.

    //�α׶���ũ ������ ������ or �˾Ƽ� �����Ǵ� ������.
    //��ŸƮ ��Ʈ�ѷ��� �̴ϼȶ���¡ �� �͵�

    public float expRatio = 1000f;//expRatio+(level*150);

    public int coin_Temp = 0;//�� �α׶���ũ �Ͽ� ���� ����, ���Ŀ� �÷��̾� �����Ϳ� �߰�.

    public float currentHp = 1000.0f;//PlayerController
    public float maxHp = 1000.0f;//PlayerController
    public float attackRatio = 1.0f;//���ݷ¹��.   
    public float armorRatio = 1.0f;//�ִ� 0.2f(������ 80�ۼ�Ʈ ����)
    public float attackSpeedRatio = 1.0f;//�ִ� 2.66f(���� 4)   
    public float moveSpeedRatio = 1.0f;//�ִ� 2.0f    
    
    //PlayerController

    public float attackForceRatio = 1.0f; //���ݷ� ����
    public float knockbackRatio = 1.0f;//�˹� �Ŀ� ����

    //wave����
    public int waveLevel = 1;
    public int enemyLeft = 30;//1���� ����. waveController�� spawnEnemy�� ������ ��.
}