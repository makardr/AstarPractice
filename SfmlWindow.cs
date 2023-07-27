﻿using Microsoft.VisualBasic;

namespace TestProject;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class SfmlWindow
{
    public static uint worldMultiplier = 100;

    private Map map;
    private Characters characters;
    private float interval = 1f; // 1 second
    private List<Shape> drawableStaticShapesList = new List<Shape>();
    private RenderWindow window;

    public SfmlWindow()
    {
        map = new Map();
        characters = new Characters();

        characters.CreateCharacter(map, 1, 1, 10, 1);
        characters.CreateCharacter(map, 9, 1, 0, 1);
        characters.CreateCharacter(map, 2, 0, 15, 6);
        characters.CreateCharacter(map, 16, 6, 17, 0);

        PopulateDrawablesList();

        InitializeWindow();
    }


    public void InitializeWindow()
    {
        // Create the SFML window
        window =
            new RenderWindow(
                new VideoMode(map.X * worldMultiplier + worldMultiplier, map.Y * worldMultiplier + worldMultiplier),
                "SFML Square");
        window.Closed += (sender, e) => window.Close();
        // Create a rectangle shape for the square

        //RectangleShape square = CreateSquare();


        // Create a clock to measure the elapsed time
        Clock clock = new Clock();


        // Create a timer to track the desired time interval
        float timer = 0f;


        // Game loop
        while (window.IsOpen)
        {
            // Process events
            window.DispatchEvents();

            // Calculate the elapsed time since the last frame
            float deltaTime = clock.Restart().AsSeconds();

            // Update the timer
            timer += deltaTime;


            // Check if the desired time interval has passed
            if (timer >= interval)
            {
                // Reset the timer
                timer = 0f;


                UpdateEntities();
                DrawWorld();
            }

            // Move the square to the left
            //square.Position -= new Vector2f(0, 100);
            //square.Position -= new Vector2f(movementSpeed * deltaTime, 0);

            // Clear the window
            window.Clear();

            // Draw the world
            DrawWorld();


            // Display the window
            window.Display();
        }
    }

    public static RectangleShape CreateSquare(int x, int y, int width, int height, uint multiplier, Color color)
    {
        RectangleShape square = new RectangleShape(new Vector2f(width, height));

        square.FillColor = color;
        square.Position = new Vector2f(x * multiplier, y * multiplier);

        return square;
    }

    public void DrawWorld()
    {
        foreach (Shape shape in drawableStaticShapesList)
        {
            window.Draw(shape);
        }

        foreach (Character character in characters.charactersList)
        {
            window.Draw(character.shape);
        }
    }


    private void PopulateDrawablesList()
    {
        int y = 0;
        foreach (string line in map.map)
        {
            int x = 0;
            foreach (char ch in line)
            {
                switch (ch)
                {
                    case '|':
                        drawableStaticShapesList.Add(CreateSquare(x, y, 100, 100, worldMultiplier, Color.Green));
                        break;
                    case '-':
                        drawableStaticShapesList.Add(CreateSquare(x, y, 100, 100, worldMultiplier, Color.Green));
                        break;
                }

                x += 1;
            }

            y += 1;
        }
    }


    private void UpdateEntities()
    {
        if (!characters.charactersList.Any(obj => obj.IsActive))
        {
            Console.WriteLine("Pathfinding finished");
            //break;
        }


        foreach (Character character in characters.charactersList)
        {
            character.MakeStep();
            map.PrintMap();
        }
    }
}