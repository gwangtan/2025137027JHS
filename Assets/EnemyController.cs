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
            if (player != null && !player.isInvincible) // �÷��̾ ���� ���°� �ƴ� ���� ���� ȿ�� ����
            {

                playerObject.SetActive(false);
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);


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

