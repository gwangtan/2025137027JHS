using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public Animator myAnimator;
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMovingRight = true;
    private GameObject playerObject;
    public AudioClip collisionSound;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        float direction = Input.GetAxis("Horizontal");

        if (isMovingRight)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            isMovingRight = !isMovingRight;
        }


        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && !player.isInvincible) // 플레이어가 무적 상태가 아닐 때만 함정 효과 적용
            {

                playerObject.SetActive(false);
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);


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

