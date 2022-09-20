using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public GameObject Target; // �ٶ� ������Ʈ�Դϴ�.
    public float xmove = 0;  // X�� ���� �̵���
    public float ymove = 0;  // Y�� ���� �̵���
    public float cameraRotSpeed = 10f; // ī�޶� ȸ���ӵ�
    public float distance = 3; // ī�޶� �Ÿ�

    // Update is called once per frame
    void Update()
    {
        xmove += Input.GetAxis("Mouse X"); // ���콺�� �¿� �̵����� xmove �� �����մϴ�.
        ymove -= Input.GetAxis("Mouse Y"); // ���콺�� ���� �̵����� ymove �� �����մϴ�.
        transform.rotation = Quaternion.Euler(ymove, xmove, 0); // �̵����� ���� ī�޶��� �ٶ󺸴� ������ �����մϴ�.
        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵����� ���� Z ������� ���͸� ���մϴ�.
        transform.position = Target.transform.position - transform.rotation * reverseDistance; // Ÿ���� ��ġ���� ī�޶� �ٶ󺸴� ���⿡ ���Ͱ��� ������ ��� ��ǥ�� �����մϴ�.
    }
}
