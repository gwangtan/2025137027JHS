using UnityEngine;

public class MoveObjectXPositive2D : MonoBehaviour
{
    public float moveSpeed = 1f; // �̵� �ӵ� (�⺻��: 1)

    void Update()
    {
        // 2D ȯ�濡���� Vector2�� ����Ͽ� �̵��մϴ�.
        // Time.deltaTime�� ���Ͽ� ������ �ӵ��� �������� �������� ����ϴ�.
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}