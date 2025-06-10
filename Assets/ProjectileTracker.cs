// ProjectileTracker.cs (혹은 ProjectileCollisionHandler 등)
using UnityEngine;

public class ProjectileTracker : MonoBehaviour
{
    public int damage = 5;

    // 이 스크립트에서는 Update에서 움직임을 제어하지 않습니다.
    // 움직임은 발사하는 스크립트(BossAttackAI)에서 Rigidbody2D.velocity로 제어됩니다.
    // void Update() {} // 이 부분은 비워두거나 삭제합니다.

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("End"))
        {
            Destroy(gameObject);
        }
        // ... (데미지 로직 등 필요하면 추가)
    }
}