using UnityEngine;

public class LaunchDiagonal : MonoBehaviour
{
    public float launchForce = 5f; // ���ư� ���� ũ��
    public Vector2 launchDirection = new Vector2(1f, 1f).normalized; // ���ư� ���� (�⺻: ������ �� �밢��)

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Start �Լ����� �ٷ� ���� ���� ���ư����� �մϴ�.
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError(gameObject.name + " ������Ʈ�� Rigidbody2D ������Ʈ�� �����ϴ�. ���ư� �� �����ϴ�.");
            // Rigidbody�� ������ ��ũ��Ʈ�� ��Ȱ��ȭ�Ͽ� ���ʿ��� Update�� �����ϴ�.
            enabled = false;
        }
    }

    // �ʿ��ϴٸ� �ٸ� ���ǿ� ���� �߻�ǵ��� ������ �� �ֽ��ϴ�.
    // ���� ���, Ư�� Ű �Է� �� �߻�ǵ��� �Ϸ��� Update �Լ����� �Է� ó���� �ϸ� �˴ϴ�.
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //         if (rb != null)
    //         {
    //             rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
    //         }
    //     }
    // }
}