using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // 인스펙터에 할당할 투사체 프리팹 (미리 만들어둔 투사체 오브젝트)
    public GameObject projectilePrefab;
    // 발사 간격 (초)
    public float fireRate = 0.5f;
    // 다음 발사까지 남은 시간
    private float nextFireTime;

    // 새로운 기능: 다방향 발사 활성화 여부
    public bool isMultiDirectionalShootingActive = false;
    // 다방향 발사 시 투사체가 퍼지는 각도 (예: 30도)
    public float projectileSpreadAngle = 30f;

    void Update()
    {
        // 현재 게임 시간이 다음 발사 가능 시간보다 크면 투사체 발사
        if (Time.time >= nextFireTime)
        {
            // 다음 발사 시간 설정
            nextFireTime = Time.time + fireRate;

            if (isMultiDirectionalShootingActive)
            {
                // 정면으로 투사체 발사
                ShootSingleProjectile(Quaternion.identity);
                // 왼쪽 방향으로 투사체 발사 (Z축 기준으로 projectileSpreadAngle만큼 회전)
                ShootSingleProjectile(Quaternion.Euler(0, 0, projectileSpreadAngle));
                // 오른쪽 방향으로 투사체 발사 (Z축 기준으로 -projectileSpreadAngle만큼 회전)
                ShootSingleProjectile(Quaternion.Euler(0, 0, -projectileSpreadAngle));
                Debug.Log("PlayerShooter (Fire): 다방향 투사체 발사!");
            }
            else
            {
                // 기본 정면 발사
                ShootSingleProjectile(Quaternion.identity);


            }
        }
    }

    // 투사체 하나를 특정 회전으로 발사하는 헬퍼 메서드
    // rotationOffset은 플레이어의 현재 회전에서 추가적으로 적용할 회전입니다.
    private void ShootSingleProjectile(Quaternion rotationOffset)
    {
        // 플레이어의 위치와 플레이어의 현재 회전에 rotationOffset을 더한 회전으로 투사체 생성
        // Mover 스크립트가 transform.forward를 사용한다면 이 회전값에 따라 투사체 진행 방향이 결정됩니다.
        GameObject newProjectileGO = Instantiate(projectilePrefab, transform.position, transform.rotation * rotationOffset);

        // 생성된 투사체에서 Mover 컴포넌트를 가져옵니다.
        Mover newProjectileMover = newProjectileGO.GetComponent<Mover>();

        // Mover 컴포넌트가 있다면 PlayerStatsManager에서 현재 데미지 값을 가져와 설정합니다.
        if (newProjectileMover != null)
        {
            if (PlayerStatsManager.Instance != null)
            {
                newProjectileMover.damage = PlayerStatsManager.Instance.currentProjectileDamage;
            }
            else
            {
                Debug.LogWarning("PlayerShooter (ShootSingleProjectile): PlayerStatsManager 인스턴스를 찾을 수 없습니다! 기본 투사체 데미지를 사용합니다.");
            }
        }
    }

    // 발사를 멈추는 공용 메서드 (외부에서 호출 가능)
    public void StopShooting()
    {
        // 이 스크립트를 비활성화하여 발사를 멈춥니다.
        enabled = false;
        Debug.Log("PlayerShooter (StopShooting): 발사 중지됨.");
    }

    // 발사를 다시 시작하는 공용 메서드 (외부에서 호출 가능)
    public void StartShooting()
    {
        enabled = true; // 스크립트 활성화
        nextFireTime = Time.time; // 즉시 발사 가능하도록 초기화
        Debug.Log("PlayerShooter (StartShooting): 발사 재개됨.");
    }

    // 다방향 발사 기능을 활성화하는 공용 메서드 (BossHealth에서 호출)
    public void ActivateMultiDirectionalShooting()
    {
        isMultiDirectionalShootingActive = true;
        Debug.Log("PlayerShooter (ActivateMultiDirectionalShooting): 다방향 발사 활성화됨.");
    }

    // 다방향 발사 기능을 비활성화하는 공용 메서드 (필요시 사용)

}
