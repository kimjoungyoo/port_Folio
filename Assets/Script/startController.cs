using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startController : MonoBehaviour
{
    public SharedData sharedData;

    public GameObject blackOut;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        blackOut.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        init();
    }
    private void init()
    {
        sharedData.waveLevel = 1;
        sharedData.enemyLeft = 20;
        sharedData.currentHp = sharedData.maxHp;
        //Ç®Ã¼·Â
    }
    private IEnumerator fadeOut()
    {
        audioSource.Play();
        blackOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("CampScene");
    }
    public void onStartButtonClicked()
    {
        StartCoroutine(fadeOut());
    }

    public void onExitButtonClicked()
    {
        Application.Quit();
    }
}
