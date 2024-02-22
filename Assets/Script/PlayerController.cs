using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도

    //플레이어관련 기능
    private Animator playerAnimator;
    private Rigidbody rb;

    public SharedData sharedData;

    //필요한 오브젝트들
    public GameObject weapon;
    public Slider hpBar;
    public GameObject blackOut;

    //플레이어 스탯에 따른 변경.
    public float maxHp;
    public float currentHp;
    public float attackSpeed = 1.5f;//기사는 기본적으로 1.0f. 4.0f까지가 한계라고 생각함.
    public float armor = 1.0f; //방어력, 1.0f가 기본으로 낮아질수록 받는 데미지가 낮아짐.

    //스킬_explosion
    public GameObject skillExplosionPrefab;
    public Transform skillExplosionPos;
    private bool isExplosionActivated = false;

    public float explosionCoolTime = 6.0f;
    public bool isExplosionCoolTime = false;

    //스킬_tunrUndead
    public GameObject skillturnUndeadPrefab;
    public Transform skillturnUndeadPos;
    private bool isturnUndeadActivated = false;

    public float turnUndeadCoolTime = 16.0f;
    public bool isturnUndeadCoolTime = false;

    //버프상태 아우라
    public GameObject healAura;

    //private bool isMove;
    private bool isDead = false;

    //코인겟
    private AudioSource audioCoin;
    public AudioClip coinClip;

    private void Start()
    {
        attackSpeed = attackSpeed * sharedData.attackSpeedRatio;
        sharedData.maxHp = 1000f + sharedData.level * 50;
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        //리지드바디와 애니메이터 가져옴.
        audioCoin = gameObject.GetComponent<AudioSource>();
        moveSpeed *= sharedData.moveSpeedRatio;
    }

    private void Update()
    {
        if (!isDead)
        {
            PlayerMove();
            PlayerAttack();

            skillCoolTime();

            Skill_Explosion();
            Skill_TurnUndead();
        }        
        playerHP();//체력
        PlayerDead();        
    }

    private void playerHP()
    {
        currentHp = sharedData.currentHp;       
        maxHp = sharedData.maxHp;
        hpBar.value = currentHp / maxHp;
    }

    private void PlayerAttack()
    {
        
        //계수적용받음.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetFloat("attackSpeed", attackSpeed);
            playerAnimator.SetTrigger("isAttack");           
        }
    }

    private void PlayerMove()
    {
        float h = -Input.GetAxis("Vertical");
        float v = Input.GetAxis("Horizontal");


        Vector3 moveHorizontal = Vector3.right * h;
        Vector3 moveVertical = Vector3.forward * v;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized;

        transform.LookAt(transform.position + velocity);

        transform.Translate(velocity * moveSpeed * Time.deltaTime, Space.World);

        //playerAnimator.SetBool("isMove", true);
        playerAnimator.SetFloat("moveSpeed", velocity.magnitude);
        //플레이어 속도
    }

    private void PlayerDead()
    {
        if(currentHp <= 0)
        {
            isDead = true;
            playerAnimator.SetTrigger("isDead");
            StartCoroutine(fadeOut());
        }
    }
    private IEnumerator fadeOut()
    {
        //audioSource.Play();        
        yield return new WaitForSeconds(2.0f);
        blackOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("StartScene");
    }

    private void skillCoolTime()
    {
        if (isExplosionCoolTime)
        {
            if(explosionCoolTime > 0)
            {
                explosionCoolTime -= Time.deltaTime;

            }else if(explosionCoolTime <= 0)
            {
                isExplosionCoolTime = false;
            }
        }

        if (isturnUndeadCoolTime)
        {
            if(turnUndeadCoolTime > 0)
            {
                turnUndeadCoolTime -= Time.deltaTime;
            }else if(turnUndeadCoolTime <= 0)
            {
                isturnUndeadCoolTime = false;
            }
        }
    }

    public void Skill_Explosion(){
        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isExplosionCoolTime)
                return;
            // 이미 스킬이 발동 중인 경우는 무시
            if (isExplosionActivated)
                return;

            // 애니메이션 발동
            playerAnimator.SetTrigger("isSkill");
            isExplosionActivated = true;

            // 애니메이션의 길이만큼 기다린 후 스킬 발동
            StartCoroutine(TriggerSkillExplosion(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            explosionCoolTime = 6.0f;
        }
    }

    IEnumerator TriggerSkillExplosion(float delay)
    {
        if (skillExplosionPrefab == null)
        {
            Debug.LogError("skillExplosion 프리팹이 할당되지 않았습니다.");
            yield break;
        }
            
        yield return new WaitForSeconds(delay);

        // 스킬 폭발 생성
        Instantiate(skillExplosionPrefab, skillExplosionPos.position, skillExplosionPos.rotation);

        isExplosionActivated = false;
        isExplosionCoolTime = true;        
    }

    public void Skill_TurnUndead() {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isturnUndeadCoolTime)
                return;
            // 이미 스킬이 발동 중인 경우는 무시
            if (isturnUndeadActivated)
                return;

            // 애니메이션 발동
            playerAnimator.SetTrigger("isBuff");
            isturnUndeadActivated = true;

            // 애니메이션의 길이만큼 기다린 후 스킬 발동
            StartCoroutine(TriggerSkillTurnUndead(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            turnUndeadCoolTime = 16.0f;
        }
    }

    IEnumerator TriggerSkillTurnUndead(float delay)
    {
        if (skillturnUndeadPrefab == null)
        {
            Debug.LogError("turnUndead 프리팹이 할당되지 않았습니다.");
            yield break;
        }

        yield return new WaitForSeconds(delay);

        // 스킬 폭발 생성
        Instantiate(skillturnUndeadPrefab, skillturnUndeadPos.position, skillturnUndeadPos.rotation);

        isturnUndeadActivated = false;
        isturnUndeadCoolTime = true;
    }

    private void PlayerCoinGet()
    {
        audioCoin.PlayOneShot(coinClip);
    }//코인획득소리
    private void OnTriggerEnter(Collider coin)
    {
        //coin을 획득할때
        if (coin.gameObject.CompareTag("coinGold"))
        {
            Debug.Log("gold");
            Destroy(coin.gameObject);
            sharedData.coin_Temp += 10;
            PlayerCoinGet();
        }

        if (coin.gameObject.CompareTag("coinSilver"))
        {
            Debug.Log("silver");
            Destroy(coin.gameObject);
            sharedData.coin_Temp += 5;
            PlayerCoinGet();
        }

        if (coin.gameObject.CompareTag("coinBronze"))
        {
            Debug.Log("silver");
            Destroy(coin.gameObject);
            sharedData.coin_Temp += 1;
            PlayerCoinGet();
        }
    }//코인획득.

    private void OnTriggerStay(Collider other)
    {
        //피가 최대체력이 아니면 턴언데드 힐장판에서 힐
        if (other.gameObject.CompareTag("skillTurnUndead"))
        {
            if (sharedData.currentHp < sharedData.maxHp)
            {
                sharedData.currentHp += Time.deltaTime * 25f;
                healAura.SetActive(true);
            }
            else if(sharedData.currentHp >= sharedData.maxHp)
            {
                healAura.SetActive(false);
            }
        }
        
    }//힐장판 회복

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("skillTurnUndead"))
            healAura.SetActive(false);
    }


}



