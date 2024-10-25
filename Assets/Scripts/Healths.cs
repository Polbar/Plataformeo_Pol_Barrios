using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healths : MonoBehaviour
{
    private bool interactable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable)
        {
            // Obtén el componente del jugador y llama al método para aumentar la vida
            PlayerConroller player = FindObjectOfType<PlayerConroller>();
            if (player != null)
            {
                player.IncreaseHealth(1); // Aumenta la vida en 1
            }
            SoundManager.instance.PlaySFX(SoundManager.instance._audioSource, SoundManager.instance.healAudio);
            Destroy(gameObject); // Destruye el corazón
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            interactable = false;
        }
    }
}
