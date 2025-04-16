using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterAI : MonoBehaviour
{
    public Animator animator; // �ִϸ����� ������Ʈ
    private Pathfinding pathfinding; // Pathfinding ������Ʈ

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // �̵� �ִϸ��̼� ����
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