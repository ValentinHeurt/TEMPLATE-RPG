using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { mainMenu, play, pause, dialogue, victory, gameOver, menu, inventory, table}

public class GameManager : Singleton<GameManager>
{
    GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.play; } }
    public bool IsInDialogue { get { return m_GameState == GameState.dialogue; } }
    public bool IsInInventory { get { return m_GameState == GameState.inventory; } }

    public int CursorLocker = 0;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_GameState = GameState.play;
    }
    public void Dialogue()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.dialogue;
    }

    public void Inventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.inventory;
    }
    public void Table()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.table;
    }

}
