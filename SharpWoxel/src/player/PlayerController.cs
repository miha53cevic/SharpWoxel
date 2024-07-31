using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.player.inventory;
using SharpWoxel.util;
using SharpWoxel.world.blocks;

namespace SharpWoxel.player;

internal class PlayerController
{
    public PlayerController(Camera camera, PlayerInventory playerInventory)
    {
        Camera = camera;
        PlayerSpeed = 10.0f;
        Sensetivity = 0.25f;
        PlayerInventory = playerInventory;
        PlayerInventory.ChangeInventoryItem(0, new GrassBlock());
        PlayerInventory.ChangeInventoryItem(1, new DirtBlock());
        PlayerInventory.ChangeInventoryItem(2, new WoodBlock());
    }

    public Camera Camera { get; }
    public float PlayerSpeed { get; set; }
    public float Sensetivity { get; set; }
    public PlayerInventory PlayerInventory { get; }

    private void HandleInput(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
    {
        // Keyboard inputs
        if (keyInput.IsKeyDown(Keys.W)) Camera.Position += Camera.Front * (float)deltaTime * PlayerSpeed;

        if (keyInput.IsKeyDown(Keys.S)) Camera.Position -= Camera.Front * (float)deltaTime * PlayerSpeed;

        if (keyInput.IsKeyDown(Keys.A)) Camera.Position -= Camera.Right * (float)deltaTime * PlayerSpeed;

        if (keyInput.IsKeyDown(Keys.D)) Camera.Position += Camera.Right * (float)deltaTime * PlayerSpeed;

        if (keyInput.IsKeyDown(Keys.Space)) Camera.Position += Camera.Up * (float)deltaTime * PlayerSpeed;

        if (keyInput.IsKeyDown(Keys.LeftControl)) Camera.Position -= Camera.Up * (float)deltaTime * PlayerSpeed;

        PlayerSpeed = keyInput.IsKeyDown(Keys.LeftShift) ? 20.0f : 10.0f;

        // Mouse inputs
        Camera.Yaw += mouseInput.Delta.X * Sensetivity;
        Camera.Pitch -= mouseInput.Delta.Y * Sensetivity;

        switch (mouseInput.ScrollDelta.Y)
        {
            // Inventory selection
            case < 0:
                PlayerInventory.SelectNext();
                break;
            case > 0:
                PlayerInventory.SelectPrevious();
                break;
        }
    }

    public void Update(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
    {
        HandleInput(deltaTime, keyInput, mouseInput);
    }
}