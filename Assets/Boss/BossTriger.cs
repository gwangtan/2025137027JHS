using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public BossCameraFocus bossCameraFocus; // BossCameraFocus ��ũ��Ʈ
    public GameObject bossObject; // ���� ������Ʈ
    public float bossMoveSpeed = 5f; // ���� �̵� �ӵ�
    public string targetAnimationName = "BossRun"; // ����� �ִϸ��̼� �̸�
    public float tileDestroyDelay = 0.2f; // Ÿ�� �ı� ���� �ð�
    public float tileDestroyRadius = 2f; // �ı��� Ÿ�� �ݰ�
    public LayerMask tilemapLayer; // Ÿ�ϸ� ���̾�

    private Animator bossAnimator;
    private bool bossActivated = false;

    void Start()
    {
        if (bossObject != null)
        {
            bossAnimator = bossObject.GetComponent<Animator>();
            if (bossAnimator == null)
            {
                Debug.LogWarning("���� ������Ʈ�� Animator ������Ʈ�� �����ϴ�.");
            }
            bossObject.SetActive(false); // �ʱ⿡�� ���� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("���� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (bossCameraFocus == null)
        {
            Debug.LogError("BossCameraFocus ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
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
                Debug.LogError("���� ������Ʈ�� Rigidbody2D ������Ʈ�� �����ϴ�.");
                yield break;
            }

            while (true)
            {
                bossRb.velocity = Vector2.left * bossMoveSpeed;

                // �ֺ� Ÿ�ϸ� ���� �� �ı�
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bossObject.transform.position, tileDestroyRadius, tilemapLayer);
                foreach (Collider2D hitCollider in hitColliders)
                {
                    Tilemap tilemap = hitCollider.GetComponent<Tilemap>();
                    if (tilemap != null)
                    {
                        Vector3 hitPoint = hitCollider.ClosestPoint(bossObject.transform.position);
                        Vector3Int tilePosition = tilemap.WorldToCell(hitPoint);
                        tilemap.SetTile(tilePosition, null); // Ÿ�� ����
                    }
                    else
                    {
                        // Tilemap ������Ʈ�� ���� ��� �Ϲ� GameObject�� ó�� (���� ����)
                        Destroy(hitCollider.gameObject, tileDestroyDelay);
                    }
                }

                yield return null; // ���� �����ӱ��� ���
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

    // �ʿ��ϴٸ� ���� �̵��� ���ߴ� �Լ� �߰�

    }
