using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    public static Player Main;

    [Header("Modules")]
    public PlayerController Controller;
    public PlayerPerception Perception;
    public PlayerGatherer Gatherer;
    public PlayerModel Model;
    public PlayerModule[] Modules;
    [Header("Components")]
    public Rigidbody2D UseRigidbody;
    public BoxCollider2D Collider;

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
        InitComponents();
        InitModules();
    }
    private void Update()
    {
        ModulesUpdate();
    }
    private void FixedUpdate()
    {
        ModulesFixedUpdate();
    }
    private void LateUpdate()
    {
        ModulesLateUpdate();
    }

    private void InitComponents()
    {
        UseRigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
    }
    #region Modules
    private void InitModules()
    {
        Modules = gameObject.GetComponentsInChildren<PlayerModule>();
        foreach (PlayerModule mod in Modules)
        {
            mod.Initialize(this);
        }
        Controller = (PlayerController)Modules.First(x => x is PlayerController);
        Gatherer = (PlayerGatherer)Modules.First(x => x is PlayerGatherer);
        Perception = (PlayerPerception)Modules.First(x => x is PlayerPerception);
        Model = (PlayerModel)Modules.First(x => x is PlayerModel);
    }
    private void ModulesUpdate()
    {
        TimeState time = TimeState.Create();
        foreach (PlayerModule mod in Modules)
        {
            mod.OnUpdate(time);
        }
    }
    private void ModulesFixedUpdate()
    {
        TimeState time = TimeState.Create();
        foreach (PlayerModule mod in Modules)
        {
            mod.OnFixedUpdate(time);
        }
    }
    private void ModulesLateUpdate()
    {
        TimeState time = TimeState.Create();
        foreach (PlayerModule mod in Modules)
        {
            mod.OnLateUpdate(time);
        }
    }
    #endregion
}
