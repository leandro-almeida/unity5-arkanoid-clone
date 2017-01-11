using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

    public Player player;
    public GameObject ballPrefab;
    public float powerUpSpawnRateChance;// entre 0 e 1
    public float offsetYBola;
    public Text scoreText;
    public Text gameStateText;
    public GameObject[] powerUps;
    public AudioClip audioClipBrick;
    public AudioClip audioClipClick;
    public AudioClip audioClipLooseLife;
    public AudioClip audioClipPowerUp;

    private GameStateEnum currentGameState;
    private int scorePoints;
    private int numberOfBalls;

    // Use this for initialization
    void Start()
    {
        currentGameState = GameStateEnum.START;
        numberOfBalls = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteGameState();
    }

    /// <summary>
    /// Executa o game loop principal
    /// </summary>
    void ExecuteGameState()
    {
        switch (currentGameState)
        {
            case GameStateEnum.START:
                DoStart();
                break;

            case GameStateEnum.PLAYING:
                DoPlaying();
                break;

            case GameStateEnum.PAUSED:
                DoPaused();
                break;

            case GameStateEnum.WIN:
                DoWin();
                break;

            case GameStateEnum.GAMEOVER:
                DoGameOver();
                break;
        }
    }

    public void ChangeGameState(GameStateEnum newGameState)
    {
        currentGameState = newGameState;

        if (newGameState == GameStateEnum.GAMEOVER)
        {
            Time.timeScale = 0;
            gameStateText.text = "GAME OVER!!! :(";
        }
    }

    public GameStateEnum GetCurrentGameState()
    {
        return currentGameState;
    }

    /// <summary>
    /// Start - Cena Inicial do Jogo
    /// </summary>
    private void DoStart()
    {
        // configurar valores do jogo aqui

        // inicia jogo
        ChangeGameState(GameStateEnum.PLAYING);
    }

    /// <summary>
    /// Playing - Jogo em execuçao
    /// </summary>
    private void DoPlaying()
    {

        HandleBasicInputs();
        //player.HandleMovement();
        //ball.HandleBallInitialTap();

    }

    /// <summary>
    /// Paused - Jogo pausado
    /// </summary>
    private void DoPaused()
    {
        HandleBasicInputs();
    }

    /// <summary>
    /// Win - Jogador venceu
    /// </summary>
    private void DoWin()
    {
        HandleBasicInputs();
    }

    /// <summary>
    /// GameOver - Jogador perdeu
    /// </summary>
    private void DoGameOver()
    {
        HandleBasicInputs();
    }

    /// <summary>
    /// Trata os inputs basicos - ESC para pausar e retomar o jogo
    /// </summary>
    private void HandleBasicInputs()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameStateEnum.PLAYING)
            {
                Debug.Log("GAME PAUSED");
                gameStateText.text = "JOGO PAUSADO!";
                ChangeGameState(GameStateEnum.PAUSED);
                Time.timeScale = 0;
            }
            else if (currentGameState == GameStateEnum.PAUSED)
            {
                Debug.Log("GAME RESUMED");
                gameStateText.text = "Use A/D para mover o jogador!";
                ChangeGameState(GameStateEnum.PLAYING);
                Time.timeScale = 1;
            }
            else if (currentGameState == GameStateEnum.GAMEOVER)
            {
                gameStateText.text = "Use A/D para mover o jogador!";
                Time.timeScale = 1;
                ChangeGameState(GameStateEnum.START);
                Application.LoadLevel("Gameplay");
            }
            // adicionar outros estados futuramente aqui
        }
        else if (Input.GetKey(KeyCode.Menu))
        {
            if (currentGameState == GameStateEnum.GAMEOVER)
            {
                Application.Quit();
            }
        }

    }

    public void SpawnRandomPowerUp(Vector2 initialPosition)
    {
        Instantiate(powerUps[Random.Range(0, powerUps.Length)], initialPosition, Quaternion.identity);
    }

    public void IncreaseScore(int factor)
    {
        scorePoints += 10 * factor;
        scoreText.text = scorePoints.ToString();
    }

    public int GetNumberOfBalls()
    {
        return numberOfBalls;
    }

    public void DecreaseNumberOfBalls()
    {
        numberOfBalls--;
    }

    public void SpawnNewBall()
    {
        Debug.Log("spawning new ball");
        numberOfBalls++;
        Instantiate(ballPrefab, new Vector3(player.transform.position.x, player.transform.position.y + offsetYBola), Quaternion.identity);
    }
}
