using UnityEngine;

public class BossMoveAI : MonoBehaviour
{
    // --- ���� �̵� ���� ���� ---
    public float moveSpeed = 3f; // ���� �̵� �ӵ�
    public float moveInterval = 2f; // ���� �̵� ��ǥ ���� ���� ���� (��)

    // ������ ������ �� �ִ� ���� ��ǥ ���� (Inspector���� ����)
    [Header("Movement Bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = 0f; // �Ϲ������� �÷��̾� ���� ��ġ�ϵ��� ����
    public float maxY = 5f;

    private Vector3 targetPosition; // ������ ���� ��ǥ �̵� ����
    private float nextMoveTime;     // ���� ��ǥ ���� ���� ���� �ð�

    void Start()
    {
        // ���� ���� �� ù ��° �̵� ��ǥ ���� ����
        SetNewTargetPosition();
        // ù �̵� ��ǥ ���� �ð� �ʱ�ȭ
        nextMoveTime = Time.time + moveInterval;
    }

    void Update()
    {
        // 'moveInterval' �ð��� �������� ���ο� ��ǥ ���� ����
        if (Time.time >= nextMoveTime)
        {
            SetNewTargetPosition(); // ���ο� ��ǥ ���� ���
            nextMoveTime = Time.time + moveInterval; // ���� ��ǥ ���� �ð� ����
        }

        // ���� ��ġ���� ��ǥ ������ ���� �ε巴�� �̵�
        // Vector3.MoveTowards�� �� ���� ���̸� ������ �ӵ��� �̵���ų �� �����մϴ�.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // ���ο� ���� �̵� ��ǥ ���� ���� �Լ�
    void SetNewTargetPosition()
    {
        // ������ �ּ�/�ִ� X, Y ���� ������ ������ ��ġ�� ����
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // ���õ� ���� X, Y�� ���� Z���� ����Ͽ� ���ο� ��ǥ ��ġ ����
        targetPosition = new Vector3(randomX, randomY, transform.position.z);

        Debug.Log("������ ���ο� ��ǥ ����: " + targetPosition);
    }
}