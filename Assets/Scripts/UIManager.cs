using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerScore;

    public Text AIScore;

    private int playerPoints, AIPoints;

    public Text gameResult;

    public Text wins, losses;

    public Image resultPanel;

    public Text instruction;

    public Image panel;

    public GameObject pauseMenu, GamePlay;

    public GameObject gameResultPanel;

    public Sprite defaultSprite;

    public Sprite goldSprite, silverSprite, bronzeSprite;

    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;

    [SerializeField]
    private Transform player, AI;

    private Vector2 playerPos, AIPos;

    private Vector3 offsetPos;

    private Camera mainCamera;

    private UIManager()
    {

    }

    static UIManager instance;

    public static UIManager uiManager
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerPoints = AIPoints = 0;
        playerScore.text = playerPoints.ToString();
        AIScore.text = AIPoints.ToString();

        ApplySpriteToPlayer();

        mainCamera = Camera.main;
        offsetPos = new Vector3(1.5f, 1.5f, 0);
    }

    private void ApplySpriteToPlayer()
    {
        if (GameData.gameType == GameType.Gold)
        {
            playerSpriteRenderer.sprite = goldSprite;
        }
        else if (GameData.gameType == GameType.Silver)
        {
            playerSpriteRenderer.sprite = silverSprite;
        }
        else if (GameData.gameType == GameType.Bronze)
        {
            playerSpriteRenderer.sprite = bronzeSprite;
        }
        else
        {
            playerSpriteRenderer.sprite = defaultSprite;
        }
    }

    public void IncrementPlayerPoint()
    {
        playerPoints += 1;
        playerScore.text = playerPoints.ToString();
    }

    public void IncrementAIPoint()
    {
        AIPoints += 1;
        AIScore.text = AIPoints.ToString();
    }

    public int getPlayerScore()
    {
        return playerPoints;
    }

    public int getAIPoints()
    {
        return AIPoints;
    }

    private void Update()
    {
        playerPos = mainCamera.WorldToScreenPoint(player.position + offsetPos);
        playerScore.rectTransform.position = playerPos;

        AIPos = mainCamera.WorldToScreenPoint(AI.position - offsetPos);
        AIScore.rectTransform.position = AIPos;
    }
}
