using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float moveSpeed = 3f;  // ������ �̵� �ӵ�
    private Transform player;      // Player�� Transform ������Ʈ�� ������ ����

    void Start()
    {
        // Player �±׸� ���� ������Ʈ�� ã�Ƽ� player ������ �Ҵ�
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Player �������� ���͸� ���ϵ��� ȸ��
            Vector2 direction = (player.position - transform.position).normalized;

            // �̵�
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }


}
