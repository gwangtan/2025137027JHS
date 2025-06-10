// ProjectileTracker.cs (Ȥ�� ProjectileCollisionHandler ��)
using UnityEngine;

public class ProjectileTracker : MonoBehaviour
{
    public int damage = 5;

    // �� ��ũ��Ʈ������ Update���� �������� �������� �ʽ��ϴ�.
    // �������� �߻��ϴ� ��ũ��Ʈ(BossAttackAI)���� Rigidbody2D.velocity�� ����˴ϴ�.
    // void Update() {} // �� �κ��� ����ΰų� �����մϴ�.

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("End"))
        {
            Destroy(gameObject);
        }
        // ... (������ ���� �� �ʿ��ϸ� �߰�)
    }
}