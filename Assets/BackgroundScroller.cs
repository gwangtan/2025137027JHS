using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 1f; // ��� ��ũ�� �ӵ�
    public float tileSizeY; // ��� Ÿ�� �ϳ��� Y�� ũ�� (World Units)

    private Vector3 startPosition; // ���� ��ġ

    void Start()
    {
        startPosition = transform.position; // ���� ��ġ�� ���� ��ġ�� ����
        // tileSizeY�� �ڵ����� ã������ Sprite Renderer�� size.y�� ��� (��, Sprite�� Ÿ�ϸ��̶�� Ÿ�ϸ� ��ü ���̰� �ƴ� ���� Ÿ�� ����)
        // ���� ���, �ϳ��� ��� ��������Ʈ ������Ʈ�� �� ��ũ��Ʈ�� �ٿ��ٸ�:
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // if (sr != null)
        // {
        //     tileSizeY = sr.bounds.size.y;
        // }
        // ���� Ÿ���� ������ ����̶��, ���� 'Ÿ�� �ϳ��� ����'�� �����ؾ� �մϴ�.
    }

    void Update()
    {
        // Y������ ��ũ�� (deltaTime�� ���Ͽ� ������ �ӵ��� ������)
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeY);

        // �� ��ġ�� �̵� (���� ��ġ���� offsetY��ŭ �̵�)
        transform.position = startPosition + Vector3.down * newPosition;
        // Vector3.down ��� Vector3.up�� ���� ���� ��ũ�ѵ˴ϴ�.
    }
}