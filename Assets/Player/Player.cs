using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMoveScript : MonoBehaviour
{
    public Animator myAnimator;
    public float speed = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public float jumpForce = 300f; // 점프 힘 변수 추가

    // 이동 속도 아이템 관련 변수
    private float originalSpeed;
    private float speedBoostDuration;

    void Start()
    {
        myAnimator.SetBool("move", false);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float direction = Input.GetAxis("Horizontal");

        if (direction != 0)
        {
            myAnimator.SetBool("move", true);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);

            if (direction > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            myAnimator.SetBool("move", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    // 이동 속도 증가 효과 적용 함수
    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        originalSpeed = speed;
        speed += boostAmount;
        speedBoostDuration = duration;
        StartCoroutine(ResetSpeed());
    }

    // 이동 속도 복원 코루틴
    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(speedBoostDuration);
        speed = originalSpeed;
    }

    // 점프력 변경 함수 추가 (ItemEffect에서 호출)
    public void ChangeJumpForce(float multiplier)
    {
        jumpForce *= multiplier;
    }
}
