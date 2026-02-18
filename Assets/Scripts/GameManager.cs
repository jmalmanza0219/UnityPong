using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public NetworkVariable<int> leftScore = new NetworkVariable<int>(0);
    public NetworkVariable<int> rightScore = new NetworkVariable<int>(0);

    public NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);
    public NetworkVariable<int> winner = new NetworkVariable<int>(0);
    public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);

    [SerializeField] private int targetScore = 5;
    [SerializeField] private BallMovement ball;

    // Prevent double scoring from multiple triggers in the same moment
    private bool canScore = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        gameStarted.Value = false;
        ball.StopBall();

        leftScore.Value = 0;
        rightScore.Value = 0;
        
        gameOver.Value = false;
        winner.Value = 0;

        
        
    }

    public void AddLeftScore()
    {
        if (!IsServer || gameOver.Value || !canScore) return;

        canScore = false;
        leftScore.Value++;

        CheckGameOverOrServe(-1f);
        Invoke(nameof(EnableScoring), 0.25f);
    }

    public void AddRightScore()
    {
        if (!IsServer || gameOver.Value || !canScore || !gameStarted.Value) return;

        canScore = false;
        rightScore.Value++;

        CheckGameOverOrServe(1f);
        Invoke(nameof(EnableScoring), 0.25f);
    }

    private void CheckGameOverOrServe(float serveXDir)
    {
        if(leftScore.Value >= targetScore)
        {
            EndGame(leftWon: true);
            return;
        }

        if(rightScore.Value >= targetScore)
        {
            EndGame(leftWon: false);
            return;
        }
        Serve(serveXDir);
    }

    private void EndGame(bool leftWon)
    {
        gameOver.Value = true;
        winner.Value = leftWon ? 1 : 2;
        ball.StopBall();
    }

   private void Serve(float xDir)
    {
        float y = Random.Range(-0.5f, 0.5f);
        ball.Serve(new Vector2(xDir, y), 5f);
    }

    private void EnableScoring()
    {
        if(gameOver.Value) return;
        canScore = true;
    }

    public void StartGame()
    {
        if(!IsServer) return;

        gameStarted.Value = true;

        leftScore.Value = 0;
        rightScore.Value = 0;

        gameOver.Value = false;
        winner.Value = 0;

        canScore = true;

        float xDir = Random.value < 0.5f ? -1f : 1f;
        float yDir = Random.Range(-0.5f, 0.5f);
        ball.Serve(new Vector2(xDir, yDir), 5f);
    }
}
