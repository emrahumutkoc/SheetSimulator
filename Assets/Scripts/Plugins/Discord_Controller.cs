using UnityEngine;
using Discord;

public class Discord_Controller : MonoBehaviour {
    public long applicationID;
    [Space]
    public string details = "Sheeting";
    public string state = "SS";
    [Space]
    public string largeImage = "game_logo";
    public string largeText = "Sheet Simulator";

    private Rigidbody rb;
    private long time;

    private static bool instanceExists;
    private Discord.Discord discord;

    void Awake() {
        if (!instanceExists) {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        } else if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Log in with the Application ID
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
/*        if (GameObject.FindWithTag("Player").GetComponent<Rigidbody>() != null) {
            rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        }*/
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    void Update() {
        try {
            discord.RunCallbacks();
        } catch {
            Destroy(gameObject);
        }
    }

    void LateUpdate() {
        UpdateStatus();
    }

    void UpdateStatus() {
        // Update Status every frame
        try {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity {
                Details = details,
                State = state,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = time
                }
            };

            activityManager.UpdateActivity(activity, (res) => {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        } catch {
            // If updating the status fails, Destroy the GameObject
            Destroy(gameObject);
        }
    }

/*    public void OnApplicationQuit() {
        var activityManager = discord.GetActivityManager();
        activityManager.ClearActivity((result) => {
            if (result == Discord.Result.Ok) {
                Debug.Log("Discord activity cleared successfully.");
            } else {
                Debug.LogWarning("Failed to clear Discord activity.");
            }
        });
        discord.Dispose();
    }*/
}
