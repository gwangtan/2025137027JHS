using UnityEngine;

public class BossAI : MonoBehaviour
{
    // 투사체 속도 (이제 인스펙터에서 직접 조절하거나, BossAI에서 통일된 값을 줄 수 있습니다.)
    public float moveSpeed = 3f; // 기본 속도 설정 (원하는 값으로 조절)

    // 외부에서 설정할 이동 방향 벡터 (이제 방향만 주입받습니다)
    private Vector3 moveDirection;

    public int damage = 5;

    // 투사체 생성 시 호출되어 방향만 설정받는 메서드
    public void SetMovement(Vector3 direction) // 속도 매개변수 제거
    {
        moveDirection = direction.normalized; // 방향 벡터를 정규화하여 일정한 크기로 만듭니다.
        // currentMoveSpeed는 이제 사용하지 않고, moveSpeed를 직접 사용합니다.
    }

    void Update()
    {
        // 설정된 방향과 고정된 moveSpeed로 이동
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 선택 사항: 일정 시간 후 자동 파괴 (화면 밖으로 나가지 않는 경우 대비)
        // Destroy(gameObject, 5f); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("End"))
        {
            Debug.Log("Mover가 'End' 태그 오브젝트와 충돌! 투사체 파괴.");
            Destroy(gameObject);
        }

        // 플레이어 데미지 관련 코드는 이전에 임시 비활성화했습니다.
        // 나중에 플레이어 체력 시스템을 구현할 때 이 부분을 다시 활성화하세요.
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