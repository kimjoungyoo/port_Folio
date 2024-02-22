using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�

    //�÷��̾���� ���
    private Animator playerAnimator;
    private Rigidbody rb;

    public SharedData sharedData;

    //�ʿ��� ������Ʈ��
    public GameObject weapon;
    public Slider hpBar;
    public GameObject blackOut;

    //�÷��̾� ���ȿ� ���� ����.
    public float maxHp;
    public float currentHp;
    public float attackSpeed = 1.5f;//���� �⺻������ 1.0f. 4.0f������ �Ѱ��� ������.
    public float armor = 1.0f; //����, 1.0f�� �⺻���� ���������� �޴� �������� ������.

    //��ų_explosion
    public GameObject skillExplosionPrefab;
    public Transform skillExplosionPos;
    private bool isExplosionActivated = false;

    public float explosionCoolTime = 6.0f;
    public bool isExplosionCoolTime = false;

    //��ų_tunrUndead
    public GameObject skillturnUndeadPrefab;
    public Transform skillturnUndeadPos;
    private bool isturnUndeadActivated = false;

    public float turnUndeadCoolTime = 16.0f;
    public bool isturnUndeadCoolTime = false;

    //�������� �ƿ��
    public GameObject healAura;

    //private bool isMove;
    private bool isDead = false;

    //���ΰ�
    private AudioSource audioCoin;
    public AudioClip coinClip;

    private void Start()
    {
        attackSpeed = attackSpeed * sharedData.attackSpeedRatio;
        sharedData.maxHp = 1000f + sharedData.level * 50;
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        //������ٵ�� �ִϸ����� ������.
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
        playerHP();//ü��
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
        
        //����������.
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
        //�÷��̾� �ӵ�
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
            // �̹� ��ų�� �ߵ� ���� ���� ����
            if (isExplosionActivated)
                return;

            // �ִϸ��̼� �ߵ�
            playerAnimator.SetTrigger("isSkill");
            isExplosionActivated = true;

            // �ִϸ��̼��� ���̸�ŭ ��ٸ� �� ��ų �ߵ�
            StartCoroutine(TriggerSkillExplosion(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            explosionCoolTime = 6.0f;
        }
    }

    IEnumerator TriggerSkillExplosion(float delay)
    {
        if (skillExplosionPrefab == null)
        {
            Debug.LogError("skillExplosion �������� �Ҵ���� �ʾҽ��ϴ�.");
            yield break;
        }
            
        yield return new WaitForSeconds(delay);

        // ��ų ���� ����
        Instantiate(skillExplosionPrefab, skillExplosionPos.position, skillExplosionPos.rotation);

        isExplosionActivated = false;
        isExplosionCoolTime = true;        
    }

    public void Skill_TurnUndead() {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isturnUndeadCoolTime)
                return;
            // �̹� ��ų�� �ߵ� ���� ���� ����
            if (isturnUndeadActivated)
                return;

            // �ִϸ��̼� �ߵ�
            playerAnimator.SetTrigger("isBuff");
            isturnUndeadActivated = true;

            // �ִϸ��̼��� ���̸�ŭ ��ٸ� �� ��ų �ߵ�
            StartCoroutine(TriggerSkillTurnUndead(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            turnUndeadCoolTime = 16.0f;
        }
    }

    IEnumerator TriggerSkillTurnUndead(float delay)
    {
        if (skillturnUndeadPrefab == null)
        {
            Debug.LogError("turnUndead �������� �Ҵ���� �ʾҽ��ϴ�.");
            yield break;
        }

        yield return new WaitForSeconds(delay);

        // ��ų ���� ����
        Instantiate(skillturnUndeadPrefab, skillturnUndeadPos.position, skillturnUndeadPos.rotation);

        isturnUndeadActivated = false;
        isturnUndeadCoolTime = true;
    }

    private void PlayerCoinGet()
    {
        audioCoin.PlayOneShot(coinClip);
    }//����ȹ��Ҹ�
    private void OnTriggerEnter(Collider coin)
    {
        //coin�� ȹ���Ҷ�
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
    }//����ȹ��.

    private void OnTriggerStay(Collider other)
    {
        //�ǰ� �ִ�ü���� �ƴϸ� �Ͼ𵥵� �����ǿ��� ��
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
        
    }//������ ȸ��

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("skillTurnUndead"))
            healAura.SetActive(false);
    }


}



