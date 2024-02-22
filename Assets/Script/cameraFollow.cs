using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform 컴포넌트

    public float smoothSpeed = 0.125f; // 회전 스무딩 정도

    public Vector3 offset; // 카메라와 플레이어 사이의 오프셋

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // 플레이어 위치에 오프셋을 더한 목표 위치

        // 부드러운 회전을 위해 Lerp 사용
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(0f, target.rotation.eulerAngles.y, 0f); // y축 회전만 허용

        transform.LookAt(target); // 플레이어를 바라보도록 카메라 회전
    }
}
