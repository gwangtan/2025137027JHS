using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomingProjectile : MonoBehaviour
{
    private Transform target;
    private float trackingDuration;
    private float currentTrackingTime;
    private float speed = 6f; // ����ü �̵� �ӵ� (���� ����)
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
            // ��ǥ ���� ���
            Vector2 direction = (target.position - transform.position).normalized;
            lastTrackedDirection = direction;
            // �̵�
            transform.Translate(direction * speed * Time.deltaTime);

            currentTrackingTime += Time.deltaTime;
            if (currentTrackingTime >= trackingDuration)
            {
                isTracking = false;
            }
        }
        else
        {
            // ������ ������ ������ �������� ���� �̵�
            transform.Translate(lastTrackedDirection * speed * Time.deltaTime);
        }
    }

    // �浹 ó�� (�ʿ信 ���� ����)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && !player.isInvincible) // �÷��̾ ���� ���°� �ƴ� ���� ���� ȿ�� ����
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);


                // 2�� �ڿ� Scene�� �ʱ�ȭ�ϴ� Coroutine ȣ��
                StartCoroutine(ResetSceneAfterDelay());
            }
        }

        // 2�� �Ŀ� Scene�� �����ϴ� Coroutine
        IEnumerator ResetSceneAfterDelay()
        {
            // 2�� ���
            yield return new WaitForSeconds(0f);

            // ���� Scene�� �ٽ� �ε�
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}


