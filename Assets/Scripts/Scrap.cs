using UnityEngine;

public class Scrap : MonoBehaviour
{
    public GameObject promptText;

    void Start()
    {
        // Hide the pickup prompt initially
        promptText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player is now close
        if (other.GetComponent<PlanetPlayerControls>())
        {
            // Show the pickup prompt
            promptText.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If the player is nearby
        if (other.GetComponent<PlanetPlayerControls>())
        {
            // If the player hits the primary button
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("XRI_Right_PrimaryButton"))
            {
                // Let the player know they picked up this scrap
                FindObjectOfType<Player>().OnScrapCollected(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the player is no longer close
        if (other.GetComponent<PlanetPlayerControls>())
        {
            // Hide the pickup prompt
            promptText.SetActive(false);
        }
    }
}
