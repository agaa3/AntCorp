using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    public static Player Main;

    private void Awake()
    {
        if (Main == null)
        {
            Main = this;
        }
        else if (Main != this)
        {
            Destroy(this.gameObject);
        }
    }
}
