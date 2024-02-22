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

    //필요한 스크립트
    public SharedData sharedData;
    public PlayerController playerController;
    public waveController waveController;

    public float enemyHP = 150f;
    public float enemyArmor = 1.0f;
    public float enemyAttackForce = 50f;

    //공격관련
    private bool isAttacking = false; // 현재 공격 중인지 여부
    private bool attackSuccess = false;//공격을 성공했는지, 1회만 닳도록
    private float attackRange = 3.3f;
    private Animator playerAnimator;

    //몬스터 피격 관련
    private weaponController weaponController;
    private bool isDead = false;
    private bool isExplosionHit = false;
    public float playerDamage;
    public GameObject turnUndeadDebuff;

    //카메라 흔들림효과
    public Camera cam;
    public float shakeTime = 0.25f;
    private float timer = 0f;
    public float shakeIntensity = 25f;
    public float distanceToPlayer;

    //태양각도조절
    public GameObject sun;
    public float sunAngle;//-120 -> 0이 될수있도록 1킬당 움직이는 각도.
    private Vector3 sunRotation;
    private Light sunLight;
    private float sunLightIntensity;

    //코인드랍
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
        //플레이어에게 영향줄 수 있도록 스크립트 불러오기
        playerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();

        waveController = ctrlWave.GetComponent<waveController>();

        //피격효과용 카메라
        cam = Camera.main;

        //태양
        sun = GameObject.FindGameObjectWithTag("sun");
        sunLight = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
        sunAngle = 120 / sharedData.enemyLeft;
        sunLightIntensity = 4.0f / sharedData.enemyLeft;

        //피격당할때 매번 무기컨트롤러를 불러오지 않기위해 초기화.
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
            // 공격 애니메이션 작동
            enemyAnimator.SetTrigger("isAttack");
            isAttacking = true;
        }
        else
        {
            if (!isDead)
            {
                agent.SetDestination(player.transform.position);
                if (enemyHP <= 0)
                {//불 판정으로 킬수가 1만 오르게 함.
                    enemyDestroy();
                    coinDrop();
                }
            }         
        }

        if (!isDead && isAttacking)
        {           
            AnimatorStateInfo currentState = enemyAnimator.GetCurrentAnimatorStateInfo(0);
            if (!attackSuccess && currentState.IsName("Attack") && (currentState.normalizedTime >= 0.5f && currentState.normalizedTime <= 0.75f) && distanceToPlayer <= attackRange)
            {//50%~75%사이에 손 휘두를 때만 공격판정. 그 때 가까우면 맞음.

                playerAnimator.SetTrigger("isDamaged");
                sharedData.currentHp -= enemyAttackForce;//공유데이터에서 변경함.
                attackSuccess = true;
                shakeIntensity = 25f;
                StartCoroutine(ShakeCamPos());

            }else if (currentState.IsName("Attack") && currentState.normalizedTime > 0.75f)
            {
                //currentState.normalizedTime >= 1.0f에서 지금으로 바꾸고 돌아감.
                //Debug.Log("attackDone");
                // Attack 애니메이션이 종료됨
                isAttacking = false;
                attackSuccess = false;
            }
        }
    }//공격

    private void enemyDestroy()
    {
        enemyAnimator.SetBool("isDead", true);
        isDead = true;
        Debug.Log("killed");
        waveController.waveKillLeft--;//shareDate가 아닌 waveController에서 다룸.
        sunRising();
        
        Destroy(gameObject, 1.5f);
    }//죽음

    private void coinDrop()
    {
        float randomValue = Random.Range(0f, 1f);
        GameObject coinPrefab = (randomValue <= 0.5f) ? coinBronze : (randomValue <= 0.8f) ? coinSilver : coinGold;
        //이후엔 playerController에서 다룸.
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
    {//3배딜

        if (!isExplosionHit)
        {
            Debug.Log("exlopsionDamage");
            isExplosionHit = true;
            enemyHP -= 3f * playerDamage;
            shakeIntensity = 200f;
            StartCoroutine(ShakeCamPos());

            // 플레이어와 몬스터 사이의 벡터
            Vector3 directionToPlayer = (transform.position - player.transform.position).normalized;

            // 플레이어의 반대 방향으로 힘을 가할 벡터 계산
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
        Vector3 currentPos = cam.transform.position;//초기값
        while (shakeTime > timer)
        {
            float x = Random.Range(-0.05f, 0.05f);
            float z = Random.Range(-0.05f, 0.05f);
            cam.transform.position = currentPos + new Vector3(x, 0, z) * shakeIntensity;

            //transform.position = currentPos + Random.insideUnitSphere * shakeIntensity;
            //발작이 심해서 버림.
            timer += Time.deltaTime;

            yield return null;
            //딱히 리턴하지않음.
        }
        timer = 0f;

        cam.transform.position = currentPos;
        //흔들고 나서 원래 포지션으로 귀환
    }//피격 카메라 흔들림

    private void sunRising()
    {
        //Debug.Log("sunUp");
        sunRotation = sun.transform.localEulerAngles;//태양각도
        sun.transform.localEulerAngles = new Vector3((240f+((sharedData.enemyLeft-waveController.waveKillLeft)*sunAngle)), sunRotation.y, sunRotation.z);
        sunLight.intensity += sunLightIntensity;
    }
}

