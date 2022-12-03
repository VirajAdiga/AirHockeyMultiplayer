using UnityEngine;
using Photon.Pun;

public class CollisionManagerMultiplayer : MonoBehaviour
{
    [SerializeField]
    private Collider2D otherPlayerCollider;

    [SerializeField]
    private Collider2D masterClientCollider;


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider == otherPlayerCollider)
        {
            MultiplayerGameController.gameController.photonView.RPC("MasterClientHasScored", RpcTarget.All);
            AudioManager.audioManager.PlayGoalSound();
        }

        else if (collision.collider == masterClientCollider)
        {
            MultiplayerGameController.gameController.photonView.RPC("OtherPlayerHasScored", RpcTarget.All);
            AudioManager.audioManager.PlayGoalSound();
        }

        else
        {
            AudioManager.audioManager.PlayHitSound();
        }
    }
}
