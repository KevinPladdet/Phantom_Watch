using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScript : MonoBehaviour
{
    public static WinScript instance;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    public TextMeshProUGUI timeText;
    public GameObject winObject;
    private int totalGameMinutes = 12; // Total duration of the game in real-world minutes (adjustable in inspector)

    public Color defaultColor;
    public Color camerasColor;

    public float elapsedTime = 0f; // Tracks elapsed real-time
    public int currentHour = 12; // Start at 12 AM
    public int currentMinute = 0; // Start at 0 minutes
    private const int maxHours = 6; // 6 AM marks the end
    private bool gamePaused = false; // Tracks if the game is paused
    bool started = false;

    private float secondsPerInGameMinute; // Calculated based on totalGameMinutes

    [SerializeField] AudioClip bellSound;

    public void StartScript()
    {
        started = true;
    }

    public void PlaySound()
    {
        AudioPool.Instance.PlaySound(bellSound);
    }

    void Start()
    {
        AnomalyController.instance.playerDead.AddListener(LooseGame);

        timeText.color = defaultColor;

        if (winObject != null)
        {
            winObject.SetActive(false); // Ensure the win object is disabled at the start
        }


        // Calculate how many real seconds correspond to 1 in-game minute
        secondsPerInGameMinute = (totalGameMinutes * 60f) / (maxHours * 60);
        UpdateTimeDisplay();
    }

    public void ToggleInCam(bool inCam)
    {
        timeText.color = inCam ? camerasColor : defaultColor;
    }

    void Update()
    {
        if (!started) { return; }
        if (gamePaused) return;

        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the required real-time seconds for 1 in-game minute have passed
        if (elapsedTime >= secondsPerInGameMinute)
        {
            elapsedTime = 0f; // Reset elapsed time
            IncrementTime();
        }
    }

    void IncrementTime()
    {
        // Increment the in-game minute
        currentMinute++;

        if (currentMinute >= 60)
        {
            // Increment the hour and reset minutes
            currentMinute = 0;
            currentHour++;

            if(bellSound != null)
            {
                AudioPool.Instance.PlaySound(bellSound);
            }

            // Handle hour progression from 12 to 1
            if (currentHour > 12)
            {
                currentHour -= 12; // Convert 13, 14, etc., to 1, 2, etc.
            }

            Debug.Log("Hour changed to: " + currentHour + " AM");
        }

        // Check if it's 6:00 AM
        if (currentHour == maxHours && currentMinute == 0)
        {
            EndGame();
            return;
        }

        UpdateTimeDisplay(); 
    }

    void UpdateTimeDisplay()
    {
        string formattedHour = currentHour.ToString("D2");
        string formattedMinute = currentMinute.ToString("D2");
        timeText.text = $"{formattedHour}:{formattedMinute} AM";
    }

    public GameObject LooseUI;

    void LooseGame()//lose the game
    {
        bellSound = null;
        Time.timeScale = 0;
        gamePaused = true;

        LooseUI.SetActive(true);
         
        LooseUI.GetComponent<EndingStatScript>().SetData();
        timeText.gameObject.SetActive(false);
    }

    void EndGame()//win the game
    {
        bellSound = null;
        Time.timeScale = 0;
        gamePaused = true;

        if (winObject != null)
        {
            winObject.SetActive(true); // Enable the win object
            winObject.GetComponent<EndingStatScript>().SetData();
        }
        timeText.gameObject.SetActive(false);

        Debug.Log("Game Over! You win!");
    }
    
}
