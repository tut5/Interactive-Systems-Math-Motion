using UnityEngine;
using UnityEngine.InputSystem;
using Valve.VR;

public class TrackingManager : MonoBehaviour
{
    [Header("Settings")]
    [Range(1, 4)] public int numberOfPlayers = 4;  // Public field (no [SerializeField] needed)
    public bool enableRotation = false;             // Public field
    public bool enableYAxis = false;               // Public field

    [Header("Player Assignments")]
    public GameObject[] playerObjects;  // Public array for player GameObjects

    private int selectedPlayerIndex = 0;
    private bool isTrackingEnabled = true;

    void Start()
    {
        // Initialize players (max 4)
        if (playerObjects.Length > 4)
            Debug.LogError("Maximum 4 players allowed!");
    }

    void Update()
    {
        // Toggle tracking with a key (e.g., T)
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            isTrackingEnabled = !isTrackingEnabled;
            Debug.Log("Tracking: " + isTrackingEnabled);
        }

        if (isTrackingEnabled)
        {
            // Get tracking data from SteamVR (implement tracker logic here)
            UpdatePlayersFromTrackers();
        }
        else
        {
            // Keyboard input for testing
            HandlePlayerSelection();
            HandleKeyboardMovement();
        }
    }

    // Call this method to update player positions from trackers
    private void UpdatePlayersFromTrackers()
    {
        // Example: Get tracker data (adjust for your setup)
        for (int i = 0; i < numberOfPlayers; i++)
        {
            var tracker = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose");
            if (tracker.GetDeviceToSourceIndex((SteamVR_Input_Sources)i) != -1)
            {
                Vector3 position = tracker.GetLocalPosition((SteamVR_Input_Sources)i);
                Quaternion rotation = tracker.GetLocalRotation((SteamVR_Input_Sources)i);

                if (!enableYAxis) position.y = 0;
                if (!enableRotation) rotation = Quaternion.identity;

                playerObjects[i].GetComponent<PlayerMovement>()
                    .SetPosition(position);
                playerObjects[i].GetComponent<PlayerMovement>()
                    .SetRotation(rotation);
            }
        }
    }

    private void HandlePlayerSelection()
    {
        // Select player with keys 1-4
        for (int i = 0; i < 4; i++)
        {
            if (Keyboard.current.digitKeys[i].wasPressedThisFrame)
            {
                selectedPlayerIndex = i;
                Debug.Log("Selected Player: " + (i + 1));
            }
        }
    }

    private void HandleKeyboardMovement()
    {
        // WASD movement for XZ axis
        Vector3 moveInput = new Vector3(
            Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
            enableYAxis ? Keyboard.current.eKey.ReadValue() - Keyboard.current.qKey.ReadValue() : 0,
            Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
        );

        playerObjects[selectedPlayerIndex].GetComponent<PlayerMovement>()
            .SetPosition(moveInput * Time.deltaTime * 5f); // Adjust speed as needed
    }
}