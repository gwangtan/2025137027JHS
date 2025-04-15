using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrapEffect2D : MonoBehaviour
{
    public AudioClip collisionSound;
    public GameObject popupImagePrefab;
    private Vector3 initialPosition;
    private GameObject playerObject;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && !player.isInvincible) // �÷��̾ ���� ���°� �ƴ� ���� ���� ȿ�� ����
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);

                Vector3 popupPosition = collision.transform.position;
                popupPosition.z -= 5;
                Instantiate(popupImagePrefab, popupPosition, Quaternion.identity);

                playerObject.SetActive(false);

                // 2�� �ڿ� Scene�� �ʱ�ȭ�ϴ� Coroutine ȣ��
                StartCoroutine(ResetSceneAfterDelay());
            }
        }

        // 2�� �Ŀ� Scene�� �����ϴ� Coroutine
        IEnumerator ResetSceneAfterDelay()
        {
            // 2�� ���
            yield return new WaitForSeconds(2f);

            // ���� Scene�� �ٽ� �ε�
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}

