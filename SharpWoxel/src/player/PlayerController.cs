﻿using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.World.Blocks;

namespace SharpWoxel.Player;

class PlayerController
{
    public Util.Camera Camera { get; private set; }
    public float PlayerSpeed { get; set; }
    public float Sensetivity { get; set; }
    public Inventory.PlayerInventory PlayerInventory { get; private set; }

    public PlayerController(Util.Camera camera, Inventory.PlayerInventory playerInventory)
    {
        Camera = camera;
        PlayerSpeed = 10.0f;
        Sensetivity = 0.25f;
        PlayerInventory = playerInventory;
        PlayerInventory.ChangeInventoryItem(0, new GrassBlock());
        PlayerInventory.ChangeInventoryItem(1, new DirtBlock());
        PlayerInventory.ChangeInventoryItem(2, new WoodBlock());
    }

    private void HandleInput(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
    {
        // Keyboard inputs
        if (keyInput.IsKeyDown(Keys.W))
        {
            Camera.Position += Camera.Front * (float)deltaTime * PlayerSpeed;
        }

        if (keyInput.IsKeyDown(Keys.S))
        {
            Camera.Position -= Camera.Front * (float)deltaTime * PlayerSpeed;
        }

        if (keyInput.IsKeyDown(Keys.A))
        {
            Camera.Position -= Camera.Right * (float)deltaTime * PlayerSpeed;
        }

        if (keyInput.IsKeyDown(Keys.D))
        {
            Camera.Position += Camera.Right * (float)deltaTime * PlayerSpeed;
        }

        if (keyInput.IsKeyDown(Keys.Space))
        {
            Camera.Position += Camera.Up * (float)deltaTime * PlayerSpeed;
        }

        if (keyInput.IsKeyDown(Keys.LeftControl))
        {
            Camera.Position -= Camera.Up * (float)deltaTime * PlayerSpeed;
        }

        if (keyInput.IsKeyDown(Keys.LeftShift))
        {
            PlayerSpeed = 20.0f;
        }
        else PlayerSpeed = 10.0f;

        // Mouse inputs
        Camera.Yaw += mouseInput.Delta.X * Sensetivity;
        Camera.Pitch -= mouseInput.Delta.Y * Sensetivity;

        // Inventory selection
        if (mouseInput.ScrollDelta.Y < 0)
        {
            PlayerInventory.SelectNext();
        }
        else if (mouseInput.ScrollDelta.Y > 0)
        {
            PlayerInventory.SelectPrevious();
        }
    }

    public void Update(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
    {
        HandleInput(deltaTime, keyInput, mouseInput);
    }
}
