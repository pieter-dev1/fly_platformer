﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FauxAttractor : MonoBehaviour
{
    private EntityComponents comps;
    [HideInInspector]
    public bool onWall = false;
    [SerializeField]
    public Transform cornerDetector;
    [SerializeField]
    private Transform mainGround;
    [SerializeField]
    public Transform currentSurface;
    private RaycastHit newHit;

    private void Start()
    {
        comps = GetComponent<EntityComponents>();
        comps.rigidbody.useGravity = false;
        enabled = false;
        currentSurface = mainGround;
    }

    private void Update()
    {
        var pos = transform.position;
        var upAxisIndex = comps.entityStats.upAxis.index;
        var verticalRotationAxis = upAxisIndex == 0 ? 2 : upAxisIndex - 1;
        var raycastStart = pos;
        raycastStart[upAxisIndex] -= 0.5f;
        //Debug.DrawRay(raycastStart, Quaternion.Euler(transform.right * 30) * (transform.forward * 3), Color.red);
        if (Physics.Raycast(raycastStart, Quaternion.Euler(transform.right * 30) * transform.forward, out newHit, 1f) && newHit.transform.gameObject != currentSurface.gameObject)
        {
            if (newHit.transform.tag.Equals(Tags.WALL))
            {
                onWall = true;
                currentSurface = newHit.transform;
                comps.entityStats.groundUp = newHit.normal;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals(Tags.GROUND))
        {
            var normal = collision.contacts[0].normal;
            var angle = Vector3.Angle(normal, transform.forward);
            if(normal.y > 0 && Mathf.Approximately(angle, 90))
            {
                onWall = false;
                currentSurface = collision.transform;
                comps.entityStats.groundUp = EntityStats.defaultGroundUp;
            }

        }
        if(enabled && (collision.collider.tag.Equals(Tags.GROUND) || collision.collider.tag.Equals(Tags.WALL)))
        {
            var rot = Vector3.zero;
            if (comps.entityStats.upAxis.index != MoveAxis.VERTICAL)
                rot[comps.entityStats.horAxis] = -90;
            if (rot != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(rot, comps.entityStats.groundUp); //rotation
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.Equals(currentSurface.gameObject) && (collision.collider.tag.Equals(Tags.WALL) && (comps.entityJump == null || !comps.entityJump.jumped)))
        {
            comps.rigidbody.velocity = Vector3.zero;
            comps.playerInput.CancelSprint();
        }
    }

    public void CancelCustomGravity(bool disableAttractor = true)
    {
        currentSurface = mainGround;
        if (comps.entityStats.groundUp != new Vector3(0, 1, 0))
            comps.entityStats.groundUp = new Vector3(0, 1, 0);
        enabled = !disableAttractor;
        onWall = false;
    }
}
