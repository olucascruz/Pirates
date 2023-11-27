using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private static GameInput instance;
    public static GameInput Instance { get{ return instance;} }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Vector2 InputAxisNormalized()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector2(x, y).normalized;
    }

    public bool JIsPressed()
    {
        return Input.GetKeyDown(KeyCode.J);
    }
    public bool KIsPressed()
    {
        return Input.GetKeyDown(KeyCode.K);

    }
    
    public bool SpaceIsPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
