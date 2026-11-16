using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float baseMoveForce;
    public int pointWorth;
    public ParticleSystem deathExplosionParticle;
    public AudioClip deathSound;

    [SerializeField] private float difficultyAdjustedMoveForce;
    private GameObject player;
    private GameManager gameManager;
    private PlayerController playerController;
    private AudioSource enemyAudio;
    private Rigidbody enemyRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        enemyAudio = Camera.main.GetComponent<AudioSource>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyRb = GetComponent<Rigidbody>();
        difficultyAdjustedMoveForce = baseMoveForce + (5 * Mathf.Floor(gameManager.DifficultyFactor));
    }

    private void FixedUpdate()
    {
        Vector3 towardsPlayer = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(towardsPlayer * difficultyAdjustedMoveForce * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController.invulnerable)
            {
                Debug.Log("Player has spawn invulnerability, repulsing enemy");
                int invulnerabilityRepulseForce = 25;
                Vector3 awayFromPlayer = (transform.position - player.transform.position).normalized;

                enemyRb.AddForce(awayFromPlayer * invulnerabilityRepulseForce, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("Enemy hit player");
                gameManager.DestroyPlayer();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Kill();
        }
    }

    private void Kill()
    {
        ParticleSystem deathExplosion = Instantiate(deathExplosionParticle, transform.position, deathExplosionParticle.transform.rotation);
        deathExplosion.Play();
        enemyAudio.PlayOneShot(deathSound, 0.5f);
        gameManager.AddScore(pointWorth);

        Destroy(gameObject);
    }
}
