using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float moveSpeed = 3f;  // 몬스터의 이동 속도
    private Transform player;      // Player의 Transform 컴포넌트를 저장할 변수

    void Start()
    {
        // Player 태그를 가진 오브젝트를 찾아서 player 변수에 할당
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Player 방향으로 몬스터를 향하도록 회전
            Vector2 direction = (player.position - transform.position).normalized;

            // 이동
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }


}
