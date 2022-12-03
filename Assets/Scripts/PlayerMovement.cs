using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerTransform;
    private bool wasClicked = false;
    private Rigidbody2D playerRigidBody;
    private CircleCollider2D playerCollider;

    private float upperLimit, lowerLimit, leftLimit, rightLimit;

    void Start()
    {
        upperLimit = GameController.gameController.middle.position.y;
        lowerLimit = GameController.gameController.down.position.y;
        leftLimit = GameController.gameController.left.position.x;
        rightLimit = GameController.gameController.right.position.x;

        playerTransform = transform;
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
