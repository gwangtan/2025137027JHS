using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public string playerTag = "Player";
    public float speedBoostAmount = 3f;
    public float speedBoostDuration = 3f;
    public GameObject speedEffectPrefab; // ������ �̹��� ������

    private GameObject speedEffectInstance; // ������ �̹��� ��ü

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            PlayerMoveScript player = collision.GetComponent<PlayerMoveScript>();
            if (player != null)
            {
                player.ApplySpeedBoost(speedBoostAmount, speedBoostDuration);
                CreateSpeedEffect(player.transform); // �̹��� ���� �� ���� ����
                Destroy(gameObject);
            }
        }
    }

    private void CreateSpeedEffect(Transform playerTransform)
    {
        // �̹��� ������ ����
        speedEffectInstance = Instantiate(speedEffectPrefab, playerTransform.position + Vector3.up * 0.6f, Quaternion.identity);

        // �̹��� ��ü�� �÷��̾��� �ڽ����� �����Ͽ� �Բ� �̵��ϵ��� ��
        speedEffectInstance.transform.SetParent(playerTransform);

        // �̹��� ��ü�� �÷��̾��� 0.6y ��ǥ ���� ��ġ�ϵ��� ����
        speedEffectInstance.transform.localPosition = Vector3.up * 0.6f;

        // 3�� �Ŀ� �̹��� ��ü�� ����
        Destroy(speedEffectInstance, 3f);
    }
}
