using UnityEngine;

public class Mover : MonoBehaviour
{
    // �ν����Ϳ��� ���� ������ �̵� �ӵ� ����
    public float moveSpeed = 10f; // ����ü �ӵ�

    // ����ü ������. �� ���� �÷��̾��� ����ü�� �����ϴ� ��ũ��Ʈ���� ������ �����Դϴ�.
    // ���� ���, �÷��̾��� ���� ��ũ��Ʈ���� ������ ���� ������ �� �ֽ��ϴ�:
    // Mover newProjectile = Instantiate(projectilePrefab).GetComponent<Mover>();
    // newProjectile.damage = PlayerStatsManager.Instance.currentProjectileDamage;
    public int damage;

    void Update()
    {
        // Y�� ���� �������� ��� �̵�
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // ���� ����: ���� �ð� �� �ڵ� �ı�
        // Destroy(gameObject, 5f);
    }

    // �ٸ� 2D �ݶ��̴��� �浹(Ʈ����)���� �� ȣ��Ǵ� �Լ�
    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� �±װ� "End"���� Ȯ��
        if (other.CompareTag("End"))
        {
            Debug.Log("Mover�� 'End' �±� ������Ʈ�� �浹!");
            // Mover(����ü) �ڽ��� �ı��մϴ�.
            Destroy(gameObject);
        }

        // �浹�� ������Ʈ�� �������� Ȯ�� (�������� "Boss" �±׸� �ο��ؾ� �մϴ�.)
        if (other.CompareTag("Boss"))
        {
            // �浹�� ������Ʈ���� BossHealth ��ũ��Ʈ�� �����ɴϴ�.
            BossHealth bossHealth = other.GetComponent<BossHealth>();

            // BossHealth ��ũ��Ʈ�� �ִٸ� �������� �ݴϴ�.
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage); // Mover�� damage ���� ����
            }

            // ����ü�� �������� ������ ��������� �մϴ�.
            Destroy(gameObject);
        }
    }
}
