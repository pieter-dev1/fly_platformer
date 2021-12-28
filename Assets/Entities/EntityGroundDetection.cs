using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGroundDetection : MonoBehaviour
{
    [SerializeField]
    private EntityComponents comps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(Layers.GROUND) || (other.tag == Tags.WALL && transform.position.y > other.transform.position.y))
        {
            if (comps.entityJump.jumped)
                comps.audioManager.PlaySound("BuzzSigh1");
            if (comps.entityMovement.moving)
                comps.audioManager.PlaySound("FlyWalkStrijk");
            comps.entityStats.grounded = true;
            comps.animator.SetBool("grounded", true);
            comps.entityJump.jumped = false;
            comps.rigidbody.velocity = Vector3.zero;
            comps.entityStats.blocks.Remove(Blocks.MOVE);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(Layers.GROUND) || other.tag == Tags.WALL)
        {
            comps.entityStats.grounded = false;
            comps.animator.SetBool("grounded", false);
            comps.audioManager.StopSound("FlyWalkStrijk");
        }
    }
}
