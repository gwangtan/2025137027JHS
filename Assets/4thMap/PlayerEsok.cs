using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public string playerTag = "Player";
    public float speedBoostAmount = 3f;
    public float speedBoostDuration = 3f;
    public GameObject speedEffectPrefab; // 생성할 이미지 프리팹

    private GameObject speedEffectInstance; // 생성된 이미지 객체

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            PlayerMoveScript player = collision.GetComponent<PlayerMoveScript>();
            if (player != null)
            {
                player.ApplySpeedBoost(speedBoostAmount, speedBoostDuration);
                CreateSpeedEffect(player.transform); // 이미지 생성 및 추적 시작
                Destroy(gameObject);
            }
        }
    }

    private void CreateSpeedEffect(Transform playerTransform)
    {
        // 이미지 프리팹 생성
        speedEffectInstance = Instantiate(speedEffectPrefab, playerTransform.position + Vector3.up * 0.6f, Quaternion.identity);

        // 이미지 객체를 플레이어의 자식으로 설정하여 함께 이동하도록 함
        speedEffectInstance.transform.SetParent(playerTransform);

        // 이미지 객체가 플레이어의 0.6y 좌표 위에 위치하도록 설정
        speedEffectInstance.transform.localPosition = Vector3.up * 0.6f;

        // 3초 후에 이미지 객체를 제거
        Destroy(speedEffectInstance, 3f);
    }
}
