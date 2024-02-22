using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CampSceneController : MonoBehaviour
{
    public GameObject upgeradeChest;
    //모닥불
    public GameObject bonFire;
    public GameObject bonFireParticle;
    public GameObject bonFireLight;

    public GameObject gameExit;
    public GameObject Player;

    public GameObject ui_upgrade;
    public GameObject ui_nextWave;
    public GameObject ui_exitGame;
    public GameObject blackOut;

    public GameObject ui_interactUpgrade;

    //애니메이션관련
    public Animator chestAnimator;

    //스크립트
    public SharedData sharedData;

    private float BFdistance;
    private float UCdistance;
    private float EGdistance;
    private float interactDistance = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        init();

        chestAnimator = upgeradeChest.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void Update()
    {
        interactBonFire();
        interactCheset();
        interactGameExit();
    }

    private void init()
    {
        sharedData.coin_Temp = 0;
    }

    private void interactBonFire()
    {
        BFdistance = Vector3.Distance(Player.transform.position, bonFire.transform.position);
        
        if(BFdistance < interactDistance)
        {
            ui_nextWave.SetActive(true);
            bonFireLight.SetActive(true);
            bonFireParticle.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {//ui뜬 상태에서 E키 누르면 상호작용.
                StartCoroutine(fadeOut());
            }
        }
        else if(BFdistance >= interactDistance)
        {
            ui_nextWave.SetActive(false);
            bonFireLight.SetActive(false);
            bonFireParticle.SetActive(false);

            
        }
        
    }

    private IEnumerator fadeOut()
    {
        //audioSource.Play();
        blackOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("WaveScene");
    }


    private void interactCheset()
    {
        UCdistance = Vector3.Distance(Player.transform.position, upgeradeChest.transform.position);

        if (UCdistance < interactDistance)
        {
            ui_upgrade.SetActive(true);
            chestAnimator.SetTrigger("isOpen");
            if (Input.GetKeyDown(KeyCode.E))
            {//ui뜬 상태에서 E키 누르면 상호작용.
                //일시정지
                ui_interactUpgrade.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else if (UCdistance >= interactDistance)
        {
            ui_upgrade.SetActive(false);
            chestAnimator.SetTrigger("isClose");
            ui_interactUpgrade.SetActive(false);

        }

    }
    public void upgradeExit()
    {
        ui_interactUpgrade.SetActive(false);
        Time.timeScale = 1.0f;
        //x모양 버튼 누르면 일시정지 해제 및 ui종료
    }

    private void interactGameExit()
    {
        EGdistance = Vector3.Distance(Player.transform.position, gameExit.transform.position);

        if (EGdistance < interactDistance)
        {
            ui_exitGame.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {//ui뜬 상태에서 E키 누르면 상호작용.
                SceneManager.LoadScene("StartScene");
            }
        }
        else if (EGdistance >= interactDistance)
        {
            ui_exitGame.SetActive(false);
        }

    }
}
