using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour
{
    public GameObject player;
    private Animator playerAnimator;
    private AudioSource audioSource;
    public SharedData sharedData;

    public float attackForce = 100f; // 공격력 기본계수.(기본공+추가공)*공격배수 = 최종공격력
    public float knockbackForce = 5000f; // 넉백시키는 힘. 기사 검 기준. 적 rigid 무게60기준.
    public bool isAttacking = false;
    public float weaponDamage;

    //카메라 흔들림효과
    public Camera cam;
    public float shakeTime = 0.25f;
    private float timer = 0f;
    public float shakeIntensity = 25f;

    public GameObject particleEffectPrefab; // 히트이펙트.

    // Start is called before the first frame update
    void Start()
    {
        sharedData = player.GetComponent<SharedData>();
        playerAnimator = player.GetComponent<Animator>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        weaponDamage = attackForce * sharedData.attackRatio * sharedData.attackForceRatio;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))           
        {
            AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Attack") && currentState.normalizedTime <= 0.6f)
            {//공격중이고 && 공격애니메이션의 60% 상태까지만 공격판정.
                isAttacking = true;
                Debug.Log("attacked");
                shakeCamera(shakeTime, shakeIntensity);
                audioSource.Play();

                Vector3 knockbackDirection = player.transform.position - collision.transform.position;
                //knockbackDirection.y = player.transform.position.y + 0.1f; // y 축 값을 0으로 설정

                // 파티클 이펙트 발동
                Instantiate(particleEffectPrefab, collision.transform.position, Quaternion.identity);

                Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                enemyRigidbody.AddForce(-knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
            }else if(currentState.normalizedTime >= 1f)
            {
                isAttacking = false;
            }
        }
            
    }

    void shakeCamera(float shakeTime, float shakeIntensity)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeCamPos");
        StartCoroutine("ShakeCamPos");
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
    }
}
