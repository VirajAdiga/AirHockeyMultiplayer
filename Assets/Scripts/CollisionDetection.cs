using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField]
    private Collider2D AIPlayerCollider;

    [SerializeField]
    private Collider2D playerCollider;

    private UnityEvent AIPoint, PlayerPoint;

    private void Start()
    {
        if (AIPoint == null)
            AIPoint = new UnityEvent();
        if (PlayerPoint == null)
            PlayerPoint = new UnityEvent();
        AIPoint.AddListener(GameController.gameController.AIHasScored);
        PlayerPoint.AddListener(GameController.gameController.PlayerHasScored);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider == AIPlayerCollider)
        {
            PlayerPoint.Invoke();
            AudioManager.audioManager.PlayGoalSound();
        }

        else if (collision.collider == playerCollider)
        {
            AIPoint.Invoke();
            AudioManager.audioManager.PlayGoalSound();
        }

        else
        {
            AudioManager.audioManager.PlayHitSound();
        }
    }
}
