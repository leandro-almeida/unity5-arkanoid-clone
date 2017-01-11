using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public GameController gameController;
    public float velocidadeTouch;
    public float velocidadeKeyboard;
    public int lives;
    public GameObject lifePrefab;

    private float direcao;
    private GameObject livesContainer;
    private GameObject[] lifeObjects;
    private Rigidbody2D rb;
    private Animator playerAnim;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        livesContainer = GameObject.Find("livesContainer");
        lifeObjects = new GameObject[lives];
        Vector3 pos;
        for (int i = 0; i < lives; i++)
        {
            pos = new Vector3(livesContainer.transform.position.x + (i * lifePrefab.GetComponent<BoxCollider2D>().size.x), livesContainer.transform.position.y);
            lifeObjects[i] = Instantiate(lifePrefab, pos, Quaternion.identity) as GameObject;
            lifeObjects[i].transform.parent = livesContainer.transform;
        }
    }

    void Update()
    {
        #if UNITY_STANDALONE
        if (gameController.GetCurrentGameState() == GameStateEnum.PLAYING)
        {
            direcao = Input.GetAxisRaw("Horizontal");
        }
        #endif
    }

    void FixedUpdate()
    {
        if (gameController.GetCurrentGameState() == GameStateEnum.PLAYING)
        {

            #if UNITY_STANDALONE
            rb.velocity = new Vector2(direcao * velocidadeKeyboard, 0);
            #endif


            #if UNITY_ANDROID
            if (Lean.LeanTouch.SoloDragDelta.x != Vector2.zero.x)
            {
                rb.velocity = new Vector2(Lean.LeanTouch.SoloDragDelta.x * velocidadeTouch, 0);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            #endif
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "powerup_tamanho")
        {
            // O parametro 0 (zero) indica para voltar ao inicio da animacao (reinicia se ja estiver tocando)
            // -1 eh a layer

            if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("anim_powerup_tamanho"))
            {
                playerAnim.Play("anim_powerup_tamanho", -1, 0.05f);
            }
            else
            {
                playerAnim.Play("anim_powerup_tamanho");
            }

            audioSource.PlayOneShot(gameController.audioClipPowerUp);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "powerup_vida")
        {
            IncreaseLife();
            audioSource.PlayOneShot(gameController.audioClipPowerUp);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "powerup_bola")
        {
            gameController.SpawnNewBall();
            audioSource.PlayOneShot(gameController.audioClipPowerUp);
            Destroy(other.gameObject);
        }
    }

    public int LooseLife()
    {
        lives--;
        lifeObjects[lives].SetActive(false);

        if (lives <= 0)
        {
            Debug.Log("GAME OVER!");
            gameController.ChangeGameState(GameStateEnum.GAMEOVER);
        }

        return lives;
    }

    public void IncreaseLife()
    {
        if (lives >= 0 && lives < 3)
        {
            lives++;
            lifeObjects[lives - 1].SetActive(true);
        }
    }

}
