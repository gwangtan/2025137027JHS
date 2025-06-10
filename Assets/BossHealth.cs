using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Canvas.ForceUpdateCanvases() 사용을 위해 필요

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 1000;
    [SerializeField] private int currentHealth;

    // 체력 700 이하일 때 나타날 팝업 GameObject
    public GameObject popup700Health_Option1;
    public GameObject popup700Health_Option2;
    // 체력 400 이하일 때 나타날 팝업 GameObject
    public GameObject popup400Health_Option1;
    public GameObject popup400Health_Option2;

    private bool popup700Shown = false; // 700 체력 팝업이 이미 표시되었는지 여부
    private bool popup400Shown = false; // 400 체력 팝업이 이미 표시되었는지 여부

    // PlayerStatsManager에 대한 참조
    private PlayerStatsManager playerStatsManager;
    // PlayerShooter에 대한 참조 추가
    private PlayerShooter playerShooter;

    [Header("UI 연동")]
    public Slider healthSlider; // 보스 체력 표시용 슬라이더

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        Debug.Log($"Start(): 초기화 완료 - currentHealth: {currentHealth}, maxHealth: {maxHealth}");

        // PlayerStatsManager 인스턴스 찾기
        playerStatsManager = PlayerStatsManager.Instance;
        if (playerStatsManager == null)
        {
            Debug.LogError("BossHealth (Start): PlayerStatsManager 인스턴스를 찾을 수 없습니다!");
        }

        // PlayerShooter 인스턴스 찾기
        // PlayerShooter 스크립트가 플레이어 GameObject에 직접 붙어있다고 가정합니다.
        // 씬에서 PlayerShooter 컴포넌트를 가진 GameObject를 찾습니다.
        playerShooter = FindObjectOfType<PlayerShooter>();
        if (playerShooter == null)
        {
            Debug.LogError("BossHealth (Start): PlayerShooter 인스턴스를 찾을 수 없습니다! 플레이어 발사 기능에 영향을 미칠 수 있습니다. PlayerShooter 스크립트가 활성화된 GameObject에 있는지 확인하세요.");
        }
        else
        {
            Debug.Log($"BossHealth (Start): PlayerShooter 인스턴스 찾음: {playerShooter.name}");
        }

        Debug.Log("Start 실행됨, currentHealth = " + currentHealth);
    }

    // 보스가 데미지를 입었을 때 호출되는 함수
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log($"TakeDamage(): 현재 체력 {currentHealth}, 퍼센트 {(float)currentHealth / maxHealth * 100f}%");

        // 체력 70% 이하 (700) 팝업 처리
        if ((float)currentHealth / maxHealth <= 0.7f && !popup700Shown)
        {
            Debug.Log("TakeDamage(): 체력 70% 이하 (700) 조건 만족, 팝업 표시.");
            ShowPopup(popup700Health_Option1, popup700Health_Option2);
            popup700Shown = true;
        }

        // 체력 40% 이하 (400) 팝업 처리
        // 700 팝업보다 우선순위를 높게 둡니다 (더 낮은 체력 구간이므로)
        if ((float)currentHealth / maxHealth <= 0.4f && !popup400Shown)
        {
            Debug.Log("TakeDamage(): 체력 40% 이하 (400) 조건 만족, 팝업 표시.");
            ShowPopup(popup400Health_Option1, popup400Health_Option2);
            popup400Shown = true;
        }

        // 보스 사망 처리
        if (currentHealth <= 0)
            Die();
    }

    // 두 개의 팝업 GameObject를 활성화하는 함수
    void ShowPopup(GameObject option1, GameObject option2)
    {
        Debug.Log("BossHealth (ShowPopup): 팝업 활성화 함수 호출됨.");

        // 각 팝업 GameObject의 CanvasGroup을 활성화합니다.
        EnablePopupCanvasGroup(option1);
        EnablePopupCanvasGroup(option2);

        // 팝업이 나타난 후 게임을 일시 정지시키는 코루틴 시작
        StartCoroutine(PauseAfterPopup());
    }

    // 팝업 활성화 후 게임을 일시 정지시키는 코루틴
    IEnumerator PauseAfterPopup()
    {
        yield return null; // 1 프레임 대기 (UI 갱신을 위해)
        Canvas.ForceUpdateCanvases(); // UI 캔버스를 강제로 즉시 갱신합니다.
        Time.timeScale = 0f; // 게임 시간 정지
        Debug.Log("BossHealth (PauseAfterPopup): 게임 정지됨 (Time.timeScale = 0)");
    }

    // 팝업을 숨기고 게임을 재개하는 함수 (UI 버튼 등에서 호출)
    // optionChosen: 어떤 옵션을 선택했는지 나타내는 정수 (예: 1, 2)
    public void HidePopupsAndResumeGame(int optionChosen)
    {
        Debug.Log($"BossHealth (HidePopupsAndResumeGame): 호출됨! 선택 옵션: {optionChosen}");

        HideAllPopupsImmediately(); // 모든 팝업 즉시 숨기기

        Time.timeScale = 1f; // 게임 시간 재개
        Debug.Log("BossHealth (HidePopupsAndResumeGame): 게임 재개됨 (Time.timeScale = 1)");

        // 플레이어 능력치 적용
        if (playerStatsManager != null)
        {
            // 400 체력 팝업이 표시된 상태에서 선택된 경우 (더 낮은 체력 구간이므로 우선 처리)
            if (popup400Shown && currentHealth <= 400)
            {
                Debug.Log($"HidePopupsAndResumeGame: 400 체력 팝업 처리 중. 선택 옵션: {optionChosen}");
                if (optionChosen == 1)
                {
                    // 400 체력 팝업에서 option1을 선택했을 때 (현재는 추가 기능 없음)
                    Debug.Log("BossHealth (HidePopupsAndResumeGame): 400 체력 팝업 Option1 선택. 추가 기능 없음.");
                }
                else if (optionChosen == 2)
                {
                    // 400 체력 팝업에서 option2를 선택했을 때 (투사체 다방향 발사 활성화)
                    if (playerShooter != null)
                    {
                        Debug.Log("BossHealth (HidePopupsAndResumeGame): PlayerShooter 인스턴스 유효. 다방향 발사 활성화 시도.");
                        playerShooter.ActivateMultiDirectionalShooting();
                        Debug.Log("BossHealth (HidePopupsAndResumeGame): 400 체력 팝업 Option2 선택, 다방향 발사 활성화 완료.");
                    }
                    else
                    {
                        Debug.LogWarning("BossHealth (HidePopupsAndResumeGame): PlayerShooter 인스턴스를 찾을 수 없어 다방향 발사를 활성화할 수 없습니다. 유니티 씬에서 Player GameObject에 PlayerShooter 스크립트가 올바르게 할당되고 활성화되어 있는지 확인하세요.");
                    }
                }
            }
            // 700 체력 팝업이 표시된 상태에서 선택된 경우 (400 체력 팝업이 활성화되지 않았을 때만)
            else if (popup700Shown && currentHealth <= 700)
            {
                Debug.Log($"HidePopupsAndResumeGame: 700 체력 팝업 처리 중. 선택 옵션: {optionChosen}");
                if (optionChosen == 1)
                {
                    // 700 체력 팝업에서 option1을 선택했을 때 (투사체 데미지 +2)
                    playerStatsManager.IncreaseProjectileDamage(2);
                    Debug.Log("BossHealth (HidePopupsAndResumeGame): 700 체력 팝업 Option1 선택, 투사체 데미지 +2 적용.");
                }
                else if (optionChosen == 2)
                {
                    // 700 체력 팝업에서 option2를 선택했을 때 (무적 시간 2배)
                    playerStatsManager.MultiplyInvincibilityDuration(2f);
                    Debug.Log("BossHealth (HidePopupsAndResumeGame): 700 체력 팝업 Option2 선택, 무적 시간 2배 적용.");
                }
            }
        }
        else
        {
            Debug.LogWarning("BossHealth (HidePopupsAndResumeGame): PlayerStatsManager 인스턴스를 찾을 수 없어 능력치 적용이 불가능합니다.");
        }
    }

    // 모든 팝업 GameObject를 즉시 비활성화하는 함수
    private void HideAllPopupsImmediately()
    {
        Debug.Log("BossHealth (HideAllPopupsImmediately): 모든 팝업 비활성화 시도.");

        DisablePopupCanvasGroup(popup700Health_Option1);
        DisablePopupCanvasGroup(popup700Health_Option2);
        DisablePopupCanvasGroup(popup400Health_Option1);
        DisablePopupCanvasGroup(popup400Health_Option2);
    }

    // 특정 팝업 GameObject의 CanvasGroup을 비활성화하는 헬퍼 함수
    private void DisablePopupCanvasGroup(GameObject popup)
    {
        if (popup == null)
        {
            Debug.LogWarning("DisablePopupCanvasGroup: 참조된 팝업 GameObject가 null입니다.");
            return; // GameObject가 null이면 함수 종료
        }

        var canvasGroup = popup.GetComponent<CanvasGroup>(); // CanvasGroup 컴포넌트 가져오기
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // 투명하게 설정
            canvasGroup.interactable = false; // 상호작용 불가능하게 설정
            canvasGroup.blocksRaycasts = false; // 레이캐스트 차단 해제 (클릭 이벤트 통과)
        }

        popup.SetActive(false); // GameObject 비활성화
        Debug.Log($"BossHealth (DisablePopupCanvasGroup): {popup.name} 비활성화됨.");
    }

    // 특정 팝업 GameObject의 CanvasGroup을 활성화하는 헬퍼 함수
    private void EnablePopupCanvasGroup(GameObject popup)
    {
        if (popup == null)
        {
            Debug.LogWarning("EnablePopupCanvasGroup: 참조된 팝업 GameObject가 null입니다.");
            return; // GameObject가 null이면 함수 종료
        }

        popup.SetActive(true); // GameObject 활성화

        var canvasGroup = popup.GetComponent<CanvasGroup>(); // CanvasGroup 컴포넌트 가져오기
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; // 불투명하게 설정
            canvasGroup.interactable = true; // 상호작용 가능하게 설정
            canvasGroup.blocksRaycasts = true; // 레이캐스트 차단 (클릭 이벤트 처리)
        }

        Debug.Log($"BossHealth (EnablePopupCanvasGroup): {popup.name} 활성화됨.");
    }

    // 보스 사망 처리 함수
    void Die()
    {
        Debug.Log("BossHealth (Die): 보스 사망!");
        Destroy(gameObject); // 보스 GameObject 파괴
        Time.timeScale = 1f; // 게임 재개 (혹시 사망 시 정지되어 있다면)
        Debug.Log("BossHealth (Die): 게임 재개됨 (Time.timeScale = 1)");
        // TODO: 게임 종료, 승리 화면 등 추가 로직 구현
    }
}
