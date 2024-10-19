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
    private int playerLevel = 1;
    private int playerHealth = 100;
    private int playerExperience = 0;
    private const int EXP_TO_LEVEL_UP = 40; // Experience needed to level up
    private int enemyHealth = 50;
    private const int ENEMY_EXP_REWARD = 20; // Experience awarded for defeating an enemy
    private int playerAttack = 20;
    private int enemyAttack = 15;

    // Text UI elements
    public Text playerStatsText;

    private enum BattleState { PlayerTurn, EnemyTurn, BattleOver }
    private BattleState currentState;

    private bool isGuarding = false; // Track if the player is guarding

    void Start()
    {
        // Assign the appropriate functions to the buttons
        fightButton.onClick.AddListener(Fight);
        guardButton.onClick.AddListener(Guard);
        runButton.onClick.AddListener(Run);

        currentState = BattleState.PlayerTurn; // Set initial state
        UpdatePlayerStatsText(); // Initialize the stats display
    }

    // Function for the "Fight" button
    void Fight()
    {
        if (currentState != BattleState.PlayerTurn) return;

        Debug.Log("Fight action selected");
        enemyHealth -= playerAttack; // Player attacks enemy
        Debug.Log($"Enemy Health: {enemyHealth}");

        // Check if the enemy is defeated
        if (enemyHealth <= 0)
        {
            currentState = BattleState.BattleOver;
            Debug.Log("Enemy defeated!");
            GainExperience(ENEMY_EXP_REWARD); // Gain experience for defeating the enemy
            UpdatePlayerStatsText(); // Update stats display after gaining experience
            Victory(); // Call victory function
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
        Debug.Log($"Player gained {amount} experience!");

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
        Debug.Log($"Player leveled up to Level {playerLevel}! HP restored to {playerHealth}.");
    }

    // Function for the "Guard" button
    void Guard()
    {
        if (currentState != BattleState.PlayerTurn) return;

        Debug.Log("Guard action selected");
        isGuarding = true; // Set guarding state
        currentState = BattleState.EnemyTurn; // Switch to enemy's turn
        StartCoroutine(EnemyTurn());
    }

    // Function for the "Run" button
    void Run()
    {
        if (currentState != BattleState.PlayerTurn) return;

        Debug.Log("Run action selected");
        if (Random.value < 0.5f) // 50% chance to escape
        {
            currentState = BattleState.BattleOver; // End the battle
            Debug.Log("Player escaped successfully!");
        }
        else
        {
            Debug.Log("Failed to escape!");
            currentState = BattleState.EnemyTurn; // Switch to enemy's turn
            StartCoroutine(EnemyTurn());
        }
    }

    // Coroutine for the enemy's turn
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1); // Simulate delay for enemy action
        Debug.Log("Enemy's turn to attack!");

        // If the player is guarding, they take no damage
        if (!isGuarding)
        {
            playerHealth -= enemyAttack; // Enemy attacks player
            Debug.Log($"Player Health: {playerHealth}");

            if (playerHealth <= 0)
            {
                currentState = BattleState.BattleOver;
                Debug.Log("Player has been defeated!");
                RestartGame(); // Restart the game if the player is defeated
            }
        }
        else
        {
            Debug.Log("Player is guarding! No damage taken.");
            isGuarding = false; // Reset guarding state after the enemy's turn
        }

        // Switch back to player's turn
        currentState = BattleState.PlayerTurn;
        UpdatePlayerStatsText(); // Update stats display after each turn
    }

    // Victory function
    private void Victory()
    {
        Debug.Log("Victory! Returning to the main scene...");
        // You can add any victory message UI here if desired

        // Load the main scene
        SceneManager.LoadScene("MainScreen"); // Load the MainScreen scene
    }

    // Restart the game
    private void RestartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Update the player stats text display
    private void UpdatePlayerStatsText()
    {
        playerStatsText.text = $"Player    Lvl   HP      Exp\n" +
                                $"{playerLevel}         {playerHealth}    {playerExperience}";
    }
}
