using UnityEngine;

public class BossMoveAI : MonoBehaviour
{
    // --- 보스 이동 관련 설정 ---
    public float moveSpeed = 3f; // 보스 이동 속도
    public float moveInterval = 2f; // 다음 이동 목표 지점 설정 간격 (초)

    // 보스가 움직일 수 있는 월드 좌표 범위 (Inspector에서 설정)
    [Header("Movement Bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = 0f; // 일반적으로 플레이어 위에 위치하도록 설정
    public float maxY = 5f;

    private Vector3 targetPosition; // 보스의 다음 목표 이동 지점
    private float nextMoveTime;     // 다음 목표 지점 설정 가능 시간

    void Start()
    {
        // 게임 시작 시 첫 번째 이동 목표 지점 설정
        SetNewTargetPosition();
        // 첫 이동 목표 설정 시간 초기화
        nextMoveTime = Time.time + moveInterval;
    }

    void Update()
    {
        // 'moveInterval' 시간이 지났으면 새로운 목표 지점 설정
        if (Time.time >= nextMoveTime)
        {
            SetNewTargetPosition(); // 새로운 목표 지점 계산
            nextMoveTime = Time.time + moveInterval; // 다음 목표 설정 시간 갱신
        }

        // 현재 위치에서 목표 지점을 향해 부드럽게 이동
        // Vector3.MoveTowards는 두 지점 사이를 일정한 속도로 이동시킬 때 유용합니다.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // 새로운 랜덤 이동 목표 지점 설정 함수
    void SetNewTargetPosition()
    {
        // 설정된 최소/최대 X, Y 범위 내에서 랜덤한 위치를 선택
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // 선택된 랜덤 X, Y와 현재 Z값을 사용하여 새로운 목표 위치 설정
        targetPosition = new Vector3(randomX, randomY, transform.position.z);

        Debug.Log("보스의 새로운 목표 지점: " + targetPosition);
    }
}