/***************************************************************
*file: BattleSystem.cs
*author: Joseph Setiawan and Carlos Castillos
*class: CS 4700 – Game Development
*assignment: Program 3
*date last modified: 10/18/2024
*
*purpose: This script manages the battle system for player and enemy actions
*in a turn-based format, handling fight, guard, and run actions.
****************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import the SceneManager
using System.Collections;

public class BattleSystem : MonoBehaviour
{
    // UI Buttons
    public Button fightButton;
    public Button guardButton;
    public Button runButton;

    // Player stats
    private int playerLevel;
    private int playerHealth;
    private int playerExperience;
    private const int EXP_TO_LEVEL_UP = 40; // Experience needed to level up
    private int enemyHealth = 50;
    private const int ENEMY_EXP_REWARD = 20; // Experience awarded for defeating an enemy
    private int playerAttack = 20;
    private int enemyAttack = 15;

    // Text UI elements
    public Text playerStatsText;
    public GameObject battleLogue; // GameObject for Battlelogue
    private Text battleLogueText; // Text component for Battlelogue

    private enum BattleState { PlayerTurn, EnemyTurn, BattleOver }
    private BattleState currentState;

    private bool isGuarding = false; // Track if the player is guarding

    void Start()
    {
        // Load player stats when starting the battle
        PlayerData playerData = FindObjectOfType<PlayerData>();
        if (playerData != null)
        {
            playerData.LoadPlayerStats(); // Load saved player stats
            playerLevel = playerData.playerLevel; // Update local stats
            playerHealth = playerData.playerHealth; // Update local stats
            playerExperience = playerData.playerExperience; // Update local stats
        }

        // Assign the appropriate functions to the buttons
        fightButton.onClick.AddListener(Fight);
        guardButton.onClick.AddListener(Guard);
        runButton.onClick.AddListener(Run);

        currentState = BattleState.PlayerTurn; // Set initial state
        UpdatePlayerStatsText(); // Initialize the stats display

        if (battleLogue != null)
        {
            battleLogueText = battleLogue.GetComponent<Text>(); // Get the Text component from Battlelogue
            if (battleLogueText != null)
            {
                battleLogueText.text = ""; // Clear the Battlelogue text at the start
            }
            else
            {
                Debug.LogError("Battlelogue does not have a Text component!");
            }
        }
        else
        {
            Debug.LogError("Battlelogue GameObject is not assigned!");
        }
        UpdateBattleLogue("Enemy encountered!");
        UpdateBattleLogue("Your turn");
    }

    
    // Function for the "Fight" button
    void Fight()
    {
        if (currentState != BattleState.PlayerTurn) return;

        enemyHealth -= playerAttack; // Player attacks enemy
        UpdateBattleLogue("You dealt " + playerAttack.ToString() + " damage!");

        // Check if the enemy is defeated
        if (enemyHealth <= 0)
        {
            currentState = BattleState.BattleOver;
            UpdateBattleLogue("Enemy defeated!");
            GainExperience(ENEMY_EXP_REWARD); // Gain experience for defeating the enemy
            UpdatePlayerStatsText(); // Update stats display after gaining experience
            StartCoroutine(Victory()); // Call victory function
        }
        else
        {
            currentState = BattleState.EnemyTurn; // Switch to enemy's turn
            StartCoroutine(EnemyTurn());
        }
    }

    // Gain experience and check for level up
    private void GainExperience(int amount)
    {
        playerExperience += amount;
        UpdateBattleLogue($"Player gained {amount} experience!");

        // Check if the player levels up
        if (playerExperience >= EXP_TO_LEVEL_UP)
        {
            LevelUp();
        }
    }

    // Handle player leveling up
    private void LevelUp()
    {
        playerLevel++;
        playerExperience -= EXP_TO_LEVEL_UP; // Reduce experience by required amount
        playerHealth = 100; // Restore HP to full 
        UpdateBattleLogue($"Player leveled up to Level {playerLevel}! HP restored to {playerHealth}.");
    }

    // Function for the "Guard" button
    void Guard()
    {
        if (currentState != BattleState.PlayerTurn) return;

        UpdateBattleLogue("Guard action selected");
        isGuarding = true; // Set guarding state
        currentState = BattleState.EnemyTurn; // Switch to enemy's turn
        StartCoroutine(EnemyTurn());
    }

    // Function for the "Run" button
    void Run()
    {
        if (currentState != BattleState.PlayerTurn) return;

        UpdateBattleLogue("Run action selected");
        float escapeChance = Random.value; // Generate a random value
        
        if (escapeChance < 0.5f) // 50% chance to escape
        {
            currentState = BattleState.BattleOver; // End the battle
            UpdateBattleLogue("Player escaped successfully!");

            // Save the player's last position before returning
            PlayerData playerData = FindObjectOfType<PlayerData>();
            if (playerData != null)
            {
                playerData.SavePosition(transform.position); // Save the current position
            }

            StartCoroutine(Victory()); // Call the Victory function to load the main screen
        }
        else
        {
            UpdateBattleLogue("Failed to escape!");
            currentState = BattleState.EnemyTurn; // Switch to enemy's turn
            StartCoroutine(EnemyTurn());
        }
    }

    // Coroutine for the enemy's turn
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1); // Simulate delay for enemy action
        UpdateBattleLogue("Enemy's turn to attack!");

        // If the player is guarding, they take no damage
        if (!isGuarding)
        {
            playerHealth -= enemyAttack; // Enemy attacks player
            UpdateBattleLogue("You suffered " + enemyAttack.ToString() + " damage!");

            if (playerHealth <= 0)
            {
                currentState = BattleState.BattleOver;
                UpdateBattleLogue("Player has been defeated!");
                StartCoroutine(RestartGame()); // Restart the game if the player is defeated
            }
        }
        else
        {
            UpdateBattleLogue("Player is guarding! No damage taken.");
            isGuarding = false; // Reset guarding state after the enemy's turn
        }

        // Switch back to player's turn
        currentState = BattleState.PlayerTurn;
        UpdatePlayerStatsText(); // Update stats display after each turn
    }

// Coroutine for Victory
private IEnumerator Victory()
{
    PlayerData playerData = FindObjectOfType<PlayerData>();
    if (playerData != null)
    {
        // Save player stats with current values
        playerData.SavePlayerStats(playerLevel, playerHealth, playerExperience);
    }

    UpdateBattleLogue("Returning to the main scene...");
    yield return new WaitForSeconds(3f); // Wait for 3 seconds
    SceneManager.LoadScene("MainScreen"); // Load the MainScreen scene
}


    // Restart the game
    private IEnumerator RestartGame()
    {
        UpdateBattleLogue("Restarting game...");
        yield return new WaitForSeconds(2f); // Wait before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Update the player stats text display
    private void UpdatePlayerStatsText()
    {
        playerStatsText.text = $"Player    Lvl   HP      Exp\n" +
                                $"{playerLevel}         {playerHealth}    {playerExperience}";
    }

    // Function to update the Battlelogue text
    private void UpdateBattleLogue(string message)
    {
        if (battleLogueText != null)
        {
            // Split the current text into lines
            string[] lines = battleLogueText.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            // If there are already 3 lines, remove the oldest one
            if (lines.Length >= 3)
            {
                // Remove the first line and keep the remaining lines
                string newLog = string.Join("\n", lines, 1, lines.Length - 1);
                battleLogueText.text = "\n" + newLog + "\n"; // Update the text with the new log
            }

            // Append the new message
            battleLogueText.text += message + "\n"; // Add the new message
            Debug.Log("Battle Logue Updated: " + message); // Log the message for debugging
        }
        else
        {
            Debug.LogError("battleLogueText is not assigned!"); // Debugging error message
        }
    }
}
