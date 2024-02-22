using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform ������Ʈ

    public float smoothSpeed = 0.125f; // ȸ�� ������ ����

    public Vector3 offset; // ī�޶�� �÷��̾� ������ ������

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // �÷��̾� ��ġ�� �������� ���� ��ǥ ��ġ

        // �ε巯�� ȸ���� ���� Lerp ���
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(0f, target.rotation.eulerAngles.y, 0f); // y�� ȸ���� ���

        transform.LookAt(target); // �÷��̾ �ٶ󺸵��� ī�޶� ȸ��
    }
}
