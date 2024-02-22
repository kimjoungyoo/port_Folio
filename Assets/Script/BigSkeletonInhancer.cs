using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSkeletonInhancer : MonoBehaviour
{
    private enemySkeletonController ctrl;
    // Start is called before the first frame update
    void Start()
    {
        ctrl = gameObject.GetComponent<enemySkeletonController>();
        ctrl.enemyHP *= 3f;
        ctrl.enemyAttackForce *= 2f;
    }
}
