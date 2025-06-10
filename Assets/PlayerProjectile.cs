using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damageAmount = 10; // �� ����ü�� �������� �� ������

    // ����ü�� �̵� ���� (����)
    public float speed = 10f;
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); // ����: ���� �̵�
    }

    // ȭ�� ������ ������ �ı� (���� ����)
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}