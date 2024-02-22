using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySkeletonController : MonoBehaviour
{
    public GameObject player;
    private GameObject ctrlWave;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator enemyAnimator;
    public GameObject weapon;
    private Rigidbody rb;

    //�ʿ��� ��ũ��Ʈ
    public SharedData sharedData;
    public PlayerController playerController;
    public waveController waveController;

    public float enemyHP = 150f;
    public float enemyArmor = 1.0f;
    public float enemyAttackForce = 50f;

    //���ݰ���
    private bool isAttacking = false; // ���� ���� ������ ����
    private bool attackSuccess = false;//������ �����ߴ���, 1ȸ�� �⵵��
    private float attackRange = 3.3f;
    private Animator playerAnimator;

    //���� �ǰ� ����
    private weaponController weaponController;
    private bool isDead = false;
    private bool isExplosionHit = false;
    public float playerDamage;
    public GameObject turnUndeadDebuff;

    //ī�޶� ��鸲ȿ��
    public Camera cam;
    public float shakeTime = 0.25f;
    private float timer = 0f;
    public float shakeIntensity = 25f;
    public float distanceToPlayer;

    //�¾簢������
    public GameObject sun;
    public float sunAngle;//-120 -> 0�� �ɼ��ֵ��� 1ų�� �����̴� ����.
    private Vector3 sunRotation;
    private Light sunLight;
    private float sunLightIntensity;

    //���ε��
    public GameObject coinGold;
    public GameObject coinSilver;
    public GameObject coinBronze;

    private void Start()
    {
        enemyHP = 100f+sharedData.waveLevel*40;
        enemyAttackForce = 50f + sharedData.waveLevel*5;

        enemyAnimator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        ctrlWave = GameObject.FindGameObjectWithTag("WaveController");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = this.gameObject.GetComponent<Rigidbody>(); 
        //�÷��̾�� ������ �� �ֵ��� ��ũ��Ʈ �ҷ�����
        playerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();

        waveController = ctrlWave.GetComponent<waveController>();

        //�ǰ�ȿ���� ī�޶�
        cam = Camera.main;

        //�¾�
        sun = GameObject.FindGameObjectWithTag("sun");
        sunLight = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
        sunAngle = 120 / sharedData.enemyLeft;
        sunLightIntensity = 4.0f / sharedData.enemyLeft;

        //�ǰݴ��Ҷ� �Ź� ������Ʈ�ѷ��� �ҷ����� �ʱ����� �ʱ�ȭ.
        weapon = GameObject.FindWithTag("Weapon");
        weaponController = weapon.GetComponent<weaponController>();
        playerDamage = weaponController.weaponDamage;
    }

    private void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        enemyAttack();

           
    }

    private void enemyAttack()
    {
        if (!isDead && distanceToPlayer <= attackRange && !isAttacking)
        {
            //Debug.Log("attackTry");
            // ���� �ִϸ��̼� �۵�
            enemyAnimator.SetTrigger("isAttack");
            isAttacking = true;
        }
        else
        {
            if (!isDead)
            {
                agent.SetDestination(player.transform.position);
                if (enemyHP <= 0)
                {//�� �������� ų���� 1�� ������ ��.
                    enemyDestroy();
                    coinDrop();
                }
            }         
        }

        if (!isDead && isAttacking)
        {           
            AnimatorStateInfo currentState = enemyAnimator.GetCurrentAnimatorStateInfo(0);
            if (!attackSuccess && currentState.IsName("Attack") && (currentState.normalizedTime >= 0.5f && currentState.normalizedTime <= 0.75f) && distanceToPlayer <= attackRange)
            {//50%~75%���̿� �� �ֵθ� ���� ��������. �� �� ������ ����.

                playerAnimator.SetTrigger("isDamaged");
                sharedData.currentHp -= enemyAttackForce;//���������Ϳ��� ������.
                attackSuccess = true;
                shakeIntensity = 25f;
                StartCoroutine(ShakeCamPos());

            }else if (currentState.IsName("Attack") && currentState.normalizedTime > 0.75f)
            {
                //currentState.normalizedTime >= 1.0f���� �������� �ٲٰ� ���ư�.
                //Debug.Log("attackDone");
                // Attack �ִϸ��̼��� �����
                isAttacking = false;
                attackSuccess = false;
            }
        }
    }//����

    private void enemyDestroy()
    {
        enemyAnimator.SetBool("isDead", true);
        isDead = true;
        Debug.Log("killed");
        waveController.waveKillLeft--;//shareDate�� �ƴ� waveController���� �ٷ�.
        sunRising();
        
        Destroy(gameObject, 1.5f);
    }//����

    private void coinDrop()
    {
        float randomValue = Random.Range(0f, 1f);
        GameObject coinPrefab = (randomValue <= 0.5f) ? coinBronze : (randomValue <= 0.8f) ? coinSilver : coinGold;
        //���Ŀ� playerController���� �ٷ�.
        Instantiate(coinPrefab, this.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {           
            if (weaponController.isAttacking)
            {
                enemyHP -= playerDamage;
            }           
        }
        /*
        if (collision.gameObject.CompareTag("skillExplosion"))
        {
            explosionDamaged();
        }
        */
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("skillTurnUndead"))
        {
            turnUndeadDamaged();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("skillTurnUndead"))
        {
            turnUndeadDebuff.SetActive(false);
        }
    }

    private void explosionDamaged()
    {//3���

        if (!isExplosionHit)
        {
            Debug.Log("exlopsionDamage");
            isExplosionHit = true;
            enemyHP -= 3f * playerDamage;
            shakeIntensity = 200f;
            StartCoroutine(ShakeCamPos());

            // �÷��̾�� ���� ������ ����
            Vector3 directionToPlayer = (transform.position - player.transform.position).normalized;

            // �÷��̾��� �ݴ� �������� ���� ���� ���� ���
            Vector3 knockbackVector = directionToPlayer * (weaponController.knockbackForce*5);
            rb.AddForce(knockbackVector, ForceMode.Impulse);
        }
        isExplosionHit = false;
    }

    private void turnUndeadDamaged()
    {
        turnUndeadDebuff.SetActive(true);
        enemyHP -= Time.deltaTime * 40f;
    }

   
    private IEnumerator ShakeCamPos()
    {
        Vector3 currentPos = cam.transform.position;//�ʱⰪ
        while (shakeTime > timer)
        {
            float x = Random.Range(-0.05f, 0.05f);
            float z = Random.Range(-0.05f, 0.05f);
            cam.transform.position = currentPos + new Vector3(x, 0, z) * shakeIntensity;

            //transform.position = currentPos + Random.insideUnitSphere * shakeIntensity;
            //������ ���ؼ� ����.
            timer += Time.deltaTime;

            yield return null;
            //���� ������������.
        }
        timer = 0f;

        cam.transform.position = currentPos;
        //���� ���� ���� ���������� ��ȯ
    }//�ǰ� ī�޶� ��鸲

    private void sunRising()
    {
        //Debug.Log("sunUp");
        sunRotation = sun.transform.localEulerAngles;//�¾簢��
        sun.transform.localEulerAngles = new Vector3((240f+((sharedData.enemyLeft-waveController.waveKillLeft)*sunAngle)), sunRotation.y, sunRotation.z);
        sunLight.intensity += sunLightIntensity;
    }
}

