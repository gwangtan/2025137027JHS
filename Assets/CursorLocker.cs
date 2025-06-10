using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    void Start()
    {
        // Ŀ���� ���� â �߾ӿ� �����ϰ� ����ϴ�.
        // Cursor.lockState = CursorLockMode.Locked; 
        // Cursor.visible = false; 

        // 2D ž�� ���ӿ����� ���� Ŀ���� ������ �ϹǷ�,
        // �Ʒ��� ���� 'Ŭ�� ���'�� �����ϴ� ���� �� ������ �� �ֽ��ϴ�.
        Cursor.lockState = CursorLockMode.Confined; // Ŀ���� ���� â ������ �����մϴ�.
        Cursor.visible = true; // Ŀ���� ���̰� �մϴ�.
    }

    void Update()
    {
        // ���� ��� Esc Ű�� ������ Ŀ�� ������ ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // Ŀ�� ���� ����
            Cursor.visible = true; // Ŀ�� ���̱�
        }
    }
}