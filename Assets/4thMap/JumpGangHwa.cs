using UnityEngine;
using System.Collections;

public class ItemEffect : MonoBehaviour
{
    public float effectDuration = 3f; // ȿ�� ���� �ð�
    public float jumpBoostMultiplier = 1.66f; // ������ ��ȭ ����
    public GameObject floatingImagePrefab; // ��� �̹��� ������
    public string targetTag = "Player"; // ȿ���� ������ ��� �±�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ �±׸� ���� ������Ʈ�� �浹���� ���� ȿ�� ����
        if (collision.CompareTag(targetTag))
        {
            PlayerMoveScript playerMoveScript = collision.GetComponent<PlayerMoveScript>();
            if (playerMoveScript != null)
            {
                StartCoroutine(ApplyEffects(playerMoveScript, collision.gameObject));
                Destroy(gameObject); // ������ ������Ʈ ����
            }
            else
            {
                Debug.LogWarning("PlayerMoveScript�� ã�� �� �����ϴ�. �±׸� Ȯ���ϰ�, PlayerMoveScript�� ������Ʈ�� �ִ��� Ȯ���ϼ���.");
            }
        }
    }

    IEnumerator ApplyEffects(PlayerMoveScript playerMoveScript, GameObject targetObject)
    {
        // ���� ������ ����
        float originalJumpForce = playerMoveScript.jumpForce;

        // ������ ���� ����
        playerMoveScript.jumpForce *= jumpBoostMultiplier;

        // �̹��� ����
        if (floatingImagePrefab != null)
        {
            GameObject floatingImage = Instantiate(floatingImagePrefab, targetObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
            floatingImage.transform.SetParent(targetObject.transform); // �÷��̾��� �ڽ����� ����
            Destroy(floatingImage, effectDuration); // �̹����� ȿ�� ���� �ð� �Ŀ� ����
        }


        yield return new WaitForSeconds(effectDuration);

        // ������ ������� ����
        playerMoveScript.jumpForce = originalJumpForce;
    }
}
