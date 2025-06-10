using UnityEngine;

// �� ��ũ��Ʈ�� �÷��̾��� �ɷ�ġ�� �߾ӿ��� �����մϴ�.
// GameManager�� ���� �̱��� ������Ʈ�� �ٿ� ����ϴ� ���� �����ϴ�.
public class PlayerStatsManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ� (��𼭵� �� ��ũ��Ʈ�� ������ �� �ְ� �մϴ�.)
    public static PlayerStatsManager Instance { get; private set; }

    // ���� �÷��̾� ����ü�� �⺻ ������
    // �ν����Ϳ��� �ʱⰪ�� ������ �� �ֽ��ϴ�.
    public int currentProjectileDamage = 5;

    // �÷��̾��� ���� �ð� ����
    // �ν����Ϳ��� �ʱⰪ�� ������ �� �ֽ��ϴ�. (�⺻���� 1��)
    public float invincibilityDurationMultiplier = 1f;

    // ��ũ��Ʈ�� ó�� Ȱ��ȭ�� �� ȣ��˴ϴ�.
    void Awake()
    {
        Debug.Log("PlayerStatsManager (Awake): Awake ȣ���.");

        // �̱��� �ν��Ͻ� ����:
        // ���� �̹� �ν��Ͻ��� �����ϰ� ���� ��ũ��Ʈ�� �� �ν��Ͻ��� �ƴ϶��,
        // ���� ������Ʈ�� �ı��Ͽ� �ߺ� ������ �����ϴ�.
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"PlayerStatsManager (Awake): �̹� �ν��Ͻ��� �����մϴ� ({Instance.name}). ���� ������Ʈ ({gameObject.name})�� �ı��մϴ�.");
            Destroy(gameObject);
        }
        else
        {
            // �� ��ũ��Ʈ�� ������ �ν��Ͻ��� �ǵ��� �����մϴ�.
            Instance = this;
            // ���� ����Ǿ �� ������Ʈ�� �ı����� �ʵ��� �մϴ�.
            DontDestroyOnLoad(gameObject);
            Debug.Log($"PlayerStatsManager (Awake): PlayerStatsManager �ν��Ͻ� ���� �Ϸ�. ���� ������: {currentProjectileDamage}, ���� �ð� ����: {invincibilityDurationMultiplier}. �� ������Ʈ�� �� �ε� �� �ı����� �ʽ��ϴ�.");
        }
    }

    // �÷��̾� ����ü �������� ������Ű�� �Լ�
    public void IncreaseProjectileDamage(int amount)
    {
        currentProjectileDamage += amount;
        Debug.Log($"PlayerStatsManager (IncreaseProjectileDamage): �÷��̾� ����ü �������� {amount} �����߽��ϴ�! ���� ������: {currentProjectileDamage}");
    }

    // �÷��̾��� ���� �ð� ������ ������Ű�� �Լ�
    public void MultiplyInvincibilityDuration(float multiplier)
    {
        invincibilityDurationMultiplier *= multiplier;
        Debug.Log($"PlayerStatsManager (MultiplyInvincibilityDuration): ���� �ð� ������ {multiplier}�� �����߽��ϴ�! ���� ����: {invincibilityDurationMultiplier}");
    }

    // �� ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ȣ��˴ϴ�.
    void OnDisable()
    {
        Debug.LogWarning("PlayerStatsManager (OnDisable): PlayerStatsManager ��ũ��Ʈ�� ��Ȱ��ȭ�Ǿ����ϴ�!");
    }

    // �� ��ũ��Ʈ�� �ı��� �� ȣ��˴ϴ�.
    void OnDestroy()
    {
        Debug.LogWarning($"PlayerStatsManager (OnDestroy): PlayerStatsManager �ν��Ͻ� ({gameObject.name})�� �ı��Ǿ����ϴ�! currentProjectileDamage: {currentProjectileDamage}, invincibilityDurationMultiplier: {invincibilityDurationMultiplier}");
        if (Instance == this)
        {
            Instance = null; // �ν��Ͻ��� �ı��Ǹ� null�� ����
            Debug.Log("PlayerStatsManager (OnDestroy): �̱��� �ν��Ͻ� ������ null�� ������.");
        }
    }
}
