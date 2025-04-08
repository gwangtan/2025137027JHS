using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public Transform target; // ��ǥ ���� (�÷��̾�)
    public float moveSpeed = 3f; // �̵� �ӵ�
    public float nextWaypointDistance = 3f; // ���� ��������Ʈ������ �Ÿ�
    public LayerMask obstacleLayer; // ��ֹ� ���̾�

    private Vector2[] path; // ���
    private int currentWaypoint = 0; // ���� ��������Ʈ

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if (path == null || path.Length == 0) return;

        // ���� ��������Ʈ�� �����ߴ��� Ȯ��
        if (Vector2.Distance(transform.position, path[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= path.Length)
            {
                return;
            }
        }

        // ���� ��������Ʈ�� �̵�
        Vector2 direction = (path[currentWaypoint] - (Vector2)transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    System.Collections.IEnumerator UpdatePath()
    {
        while (true)
        {
            if (target != null)
            {
                path = CalculatePath((Vector2)transform.position, (Vector2)target.position);
                currentWaypoint = 0;
            }
            yield return new WaitForSeconds(0.5f); // ��� ������Ʈ ����
        }
    }

    Vector2[] CalculatePath(Vector2 startPos, Vector2 targetPos)
    {
        // ������ ��� ��� �˰��� (���� ���)
        return new Vector2[] { targetPos };
    }
}