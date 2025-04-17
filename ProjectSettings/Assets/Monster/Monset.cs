using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterAI : MonoBehaviour
{
    public Animator animator; // 애니메이터 컴포넌트
    private Pathfinding pathfinding; // Pathfinding 컴포넌트

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 이동 애니메이션 설정
        if (pathfinding != null && pathfinding.enabled)
        {
            if (pathfinding.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f)
            {
                animator.SetBool("Move", true);
            }
            else
            {
                animator.SetBool("Move", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}