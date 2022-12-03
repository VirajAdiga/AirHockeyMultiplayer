using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private bool wasClicked = false;
    private Rigidbody2D playerRigidBody;
    private CircleCollider2D playerCollider;

    private float upperLimit, lowerLimit, leftLimit, rightLimit;

    public static GameObject localPlayer;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            upperLimit = MultiplayerGameController.gameController.middle.position.y;
            lowerLimit = MultiplayerGameController.gameController.down.position.y;
            leftLimit = MultiplayerGameController.gameController.left.position.x;
            rightLimit = MultiplayerGameController.gameController.right.position.x;
        }
        else
        {
            upperLimit = MultiplayerGameController.gameController.up.position.y;
            lowerLimit = MultiplayerGameController.gameController.middle.position.y;
            leftLimit = MultiplayerGameController.gameController.left.position.x;
            rightLimit = MultiplayerGameController.gameController.right.position.x;
        }

        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false)
            return;

        if (Input.GetMouseButton(0))
        {
            wasClicked = true;
        }

        if (wasClicked)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
                if (hit.collider == playerCollider)
                {
                    Vector2 clampedPos = new Vector2(Mathf.Clamp(mousePosition.x, leftLimit, rightLimit), Mathf.Clamp(mousePosition.y, lowerLimit, upperLimit));
                    playerRigidBody.MovePosition(clampedPos);
                }
        }
    }
}
