using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public ItemType Type;
    public uint Amount;

    private const float frequency = 4.25f;
    private const float amplitude = 0.1f;
    private const float offset = 0.1f;
    private const bool applyDelay = true;


    private Vector3 origin;

    private void Awake(){
        origin = transform.position;
    }
    private void Update(){
        transform.position = origin + transform.up * offset + transform.up * amplitude * Mathf.Cos((applyDelay ? this.GetInstanceID() : 0) + Time.time * frequency);
    }
}
