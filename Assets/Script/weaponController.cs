using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour
{
    public GameObject player;
    private Animator playerAnimator;
    private AudioSource audioSource;
    public SharedData sharedData;

    public float attackForce = 100f; // ���ݷ� �⺻���.(�⺻��+�߰���)*���ݹ�� = �������ݷ�
    public float knockbackForce = 5000f; // �˹��Ű�� ��. ��� �� ����. �� rigid ����60����.
    public bool isAttacking = false;
    public float weaponDamage;

    //ī�޶� ��鸲ȿ��
    public Camera cam;
    public float shakeTime = 0.25f;
    private float timer = 0f;
    public float shakeIntensity = 25f;

    public GameObject particleEffectPrefab; // ��Ʈ����Ʈ.

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
            {//�������̰� && ���ݾִϸ��̼��� 60% ���±����� ��������.
                isAttacking = true;
                Debug.Log("attacked");
                shakeCamera(shakeTime, shakeIntensity);
                audioSource.Play();

                Vector3 knockbackDirection = player.transform.position - collision.transform.position;
                //knockbackDirection.y = player.transform.position.y + 0.1f; // y �� ���� 0���� ����

                // ��ƼŬ ����Ʈ �ߵ�
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
    }
}
