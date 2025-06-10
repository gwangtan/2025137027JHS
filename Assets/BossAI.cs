using UnityEngine;

public class BossAI : MonoBehaviour
{
    // ����ü �ӵ� (���� �ν����Ϳ��� ���� �����ϰų�, BossAI���� ���ϵ� ���� �� �� �ֽ��ϴ�.)
    public float moveSpeed = 3f; // �⺻ �ӵ� ���� (���ϴ� ������ ����)

    // �ܺο��� ������ �̵� ���� ���� (���� ���⸸ ���Թ޽��ϴ�)
    private Vector3 moveDirection;

    public int damage = 5;

    // ����ü ���� �� ȣ��Ǿ� ���⸸ �����޴� �޼���
    public void SetMovement(Vector3 direction) // �ӵ� �Ű����� ����
    {
        moveDirection = direction.normalized; // ���� ���͸� ����ȭ�Ͽ� ������ ũ��� ����ϴ�.
        // currentMoveSpeed�� ���� ������� �ʰ�, moveSpeed�� ���� ����մϴ�.
    }

    void Update()
    {
        // ������ ����� ������ moveSpeed�� �̵�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // ���� ����: ���� �ð� �� �ڵ� �ı� (ȭ�� ������ ������ �ʴ� ��� ���)
        // Destroy(gameObject, 5f); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("End"))
        {
            Debug.Log("Mover�� 'End' �±� ������Ʈ�� �浹! ����ü �ı�.");
            Destroy(gameObject);
        }

        // �÷��̾� ������ ���� �ڵ�� ������ �ӽ� ��Ȱ��ȭ�߽��ϴ�.
        // ���߿� �÷��̾� ü�� �ý����� ������ �� �� �κ��� �ٽ� Ȱ��ȭ�ϼ���.
        /*
        if (other.CompareTag("Player")) 
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); 
            }
            Destroy(gameObject);
        }
        */
    }
}