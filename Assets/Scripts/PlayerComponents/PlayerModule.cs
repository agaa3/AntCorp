using UnityEngine;

public abstract class PlayerModule : EntityModule<Player>
{
    public bool Initialized { get; private set; } = false;

    public void Initialize(Player player)
    {
        if (Parent != null)
        {
            return;
        }
        Parent = player;
        Initialized = true;
        OnInitialize(TimeState.Create());
    }
    public virtual void OnInitialize(TimeState time) { }
    public virtual void OnUpdate(TimeState time) { }
    public virtual void OnFixedUpdate(TimeState time) { }
    public virtual void OnLateUpdate(TimeState time) { }
}
