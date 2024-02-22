using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skilExplosion : MonoBehaviour
{
    private SharedData sharedData;
    private weaponController weaponController;
    public GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        weaponController = weapon.GetComponent<weaponController>();
    }

    private List<GameObject> damagedEnemies = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !damagedEnemies.Contains(other.gameObject))
        {
            // 적 캐릭터에게 데미지를 주는 로직
            other.gameObject.GetComponent<enemySkeletonController>().enemyHP -= 3*weaponController.weaponDamage;
            // 데미지를 받은 적을 리스트에 추가
            damagedEnemies.Add(other.gameObject);
        }
    }

    private void ResetDamagedEnemies()
    {
        damagedEnemies.Clear();
    }
}
