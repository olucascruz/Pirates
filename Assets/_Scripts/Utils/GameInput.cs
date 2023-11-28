using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class responsible for receiving Inputs
/// </summary>
public class GameInput : MonoBehaviour
{
    private static GameInput instance;
    public static GameInput Instance { get{ return instance;} }

    private void Awake()
    {
        //Define singleton
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    public Vector2 InputAxisNormalized()
    {
        if (GameManager.Instance.State != GameState.PLAY) return Vector2.zero;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector2(x, y).normalized;
    }

    public bool JIsPressed()
    {
        if (GameManager.Instance.State != GameState.PLAY) return false;

        return Input.GetKeyDown(KeyCode.J);
    }
    public bool KIsPressed()
    {
        if (GameManager.Instance.State != GameState.PLAY) return false;

        return Input.GetKeyDown(KeyCode.K);

    }
    
    public bool SpaceIsPressed()
    {
        if (GameManager.Instance.State != GameState.STARTING) return false;
        return Input.GetKeyDown(KeyCode.Space);
    }
}
