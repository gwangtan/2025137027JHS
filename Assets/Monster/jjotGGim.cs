using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public Transform target; // 목표 지점 (플레이어)
    public float moveSpeed = 3f; // 이동 속도
    public float nextWaypointDistance = 3f; // 다음 웨이포인트까지의 거리
    public LayerMask obstacleLayer; // 장애물 레이어

    private Vector2[] path; // 경로
    private int currentWaypoint = 0; // 현재 웨이포인트

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if (path == null || path.Length == 0) return;

        // 현재 웨이포인트에 도달했는지 확인
        if (Vector2.Distance(transform.position, path[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= path.Length)
            {
                return;
            }
        }

        // 다음 웨이포인트로 이동
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
            yield return new WaitForSeconds(0.5f); // 경로 업데이트 간격
        }
    }

    Vector2[] CalculatePath(Vector2 startPos, Vector2 targetPos)
    {
        // 간단한 경로 계산 알고리즘 (직선 경로)
        return new Vector2[] { targetPos };
    }
}