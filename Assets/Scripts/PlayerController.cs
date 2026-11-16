using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveForce;
    public float weaponCooldown;
    public bool invulnerable;
    public GameObject bulletPrefab;
    public InputAction moveAction;
    public InputAction shootAction;
    public Material normalMaterial;
    public Material invulnMaterial;
    public ParticleSystem deathExplosionParticle;
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip respawnSound;

    private Rigidbody playerRb;
    private Vector2 moveInput;
    private Vector2 shootInput;
    private AudioSource playerAudio;
    private bool canShoot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = Camera.main.GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        moveAction.Enable();
        shootAction.Enable();
        canShoot = true;
    }

    void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (shootAction.IsPressed())
        {
            shootInput = shootAction.ReadValue<Vector2>();
            Shoot(shootInput);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void SetInvulnerability(bool isInvulnerable)
    {
        invulnerable = isInvulnerable;
        Material playerMaterial = GetComponent<MeshRenderer>().material;

        if (isInvulnerable)
        {
            GetComponent<MeshRenderer>().material = invulnMaterial;
            Debug.Log("Player is invulnerable");
        }
        else
        {
            GetComponent<MeshRenderer>().material = normalMaterial;
            Debug.Log("Player is vulnerable");
        }
    }

    public void Kill()
    {
        ParticleSystem deathExplosion = Instantiate(deathExplosionParticle, transform.position, deathExplosionParticle.transform.rotation);
        deathExplosion.Play();
        playerAudio.PlayOneShot(deathSound, 1f);
        gameObject.SetActive(false);
    }

    public void Respawn(Vector3 respawnPos)
    {
        transform.position = respawnPos;
        playerAudio.PlayOneShot(respawnSound, 1f);
        gameObject.SetActive(true);
    }

    // Moves player in the direction of WASD keys
    private void MovePlayer()
    {
        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        playerRb.AddForce(movementDirection * moveForce * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            Vector3 directionOnXZPlane = new Vector3(direction.x, 0, direction.y);
            float angleOfDirection = Vector3.Angle(Vector3.forward, directionOnXZPlane);
            float adjustedAngleOfDirection = -angleOfDirection * Mathf.Sign(direction.x);

            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(90, 0, adjustedAngleOfDirection));
            playerAudio.PlayOneShot(shootSound, 0.5f);
            canShoot = false;

            StartCoroutine(ShootCooldownRoutine());
        }
    }

    private IEnumerator ShootCooldownRoutine()
    {
        yield return new WaitForSeconds(weaponCooldown);
        canShoot = true;
    }

}
