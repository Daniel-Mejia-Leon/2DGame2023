using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisScriptable : MonoBehaviour
{
    [SerializeField] public items itemInformation;
    [SerializeField] public AudioSource takenSound;

    // THIS METHOD IS USED IN THE TAKE ANIMATION
    private void destroyMethod()
    {
        Destroy(gameObject);
    }

    public void touchedByPlayer()
    {
        GetComponent<Animator>().SetBool("taken", true);
        takenSound.Play();
    }
}
