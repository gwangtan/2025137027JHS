using UnityEngine;

public class Item : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.ApplyItemEffect();
                Destroy(gameObject);
            }
        }
    }
}