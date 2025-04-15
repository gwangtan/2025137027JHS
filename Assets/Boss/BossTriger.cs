using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public BossCameraFocus bossCameraFocus; // BossCameraFocus 스크립트
    public GameObject bossObject; // 보스 오브젝트
    public float bossMoveSpeed = 5f; // 보스 이동 속도
    public string targetAnimationName = "BossRun"; // 재생할 애니메이션 이름
    public float tileDestroyDelay = 0.2f; // 타일 파괴 지연 시간
    public float tileDestroyRadius = 2f; // 파괴할 타일 반경
    public LayerMask tilemapLayer; // 타일맵 레이어

    private Animator bossAnimator;
    private bool bossActivated = false;

    void Start()
    {
        if (bossObject != null)
        {
            bossAnimator = bossObject.GetComponent<Animator>();
            if (bossAnimator == null)
            {
                Debug.LogWarning("보스 오브젝트에 Animator 컴포넌트가 없습니다.");
            }
            bossObject.SetActive(false); // 초기에는 보스 비활성화
        }
        else
        {
            Debug.LogError("보스 오브젝트가 할당되지 않았습니다.");
        }

        if (bossCameraFocus == null)
        {
            Debug.LogError("BossCameraFocus 스크립트가 할당되지 않았습니다.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!bossActivated && other.CompareTag("Player"))
        {
            bossActivated = true;
            if (bossCameraFocus != null && bossObject != null)
            {
                bossObject.SetActive(true);
                bossCameraFocus.FocusOnBoss();
                StartCoroutine(MoveBossAndDestroyTiles());
                PlayBossAnimation(targetAnimationName);
            }
        }
    }

    IEnumerator MoveBossAndDestroyTiles()
    {
        if (bossObject != null)
        {
            Rigidbody2D bossRb = bossObject.GetComponent<Rigidbody2D>();
            if (bossRb == null)
            {
                Debug.LogError("보스 오브젝트에 Rigidbody2D 컴포넌트가 없습니다.");
                yield break;
            }

            while (true)
            {
                bossRb.velocity = Vector2.left * bossMoveSpeed;

                // 주변 타일맵 감지 및 파괴
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bossObject.transform.position, tileDestroyRadius, tilemapLayer);
                foreach (Collider2D hitCollider in hitColliders)
                {
                    Tilemap tilemap = hitCollider.GetComponent<Tilemap>();
                    if (tilemap != null)
                    {
                        Vector3 hitPoint = hitCollider.ClosestPoint(bossObject.transform.position);
                        Vector3Int tilePosition = tilemap.WorldToCell(hitPoint);
                        tilemap.SetTile(tilePosition, null); // 타일 제거
                    }
                    else
                    {
                        // Tilemap 컴포넌트가 없는 경우 일반 GameObject로 처리 (선택 사항)
                        Destroy(hitCollider.gameObject, tileDestroyDelay);
                    }
                }

                yield return null; // 다음 프레임까지 대기
            }
        }
    }

    void PlayBossAnimation(string animationName)
    {
        if (bossAnimator != null)
        {
            bossAnimator.Play(animationName);
        }
    }

    // 필요하다면 보스 이동을 멈추는 함수 추가

    }
