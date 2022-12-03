using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform puck;

    private Rigidbody2D enemyRigidBody;

    private float upperLimit, lowerLimit, leftLimit, rightLimit;

    private Vector2 startPosition, targetPosition;

    private float speed;

    private float maxSpeed;

    private float offsetPos;

    private bool isPucksInOpponentCourt;

    void Start()
    {
        upperLimit = GameController.gameController.up.position.y;
        lowerLimit = GameController.gameController.middle.position.y;
        leftLimit = GameController.gameController.left.position.x;
        rightLimit = GameController.gameController.right.position.x;
        maxSpeed = GameController.gameController.speed;

        enemyRigidBody = GetComponent<Rigidbody2D>();
        startPosition = enemyRigidBody.position;

        isPucksInOpponentCourt = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (puck.position.y <= lowerLimit)
        {
            speed = maxSpeed * Random.Range(0.1f, 0.2f);

            if (isPucksInOpponentCourt)
            {
                isPucksInOpponentCourt = false;
                offsetPos = Random.Range(-1.5f, 1.5f);
            }
            targetPosition= new Vector2(Mathf.Clamp(puck.position.x+offsetPos, leftLimit, rightLimit), Mathf.Clamp(startPosition.y+offsetPos, lowerLimit, upperLimit));
            offsetPos = 0;
        }
        else
        {
            speed = maxSpeed * Random.Range(0.4f, 0.7f);
            targetPosition = new Vector2(Mathf.Clamp(puck.position.x, leftLimit, rightLimit), Mathf.Clamp(puck.position.y, lowerLimit, upperLimit));

            isPucksInOpponentCourt = true;
        }
        
        enemyRigidBody.MovePosition(Vector2.MoveTowards(enemyRigidBody.position,targetPosition,speed*Time.fixedDeltaTime));
    }
}
