using UnityEngine;

// 이 스크립트는 플레이어의 능력치를 중앙에서 관리합니다.
// GameManager와 같은 싱글톤 오브젝트에 붙여 사용하는 것이 좋습니다.
public class PlayerStatsManager : MonoBehaviour
{
    // 싱글톤 인스턴스 (어디서든 이 스크립트에 접근할 수 있게 합니다.)
    public static PlayerStatsManager Instance { get; private set; }

    // 현재 플레이어 투사체의 기본 데미지
    // 인스펙터에서 초기값을 설정할 수 있습니다.
    public int currentProjectileDamage = 5;

    // 플레이어의 무적 시간 배율
    // 인스펙터에서 초기값을 설정할 수 있습니다. (기본값은 1배)
    public float invincibilityDurationMultiplier = 1f;

    // 스크립트가 처음 활성화될 때 호출됩니다.
    void Awake()
    {
        Debug.Log("PlayerStatsManager (Awake): Awake 호출됨.");

        // 싱글톤 인스턴스 설정:
        // 만약 이미 인스턴스가 존재하고 현재 스크립트가 그 인스턴스가 아니라면,
        // 현재 오브젝트를 파괴하여 중복 생성을 막습니다.
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"PlayerStatsManager (Awake): 이미 인스턴스가 존재합니다 ({Instance.name}). 현재 오브젝트 ({gameObject.name})를 파괴합니다.");
            Destroy(gameObject);
        }
        else
        {
            // 이 스크립트가 유일한 인스턴스가 되도록 설정합니다.
            Instance = this;
            // 씬이 변경되어도 이 오브젝트가 파괴되지 않도록 합니다.
            DontDestroyOnLoad(gameObject);
            Debug.Log($"PlayerStatsManager (Awake): PlayerStatsManager 인스턴스 설정 완료. 현재 데미지: {currentProjectileDamage}, 무적 시간 배율: {invincibilityDurationMultiplier}. 이 오브젝트는 씬 로드 시 파괴되지 않습니다.");
        }
    }

    // 플레이어 투사체 데미지를 증가시키는 함수
    public void IncreaseProjectileDamage(int amount)
    {
        currentProjectileDamage += amount;
        Debug.Log($"PlayerStatsManager (IncreaseProjectileDamage): 플레이어 투사체 데미지가 {amount} 증가했습니다! 현재 데미지: {currentProjectileDamage}");
    }

    // 플레이어의 무적 시간 배율을 증가시키는 함수
    public void MultiplyInvincibilityDuration(float multiplier)
    {
        invincibilityDurationMultiplier *= multiplier;
        Debug.Log($"PlayerStatsManager (MultiplyInvincibilityDuration): 무적 시간 배율이 {multiplier}배 증가했습니다! 현재 배율: {invincibilityDurationMultiplier}");
    }

    // 이 스크립트가 비활성화될 때 호출됩니다.
    void OnDisable()
    {
        Debug.LogWarning("PlayerStatsManager (OnDisable): PlayerStatsManager 스크립트가 비활성화되었습니다!");
    }

    // 이 스크립트가 파괴될 때 호출됩니다.
    void OnDestroy()
    {
        Debug.LogWarning($"PlayerStatsManager (OnDestroy): PlayerStatsManager 인스턴스 ({gameObject.name})가 파괴되었습니다! currentProjectileDamage: {currentProjectileDamage}, invincibilityDurationMultiplier: {invincibilityDurationMultiplier}");
        if (Instance == this)
        {
            Instance = null; // 인스턴스가 파괴되면 null로 설정
            Debug.Log("PlayerStatsManager (OnDestroy): 싱글톤 인스턴스 참조가 null로 설정됨.");
        }
    }
}
