using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerObject;
    public Vector3 cameraOffset;

    private GameManager gameManager;
    private AudioEchoFilter audioEchoEffect;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioEchoEffect = GetComponent<AudioEchoFilter>();
        audioEchoEffect.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerObject != null)
        {
            gameObject.transform.position = playerObject.transform.position + cameraOffset;
        }

        if (gameManager.gameStarted && gameManager.gameOver)
        {
            audioEchoEffect.enabled = true;
        }
    }
}
