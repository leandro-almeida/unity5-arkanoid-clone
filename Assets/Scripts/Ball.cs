using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float velocidadeBola;

    private Rigidbody2D rb;
    private bool bolaParada;
    private AudioSource audioSource;
    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        Debug.Log("start ball");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController == null)
        {
            Debug.Log("GameController NULL!");
        }
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        bolaParada = true;
    }

    void Update()
    {
        if (gameController.GetCurrentGameState() == GameStateEnum.PLAYING)
        {
            if (Input.anyKey && bolaParada)
            {
                AplicarForcaInicialBola();
            }
        }
    }

    void FixedUpdate()
    {

        if (gameController.GetCurrentGameState() == GameStateEnum.PLAYING)
        {

            // verifica se a bola esta com velocidade baixa ou nula na vertical
            if (!bolaParada)
            {
                if (rb.velocity.y < 2 && rb.velocity.y > -2)
                {
                    rb.gravityScale = 3;
                }
                else
                {
                    rb.gravityScale = 0;
                }
            }

        }
    }

    public void AplicarForcaInicialBola()
    {
        rb.velocity = new Vector2(Random.Range(-2f, 2f), velocidadeBola);
        bolaParada = false;
    }

    // Resultado indica a nova direçao da bola dependendo de onde ela colidir com a plataforma (jogador)
    // valor negativo direciona para a esquerda no eixo x
    // valor positivo para a direita
    // zero direciona verticalmente (reto)
    float ColisaoBola(Vector2 posicaoBola, Vector2 posicaoJogador, float larguraJogador)
    {
        return (posicaoBola.x - posicaoJogador.x) / larguraJogador;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "brick")
        {
            HandleBrickHit(other);
        }
        else if (other.gameObject.tag == "Player")
        {
            HandlePlayerHit(other);
        }
    }

    void HandleBrickHit(Collision2D other)
    {
        Brick b = other.gameObject.GetComponent<Brick>();

        if (b.BallHit() == 0)
        {

            // sorteia chance de power up
            if (Random.Range(0f, 1f) <= gameController.powerUpSpawnRateChance)
            {
                gameController.SpawnRandomPowerUp(other.gameObject.transform.position);
            }

            gameController.IncreaseScore(b.lives);
            audioSource.PlayOneShot(gameController.audioClipBrick);
            Destroy(other.gameObject);

        }
        else
        {
            audioSource.PlayOneShot(gameController.audioClipClick);

            // modifica o alpha
            SpriteRenderer sr = other.gameObject.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a * 0.6f);
        }
    }

    void HandlePlayerHit(Collision2D other)
    {
        float result = ColisaoBola(transform.position, other.transform.position, other.collider.bounds.size.x);
        Vector2 novaDirecao = new Vector2(result, 1).normalized;
        rb.velocity = novaDirecao * velocidadeBola;
    }

    public void HandleBallFall()
    {
        if (gameController.GetNumberOfBalls() > 1) // tem mais bolas no jogo, apenas destroi
        {
            gameController.DecreaseNumberOfBalls();
            Destroy(this);
        }
        else // so tem uma bola, faz perder a vida
        {
            audioSource.PlayOneShot(gameController.audioClipLooseLife);
            bolaParada = true;
            rb.velocity = new Vector2(0, 0);
            if (gameController.player.LooseLife() > 0)
            {
                transform.position = new Vector2(gameController.player.transform.position.x, gameController.player.transform.position.y + gameController.offsetYBola);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
