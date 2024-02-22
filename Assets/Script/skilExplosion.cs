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
            // �� ĳ���Ϳ��� �������� �ִ� ����
            other.gameObject.GetComponent<enemySkeletonController>().enemyHP -= 3*weaponController.weaponDamage;
            // �������� ���� ���� ����Ʈ�� �߰�
            damagedEnemies.Add(other.gameObject);
        }
    }

    private void ResetDamagedEnemies()
    {
        damagedEnemies.Clear();
    }
}
