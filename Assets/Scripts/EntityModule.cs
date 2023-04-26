using UnityEngine;

public class EntityModule<T> : MonoBehaviour where T : Entity
{
    public T Parent;
}
