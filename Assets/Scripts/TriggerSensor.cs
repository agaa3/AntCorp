using System.Collections.Generic;
using UnityEngine;

public class TriggerSensor : MonoBehaviour
{
    public string[] TagWhitelist;
    public IReadOnlyList<Collider2D> CollidersInside => inside;
    public bool IsTriggering { get; private set; }

    [SerializeField] private List<Collider2D> inside;


    public void OnTriggerEnter2D(Collider2D other)
    {
        foreach(string s in TagWhitelist)
        {
            if (other.CompareTag(s))
            {
                Debug.Log(other.gameObject.name);
                IsTriggering = true;
                inside.Add(other);
                break;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        foreach (string s in TagWhitelist)
        {
            if (other.CompareTag(s))
            {
                inside.Remove(other);
                if (inside.Count == 0)
                {
                    IsTriggering = false;
                }
                break;
            }
        }
    }
}