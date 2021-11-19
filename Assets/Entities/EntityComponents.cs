using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponents : MonoBehaviour
{
    //Entity
    public EntityStats entityStats;
    public EntityMovement entityMovement;
    public EntityJump entityJump;
    public FauxAttractor fauxAttractor;

    //Player
    public PlayerInput playerInput;

    //Components
    public new Rigidbody rigidbody;
    public new Collider collider;
    public Mesh mesh;
    public Animator animator;


    //private void Awake()
    //{
    //    entityStats = GetComponent<EntityStats>();
    //    entityMovement = GetComponent<EntityMovement>();
    //    entityJump = GetComponent<EntityJump>();
    //    fauxAttractor = GetComponent<FauxAttractor>();

    //    playerInput = GetComponent<PlayerInput>();

    //    rigidbody = GetComponent<Rigidbody>();
    //    collider = GetComponent<Collider>();
    //    mesh = GetComponent<Mesh>();
    //    animator = GetComponent<Animator>();
    //}
}
