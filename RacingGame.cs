using UnityEngine;
using UnityEngine.UI;

public class RacingGame : MonoBehaviour
{
    // Player Car Variables
    public GameObject playerCar;
    public float playerSpeed = 20f;
    public float playerTurnSpeed = 50f;

    // AI Car Variables
    public GameObject aiCar;
    public Transform[] aiWaypoints;
    public float aiSpeed = 15f;
    public float aiWaypointRadius = 1f;
    private int currentWaypointIndex = 0;

    // Game Management Variables
    public Text lapCounterText;
    public Text winnerText;
    public int totalLaps = 3;
    private int playerLaps = 0;
    private int aiLaps = 0;

    void Start()
    {
        // Initialize UI
        winnerText.gameObject.SetActive(false);
        UpdateLapCounter();
    }

    void Update()
    {
        // Player Controls
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerCar.transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime * verticalInput);
        playerCar.transform.Rotate(Vector3.up, playerTurnSpeed * Time.deltaTime * horizontalInput);

        // AI Controls
        if (aiWaypoints.Length > 0)
        {
            Vector3 direction = aiWaypoints[currentWaypointIndex].position - aiCar.transform.position;
            aiCar.transform.Translate(direction.normalized * aiSpeed * Time.deltaTime, Space.World);

            Quaternion rotation = Quaternion.LookRotation(direction);
            aiCar.transform.rotation = Quaternion.Slerp(aiCar.transform.rotation, rotation, Time.deltaTime * aiSpeed);

            if (direction.magnitude < aiWaypointRadius)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % aiWaypoints.Length;
            }
        }
    }

    // Checkpoint Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCheckpoint"))
        {
            playerLaps++;
            CheckWinner();
            UpdateLapCounter();
        }
        else if (other.CompareTag("AICheckpoint"))
        {
            aiLaps++;
            CheckWinner();
        }
    }

    void CheckWinner()
    {
        if (playerLaps >= totalLaps)
        {
            winnerText.text = "You Win!";
            winnerText.gameObject.SetActive(true);
            Time.timeScale = 0; // Pause the game
        }
        else if (aiLaps >= totalLaps)
        {
            winnerText.text = "AI Wins!";
            winnerText.gameObject.SetActive(true);
            Time.timeScale = 0; // Pause the game
        }
    }

    void UpdateLapCounter()
    {
        lapCounterText.text = $"Laps: {playerLaps}/{totalLaps}";
    }
}
