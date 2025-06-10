using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�

    private Rigidbody2D rb;
    private Camera mainCamera; // ���� ī�޶� ����

    // ȭ�� ��� ����� ���� ����
    private float minX, maxX, minY, maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody2D ������Ʈ�� �÷��̾ �����ϴ�!");
            enabled = false; // Rigidbody2D ������ ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� ã�� �� �����ϴ�. 'MainCamera' �±װ� �ִ��� Ȯ���ϼ���.");
            enabled = false; // ī�޶� ������ ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        // ���� ���� �� ī�޶� ȭ�� ��� ���
        CalculateCameraBounds();
    }

    void CalculateCameraBounds()
    {
        // ī�޶� ����Ʈ (0,0)�� ���� �ϴ�, (1,1)�� ���� ���
        // ViewportToWorldPoint�� Z���� �ʿ�� �մϴ�.
        // �÷��̾��� Z ��ġ�� ����Ͽ� 2D ���� ��ġ��ŵ�ϴ�.
        float zDepth = transform.position.z;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDepth));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDepth));

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;

        // �÷��̾� �ݶ��̴��� ���� ũ�⸦ ��迡 �߰��Ͽ� 
        // �÷��̾��� �����ڸ��� ȭ�� ��迡 �굵�� ���� (�ɼ�)
        // BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();
        // if (playerCollider != null)
        // {
        //     float halfWidth = playerCollider.size.x / 2f;
        //     float halfHeight = playerCollider.size.y / 2f;
        //     minX += halfWidth;
        //     maxX -= halfWidth;
        //     minY += halfHeight;
        //     maxY -= halfHeight;
        // }
    }

    void FixedUpdate() // ���� ������Ʈ�� FixedUpdate���� ó���ϴ� ���� �����ϴ�.
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Z���� �÷��̾��� Z ��ġ�� �����Ͽ� 2D ��鿡�� ��ȯ
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        // �Ǵ� �׳� mouseScreenPosition.z = transform.position.z; �� �ص� ������ ��Ȯ�� Zdepth�� �̷��� ���ϴ� ���� �����ϴ�.

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // --- ���� �߰��� �κ�: ��ǥ ��ġ�� ȭ�� ��� ������ ���� ---
        // Mathf.Clamp�� ����Ͽ� ���콺 ��ǥ ��ġ�� ī�޶� ����Ʈ ���� ����
        mouseWorldPosition.x = Mathf.Clamp(mouseWorldPosition.x, minX, maxX);
        mouseWorldPosition.y = Mathf.Clamp(mouseWorldPosition.y, minY, maxY);
        // --- �� ---

        // ���� ��ġ���� ���콺 ��ġ�� �ε巴�� �̵��� ��ǥ ���� ���
        Vector3 targetLerpPosition = Vector3.Lerp(rb.position, mouseWorldPosition, moveSpeed * Time.fixedDeltaTime);

        // Rigidbody2D.MovePosition�� ����Ͽ� ���������� �̵�
        rb.MovePosition(targetLerpPosition);
    }

    // OnCollisionEnter2D�� OnCollisionExit2D�� �״�� �μ���.
    // ���� ����� �� ���������� ������ ���� Rigidbody2D.MovePosition�� ó���մϴ�.
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�÷��̾ �浹! ����: " + collision.gameObject.name + " (Layer: " + collision.gameObject.layer + ")");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("�÷��̾ �浹 ����! ����: " + collision.gameObject.name);
    }
}