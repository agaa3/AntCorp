using UnityEngine;

public class TriggerSensor : MonoBehaviour
{
    public bool IsTriggering => triggering;
    [SerializeField] private bool triggering = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("climbing_walls"))
        {
            triggering = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("climbing_walls"))
        {
            triggering = false;
        }
    }
}