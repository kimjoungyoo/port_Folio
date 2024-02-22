using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillDestroy : MonoBehaviour
{
    public float timer;

    private GameObject Player;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        playerController.healAura.SetActive(false);

        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
