using System;
using UnityEngine;

public class PlayerControllerSimple : MonoBehaviour
{
    private void Start()
    {
        
    }
    
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        Debug.Log(x);
        if(x > 0.01)
        {
            x = 1;
        }
        else if (x < -0.1)
        {
            x = -1;
        }
        else
        {
            x = 0;
        }
        x = x * Time.deltaTime * 150.0f;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z*20);
    }
}
