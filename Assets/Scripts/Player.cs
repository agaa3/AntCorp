using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    public static Player Main;

    [Header("Modules")]
    public PlayerCamera Camera;
    public PlayerController Controller;
    public PlayerPerception Perception;
    public PlayerGatherer Gatherer;
    public PlayerModel Model;
    public PlayerMotor Motor;
    public PlayerModule[] Modules;
    [Header("Components")]
    public Rigidbody2D UseRigidbody;
    public BoxCollider2D Collider;
    public LivingMixin Mixin;



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
        Mixin = GetComponent<LivingMixin>();
    }
    #region Modules
    private void InitModules()
    {
        // find a better way to add camera to modules list
        Modules = GameObject.FindObjectsOfType<PlayerModule>();
        foreach (PlayerModule mod in Modules)
        {
            mod.Initialize(this);
        }
        TryGetModule<PlayerController>(out Controller);
        TryGetModule<PlayerGatherer>(out Gatherer);
        TryGetModule<PlayerPerception>(out Perception);
        TryGetModule<PlayerModel>(out Model);
        TryGetModule<PlayerCamera>(out Camera);
        TryGetModule<PlayerMotor>(out Motor);
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
