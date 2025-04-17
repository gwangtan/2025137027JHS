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
            if (player != null && !player.isInvincible) // 플레이어가 무적 상태가 아닐 때만 함정 효과 적용
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);

                Vector3 popupPosition = collision.transform.position;
                popupPosition.z -= 5;
                Instantiate(popupImagePrefab, popupPosition, Quaternion.identity);

                playerObject.SetActive(false);

                // 2초 뒤에 Scene을 초기화하는 Coroutine 호출
                StartCoroutine(ResetSceneAfterDelay());
            }
        }

        // 2초 후에 Scene을 리셋하는 Coroutine
        IEnumerator ResetSceneAfterDelay()
        {
            // 2초 대기
            yield return new WaitForSeconds(2f);

            // 현재 Scene을 다시 로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}

