using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomingProjectile : MonoBehaviour
{
    private Transform target;
    private float trackingDuration;
    private float currentTrackingTime;
    private float speed = 6f; // 투사체 이동 속도 (조절 가능)
    private Vector2 lastTrackedDirection;
    private bool isTracking = false;
    private GameObject playerObject;
    public AudioClip collisionSound;

    public void SetTarget(Transform targetTransform, float duration)
    {
        target = targetTransform;
        trackingDuration = duration;
        currentTrackingTime = 0f;
        isTracking = true;
    }

    void Update()
    {

        if (isTracking && target != null)
        {
            // 목표 방향 계산
            Vector2 direction = (target.position - transform.position).normalized;
            lastTrackedDirection = direction;
            // 이동
            transform.Translate(direction * speed * Time.deltaTime);

            currentTrackingTime += Time.deltaTime;
            if (currentTrackingTime >= trackingDuration)
            {
                isTracking = false;
            }
        }
        else
        {
            // 추적이 끝나면 마지막 방향으로 직선 이동
            transform.Translate(lastTrackedDirection * speed * Time.deltaTime);
        }
    }

    // 충돌 처리 (필요에 따라 구현)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && !player.isInvincible) // 플레이어가 무적 상태가 아닐 때만 함정 효과 적용
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);


                // 2초 뒤에 Scene을 초기화하는 Coroutine 호출
                StartCoroutine(ResetSceneAfterDelay());
            }
        }

        // 2초 후에 Scene을 리셋하는 Coroutine
        IEnumerator ResetSceneAfterDelay()
        {
            // 2초 대기
            yield return new WaitForSeconds(0f);

            // 현재 Scene을 다시 로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}


