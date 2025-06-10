using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
// TMPro 네임스페이스는 더 이상 사용되지 않으므로 제거합니다.

public class PlayerAttackDetector : MonoBehaviour
{
    // 이동할 특정 씬의 이름
    public string targetSceneName = "GameOverScene";
    // Attack 오브젝트의 태그 (Attack 오브젝트에 이 태그를 설정해주세요)
    public string attackTag = "Attack";
    // 초기 충돌 횟수 (이 숫자가 0이 되면 씬 이동)
    public int initialCollisionCount = 3;
    // 기본 무적 시간 (PlayerStatsManager의 배율이 적용되기 전 원본 값)
    public float baseInvincibilityDuration = 2f;

    private SpriteRenderer playerSpriteRenderer;
    private bool isInvincible = false;
    private int currentCollisionCount; // 플레이어가 맞아야 하는 남은 횟수

    void Start()
    {
        // 플레이어 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("PlayerAttackDetector: SpriteRenderer 컴포넌트를 찾을 수 없습니다. 이 스크립트는 SpriteRenderer가 있는 오브젝트에 적용되어야 합니다.");
            // SpriteRenderer가 없으면 불투명도 변경 기능에 제한이 있을 수 있습니다.
        }

        // 초기 충돌 횟수 설정
        currentCollisionCount = initialCollisionCount;
        Debug.Log($"게임 시작. 남은 충돌 횟수: {currentCollisionCount}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Attack 오브젝트와 충돌했고, 현재 무적 상태가 아닐 때
        if (other.CompareTag(attackTag) && !isInvincible)
        {
            currentCollisionCount--; // 충돌 시 횟수 감소

            Debug.Log($"Attack 오브젝트에 충돌! 남은 충돌 횟수: {currentCollisionCount}");

            // 충돌 횟수가 0 이하가 되면 씬 전환
            if (currentCollisionCount <= 0)
            {
                Debug.Log($"Attack 오브젝트에 {initialCollisionCount}번 충돌하여 {targetSceneName} 씬으로 이동합니다.");
                isInvincible = false; // 씬 전환 직전 무적 상태 해제 (선택 사항)
                SceneManager.LoadScene(targetSceneName); // 특정 씬으로 이동
            }
            else
            {
                // 아직 남은 충돌 횟수가 있다면 무적 코루틴 시작
                StartCoroutine(BecomeTemporarilyInvincible());
            }
        }
    }

    // 플레이어를 일시적으로 무적 상태로 만들고 불투명도를 조절하는 코루틴
    IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true; // 무적 상태로 설정

        // PlayerStatsManager의 무적 시간 배율을 적용하여 최종 무적 시간 계산
        float effectiveInvincibilityDuration = baseInvincibilityDuration;
        if (PlayerStatsManager.Instance != null)
        {
            effectiveInvincibilityDuration *= PlayerStatsManager.Instance.invincibilityDurationMultiplier;
            Debug.Log($"PlayerAttackDetector: 최종 무적 시간 적용됨: {effectiveInvincibilityDuration}초 (기본: {baseInvincibilityDuration}초, 배율: {PlayerStatsManager.Instance.invincibilityDurationMultiplier}배)");
        }
        else
        {
            Debug.LogWarning("PlayerAttackDetector: PlayerStatsManager 인스턴스를 찾을 수 없습니다. 기본 무적 시간을 사용합니다.");
        }


        // 플레이어 불투명도 50%로 변경 (알파 값 0.5)
        if (playerSpriteRenderer != null)
        {
            Color originalColor = playerSpriteRenderer.color;
            playerSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }

        // 계산된 무적 시간만큼 대기
        yield return new WaitForSeconds(effectiveInvincibilityDuration);

        // 무적 상태 해제 및 불투명도 원래대로 복원
        isInvincible = false;
        if (playerSpriteRenderer != null)
        {
            // 코루틴 실행 중 색상이 변경되었을 수 있으므로 현재 색상에서 가져옴
            Color currentColor = playerSpriteRenderer.color;
            playerSpriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f);
        }
    }
}
