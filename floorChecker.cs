using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorChecker : MonoBehaviour
{
    public character characterscript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            characterscript.onGround = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            characterscript.onGround = false;
        }
    }
}
