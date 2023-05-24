using UnityEngine;

public class PlayerGrabber : PlayerModule
{
    public Rock Holded;
    public bool IsHolding;
    public float LookDistance = 1f;
    public float LookHeight = 0.1f;

    public bool FindHoldable(out Rock result)
    {
        Vector3 p = Parent.transform.position;
        Vector2 hull = Parent.Body.Hull;
        Vector3 front = Parent.Body.Front * (Parent.Controller.IsFacingRight ? 1f : -1f);

        var h = Physics2D.Raycast(p + front * hull.x + front * LookHeight, front, LookDistance);
        if (h != default)
        {
            Rock r;
            Debug.Log(h.transform.gameObject.name);
            if (h.transform.TryGetComponent(out r))
            {
                result = r;
                return true;
            }
        }
        result = null;
        return false;
    }
    public void BeginHold()
    {
        Debug.Log("Begin Hold");
        Holded.transform.SetParent(Parent.transform);
        Holded.transform.localPosition = new Vector3(Parent.Body.Hull.x + 0.2f, (Holded.transform.rotation * Holded.Hull).y*-0.5f, 0);
        Parent.Controller.CanFace = false;
        Holded.GetComponent<Rigidbody2D>().gravityScale = 0;
        IsHolding = true;
    }
    public void EndHold() 
    {
        Debug.Log("End Hold");
        Holded.GetComponent<Rigidbody2D>().gravityScale = 1;
        Holded.transform.parent = null;
        Parent.Controller.CanFace = true;
        Holded = null;
        IsHolding = false;
    }
    private void Start()
    {
        Parent.Controller.OnBeginTurn += (bool t) => { 
            if (IsHolding)
            {
                EndHold();
            }
        };
    }
    public override void OnUpdate(TimeState time)
    {
        bool h = Input.GetButton("Grab");
        if (IsHolding != h && !Parent.Controller.IsMidTurn)
        {
            if (h)
            {
                if (FindHoldable(out Holded))
                {
                    BeginHold();
                }
            }
            else
            {
                EndHold();
            }
        }
    }
}
