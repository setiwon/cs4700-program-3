/***************************************************************
*file: PlayerMovement.cs
*author: Joseph Setiawan and Carlos Castillos
*class: CS 4700 – Game Development
*assignment: Program Assignment
*date last modified: 10/18/2024
*
*purpose: This script controls the player movement in a maze
*and handles random enemy encounters based on player input.
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    // Instance variables
    private float speed; // Movement speed of the player
    private Rigidbody2D rb; // Rigidbody2D component for physics
    private const float ENCOUNTER_CHANCE = 0.1f; // Probability of encountering an enemy (constant)

    //function: Start
    //purpose: This function is called before the first frame update
    //to initialize instance variables.
    void Start()
    {
        speed = 100f; // Initialize speed
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    //function: Update
    //purpose: This function is called once per frame to handle
    //player movement and encounter checks.
    void Update()
    {
        MovePlayer();
        CheckForEncounter();
    }

    //function: MovePlayer
    //purpose: This function handles player movement based on user input.
    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, y, 0);
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + (Vector2)movement); // Convert to Vector2
    }

    //function: CheckForEncounter
    //purpose: This function checks if an encounter should occur based
    //on a random chance when the player moves.
    private void CheckForEncounter()
    {
        if (Input.anyKeyDown) // Check if any key is pressed
        {
            if (Random.value < ENCOUNTER_CHANCE) // Random chance for encounter
            {
                StartEncounter();
            }
        }
    }

    //function: StartEncounter
    //purpose: This function is called when a random encounter occurs.
    private void StartEncounter()
    {
        // Trigger encounter logic here (e.g., start a battle)
        Debug.Log("Encounter started!");
    }
}
