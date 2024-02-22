using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollection : MonoBehaviour
{
    public GameObject player;

    public float minDistance = 2.5f;
    public float flyingSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= minDistance)
        {
            coinSelection();
        }
    }
    
    private void coinSelection()
    {
        Vector3 direction = player.transform.position - transform.position;
        transform.position += direction.normalized * flyingSpeed * Time.deltaTime;
    }
}
