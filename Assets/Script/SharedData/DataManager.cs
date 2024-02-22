using UnityEngine;

public class DataManager : MonoBehaviour
{
    //저장할 키
    //private const string SharedDataKey = "SharedData";
   
    private const string CoinMainKey = "CoinMain";
    private const string LevelKey = "Level";
    private const string attackLevelKey = "Attack";
    private const string attackSpeedLevelKey = "AttackSpeed";
    private const string moveSpeedLevelKey = "MoveSpeed";

    // ScriptableObject 인스턴스
    public SharedData sharedData;

    private void Awake()
    {
        // 데이터 로드
        LoadData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // 애플리케이션 일시 중지 시 데이터 저장
            SaveData();
        }
    }

    private void OnDisable()
    {
        // 스크립트가 비활성화될 때 데이터 저장
        SaveData();
    }

    public void SaveData()
    {
        // ScriptableObject 데이터를 JSON 형태로 직렬화하여 저장
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
            // 저장된 데이터가 있는 경우 데이터를 불러옴
            string json = PlayerPrefs.GetString(SharedDataKey);
            sharedData = JsonUtility.FromJson<SharedData>(json);
        }
        else
        {
            // 저장된 데이터가 없는 경우 새로운 ScriptableObject 인스턴스 생성
            sharedData = ScriptableObject.CreateInstance<SharedData>();
        }
        */
    }
}