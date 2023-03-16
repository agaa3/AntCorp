using UnityEngine;

public class TriggerSensor : MonoBehaviour
{
    private bool flag = false;
    public bool IsTriggering()
    {
        return flag;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("climbing_walls"))
        {
            flag = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        flag = false;
    }
}