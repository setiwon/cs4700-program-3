using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData instance; // Singleton instance
    private Vector3 lastPosition; // Variable to store the last position of the player

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Check if there is already an instance of PlayerData
        if (instance == null)
        {
            instance = this; // Set this as the instance
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy this instance if one already exists
        }
    }

    // Function to save the player's last position
    public void SavePosition(Vector3 position)
    {
        lastPosition = position; // Save the provided position
        Debug.Log("Player position saved: " + lastPosition);
    }

    // Function to retrieve the last saved position
    public Vector3 GetLastPosition()
    {
        Debug.Log("Position Retrieved: " + lastPosition); 
        return lastPosition; // Return the last saved position
    }
}
