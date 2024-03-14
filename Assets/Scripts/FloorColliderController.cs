
using UnityEngine;

public class FloorColliderController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
