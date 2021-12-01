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
            comps.entityStats.grounded = true;
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
        }
    }
}
