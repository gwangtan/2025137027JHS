using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
// TMPro ���ӽ����̽��� �� �̻� ������ �����Ƿ� �����մϴ�.

public class PlayerAttackDetector : MonoBehaviour
{
    // �̵��� Ư�� ���� �̸�
    public string targetSceneName = "GameOverScene";
    // Attack ������Ʈ�� �±� (Attack ������Ʈ�� �� �±׸� �������ּ���)
    public string attackTag = "Attack";
    // �ʱ� �浹 Ƚ�� (�� ���ڰ� 0�� �Ǹ� �� �̵�)
    public int initialCollisionCount = 3;
    // �⺻ ���� �ð� (PlayerStatsManager�� ������ ����Ǳ� �� ���� ��)
    public float baseInvincibilityDuration = 2f;

    private SpriteRenderer playerSpriteRenderer;
    private bool isInvincible = false;
    private int currentCollisionCount; // �÷��̾ �¾ƾ� �ϴ� ���� Ƚ��

    void Start()
    {
        // �÷��̾� ������Ʈ�� SpriteRenderer ������Ʈ ��������
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("PlayerAttackDetector: SpriteRenderer ������Ʈ�� ã�� �� �����ϴ�. �� ��ũ��Ʈ�� SpriteRenderer�� �ִ� ������Ʈ�� ����Ǿ�� �մϴ�.");
            // SpriteRenderer�� ������ ������ ���� ��ɿ� ������ ���� �� �ֽ��ϴ�.
        }

        // �ʱ� �浹 Ƚ�� ����
        currentCollisionCount = initialCollisionCount;
        Debug.Log($"���� ����. ���� �浹 Ƚ��: {currentCollisionCount}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Attack ������Ʈ�� �浹�߰�, ���� ���� ���°� �ƴ� ��
        if (other.CompareTag(attackTag) && !isInvincible)
        {
            currentCollisionCount--; // �浹 �� Ƚ�� ����

            Debug.Log($"Attack ������Ʈ�� �浹! ���� �浹 Ƚ��: {currentCollisionCount}");

            // �浹 Ƚ���� 0 ���ϰ� �Ǹ� �� ��ȯ
            if (currentCollisionCount <= 0)
            {
                Debug.Log($"Attack ������Ʈ�� {initialCollisionCount}�� �浹�Ͽ� {targetSceneName} ������ �̵��մϴ�.");
                isInvincible = false; // �� ��ȯ ���� ���� ���� ���� (���� ����)
                SceneManager.LoadScene(targetSceneName); // Ư�� ������ �̵�
            }
            else
            {
                // ���� ���� �浹 Ƚ���� �ִٸ� ���� �ڷ�ƾ ����
                StartCoroutine(BecomeTemporarilyInvincible());
            }
        }
    }

    // �÷��̾ �Ͻ������� ���� ���·� ����� �������� �����ϴ� �ڷ�ƾ
    IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true; // ���� ���·� ����

        // PlayerStatsManager�� ���� �ð� ������ �����Ͽ� ���� ���� �ð� ���
        float effectiveInvincibilityDuration = baseInvincibilityDuration;
        if (PlayerStatsManager.Instance != null)
        {
            effectiveInvincibilityDuration *= PlayerStatsManager.Instance.invincibilityDurationMultiplier;
            Debug.Log($"PlayerAttackDetector: ���� ���� �ð� �����: {effectiveInvincibilityDuration}�� (�⺻: {baseInvincibilityDuration}��, ����: {PlayerStatsManager.Instance.invincibilityDurationMultiplier}��)");
        }
        else
        {
            Debug.LogWarning("PlayerAttackDetector: PlayerStatsManager �ν��Ͻ��� ã�� �� �����ϴ�. �⺻ ���� �ð��� ����մϴ�.");
        }


        // �÷��̾� ������ 50%�� ���� (���� �� 0.5)
        if (playerSpriteRenderer != null)
        {
            Color originalColor = playerSpriteRenderer.color;
            playerSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }

        // ���� ���� �ð���ŭ ���
        yield return new WaitForSeconds(effectiveInvincibilityDuration);

        // ���� ���� ���� �� ������ ������� ����
        isInvincible = false;
        if (playerSpriteRenderer != null)
        {
            // �ڷ�ƾ ���� �� ������ ����Ǿ��� �� �����Ƿ� ���� ���󿡼� ������
            Color currentColor = playerSpriteRenderer.color;
            playerSpriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f);
        }
    }
}
