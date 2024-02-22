using UnityEngine;

public class DataManager : MonoBehaviour
{
    //������ Ű
    //private const string SharedDataKey = "SharedData";
   
    private const string CoinMainKey = "CoinMain";
    private const string LevelKey = "Level";
    private const string attackLevelKey = "Attack";
    private const string attackSpeedLevelKey = "AttackSpeed";
    private const string moveSpeedLevelKey = "MoveSpeed";

    // ScriptableObject �ν��Ͻ�
    public SharedData sharedData;

    private void Awake()
    {
        // ������ �ε�
        LoadData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // ���ø����̼� �Ͻ� ���� �� ������ ����
            SaveData();
        }
    }

    private void OnDisable()
    {
        // ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ ����
        SaveData();
    }

    public void SaveData()
    {
        // ScriptableObject �����͸� JSON ���·� ����ȭ�Ͽ� ����
        PlayerPrefs.SetInt(CoinMainKey, sharedData.coin_Main);
        PlayerPrefs.SetInt(LevelKey, sharedData.level);
        PlayerPrefs.SetInt(attackLevelKey, sharedData.attackLevel);
        PlayerPrefs.SetInt(attackSpeedLevelKey, sharedData.attackSpeedLevel);
        PlayerPrefs.SetInt(moveSpeedLevelKey, sharedData.moveSpeedLevel);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(CoinMainKey))
            sharedData.coin_Main = PlayerPrefs.GetInt(CoinMainKey);

        if (PlayerPrefs.HasKey(LevelKey))
            sharedData.level = PlayerPrefs.GetInt(LevelKey);

        if (PlayerPrefs.HasKey(attackLevelKey))
            sharedData.attackLevel = PlayerPrefs.GetInt(attackLevelKey);

        if (PlayerPrefs.HasKey(attackSpeedLevelKey))
            sharedData.attackSpeedLevel = PlayerPrefs.GetInt(attackSpeedLevelKey);

        if (PlayerPrefs.HasKey(LevelKey))
            sharedData.moveSpeedLevel = PlayerPrefs.GetInt(moveSpeedLevelKey);


        /*
        if (PlayerPrefs.HasKey(SharedDataKey))
        {
            // ����� �����Ͱ� �ִ� ��� �����͸� �ҷ���
            string json = PlayerPrefs.GetString(SharedDataKey);
            sharedData = JsonUtility.FromJson<SharedData>(json);
        }
        else
        {
            // ����� �����Ͱ� ���� ��� ���ο� ScriptableObject �ν��Ͻ� ����
            sharedData = ScriptableObject.CreateInstance<SharedData>();
        }
        */
    }
}