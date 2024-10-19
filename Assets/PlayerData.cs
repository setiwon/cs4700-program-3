/***************************************************************
*file: PlayerData.cs
*author: Joseph Setiawan and Carlos Castillos
*class: CS 4700 – Game Development
*assignment: Program 3
*date last modified: 10/18/2024
*
*purpose: This script manages the player status information: HP, Level, Experience points, and location.
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData instance; // Singleton instance
    private Vector3 lastPosition; // Variable to store the last position of the player

    // Player stats
    public int playerLevel = 1;
    public int playerHealth = 100;
    public int playerExperience = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    // Function to save player stats
    public void SavePlayerStats(int level, int health, int experience)
    {
        playerLevel = level;
        playerHealth = health;
        playerExperience = experience;
        Debug.Log("Player stats saved: Level: " + playerLevel + ", Health: " + playerHealth + ", Experience: " + playerExperience);
    }

    // Function to load player stats
    public void LoadPlayerStats()
    {
        Debug.Log("Player stats loaded: Level: " + playerLevel + ", Health: " + playerHealth + ", Experience: " + playerExperience);
    }
}
