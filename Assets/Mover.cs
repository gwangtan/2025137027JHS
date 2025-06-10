using UnityEngine;

public class Mover : MonoBehaviour
{
    // 인스펙터에서 조절 가능한 이동 속도 변수
    public float moveSpeed = 10f; // 투사체 속도

    // 투사체 데미지. 이 값은 플레이어의 투사체를 생성하는 스크립트에서 설정될 예정입니다.
    // 예를 들어, 플레이어의 슈팅 스크립트에서 다음과 같이 설정할 수 있습니다:
    // Mover newProjectile = Instantiate(projectilePrefab).GetComponent<Mover>();
    // newProjectile.damage = PlayerStatsManager.Instance.currentProjectileDamage;
    public int damage;

    void Update()
    {
        // Y축 양의 방향으로 계속 이동
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 선택 사항: 일정 시간 후 자동 파괴
        // Destroy(gameObject, 5f);
    }

    // 다른 2D 콜라이더와 충돌(트리거)했을 때 호출되는 함수
    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트의 태그가 "End"인지 확인
        if (other.CompareTag("End"))
        {
            Debug.Log("Mover가 'End' 태그 오브젝트와 충돌!");
            // Mover(투사체) 자신을 파괴합니다.
            Destroy(gameObject);
        }

        // 충돌한 오브젝트가 보스인지 확인 (보스에게 "Boss" 태그를 부여해야 합니다.)
        if (other.CompareTag("Boss"))
        {
            // 충돌한 오브젝트에서 BossHealth 스크립트를 가져옵니다.
            BossHealth bossHealth = other.GetComponent<BossHealth>();

            // BossHealth 스크립트가 있다면 데미지를 줍니다.
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage); // Mover의 damage 값을 전달
            }

            // 투사체는 보스에게 닿으면 사라지도록 합니다.
            Destroy(gameObject);
        }
    }
}
