using UnityEngine;

public class PlayerHitDetector : MonoBehaviour
{
    private bool isHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit)
            return;

        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        isHit = true;
        GameOverController.Instance.TriggerGameOver();
    }

}
