using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damageAmount = 10; // 이 투사체가 보스에게 줄 데미지

    // 투사체의 이동 로직 (예시)
    public float speed = 10f;
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); // 예시: 위로 이동
    }

    // 화면 밖으로 나가면 파괴 (선택 사항)
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}