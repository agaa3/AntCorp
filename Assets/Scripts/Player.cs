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



    public T GetModule<T>() where T : PlayerModule
    {
        return (T)Modules.First(x => x is T);
    }
    public bool TryGetModule<T>(out T module) where T : PlayerModule
    {
        T m = (T)Modules.FirstOrDefault(x => x is T);
        if (m == null || m == default(T)){
            module = null;
            return false;
        }
        module = m;
        return true;
    }

    #region  Unity Callbacks
    private void Awake()
    {
        InitSingleton();
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
    #endregion

    private void InitSingleton(){
        if (Main == null)
        {
            Main = this;
        }
        else if (Main != this)
        {
            Destroy(this.gameObject);
        }
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
        Controller = GetModule<PlayerController>();
        Gatherer = GetModule<PlayerGatherer>();
        Perception = GetModule<PlayerPerception>();
        Model = GetModule<PlayerModel>();
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
