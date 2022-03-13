using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ant_friends : MonoBehaviour
{
    //  [Header("Movement")]
    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;

    //public Animator animator;
    //private float MovementSpeed = 2;
    //public float JumpForce = 10;
    //public float checkRadius;
    //private bool m_FacingRight = true;
    //bool wallClimbing;
    //public float wallSlidingSpeed;
    //bool isTouchingFront;
    //[SerializeField] private LayerMask layerMask,groundLayerMask,groundLayerMask1;

    private void Start()
    {
    }


    private void Update()
    {
        
    }
 

	private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Friends"))
        {
            Destroy(other.gameObject);
            FriendsManager.instance.ChangeFriendsScore();
        }
    }


   
}