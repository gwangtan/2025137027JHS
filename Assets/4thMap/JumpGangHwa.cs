using UnityEngine;
using System.Collections;

public class ItemEffect : MonoBehaviour
{
    public float effectDuration = 3f; // ȿ�� ���� �ð� (��)
    public float minJumpBoostMultiplier = 0.5f; // �ּ� ������ ��ȭ ����
    public float maxJumpBoostMultiplier = 2.5f; // �ִ� ������ ��ȭ ����
    public GameObject floatingImagePrefab; // ��� �̹��� ������ (���� ����)
    public string targetTag = "Player"; // ȿ���� ������ ��� �±�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ������Ʈ�� �±׸� Ȯ���Ͽ� ȿ���� ������ ������� Ȯ���մϴ�.
        if (collision.CompareTag(targetTag))
        {
            // �浹�� ������Ʈ���� PlayerMoveScript ������Ʈ�� �����ɴϴ�.
            PlayerMoveScript playerMoveScript = collision.GetComponent<PlayerMoveScript>();
            if (playerMoveScript != null)
            {
                // PlayerMoveScript�� ���� �ν�Ʈ ȿ���� �����մϴ�.
                StartCoroutine(ApplyRandomJumpBoostEffect(playerMoveScript, collision.gameObject));
                // ������ ȿ���� ����Ǿ����Ƿ� ������ ������Ʈ�� �����մϴ�.
                Destroy(gameObject);
            }
            else
            {
                // PlayerMoveScript�� ���ٸ� ��� �޽����� �ֿܼ� ����մϴ�.
                Debug.LogWarning("PlayerMoveScript�� ã�� �� �����ϴ�. �±׸� Ȯ���ϰ�, PlayerMoveScript�� ������Ʈ�� �ִ��� Ȯ���ϼ���.");
            }
        }
    }

    IEnumerator ApplyRandomJumpBoostEffect(PlayerMoveScript playerMoveScript, GameObject targetObject)
    {
        // ���� ���� �� ����
        float originalJumpForce = playerMoveScript.jumpForce;

        // ������ ������ ��ȭ ���� ����
        float randomJumpBoostMultiplier = Random.Range(minJumpBoostMultiplier, maxJumpBoostMultiplier);

        // ���� �� ���� ����
        playerMoveScript.jumpForce *= randomJumpBoostMultiplier;

        // �̹��� ����
        if (floatingImagePrefab != null)
        {
            GameObject floatingImage = Instantiate(floatingImagePrefab, targetObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
            floatingImage.transform.SetParent(targetObject.transform); // �÷��̾��� �ڽ����� ����
            Destroy(floatingImage, effectDuration); // �̹����� ȿ�� ���� �ð� �Ŀ� ����
        }

        yield return new WaitForSeconds(effectDuration);

        // ���� �� ������� ����
        playerMoveScript.jumpForce = originalJumpForce;
    }
}