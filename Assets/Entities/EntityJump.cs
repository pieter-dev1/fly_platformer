using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityJump : MonoBehaviour
{
    private EntityComponents comps;
    [HideInInspector]
    public bool jumped = false;
    [SerializeField]
    private float jumpForce = 1;
    private const float wallJumpRatio = 0.6f;

    private void Start()
    {
        comps = GetComponent<EntityComponents>();
    }

    public void Jump()
    {
        jumped = true;
        var groundUp = comps.entityStats.groundUp;
        if (comps.fauxAttractor != null && comps.fauxAttractor.enabled && comps.fauxAttractor.onWall)
        {
            var positiveNormal = comps.entityStats.upAxis.positive; //if not checked and Move properly set, player could be stuck in wall
            comps.fauxAttractor.CancelCustomGravity(false);
            comps.entityMovement.Move(new Vector2(positiveNormal ? 0.1f : -0.1f, 0));
            comps.entityStats.blocks.Add(Blocks.MOVE);
            comps.rigidbody.AddForce(groundUp * (jumpForce * wallJumpRatio));
        }
        else
            comps.rigidbody.AddForce(groundUp * jumpForce);
    }

    public void CancelJump()
    {
        var newVel = comps.rigidbody.velocity;
        newVel[comps.entityStats.upAxis.index] = 0;
        comps.rigidbody.velocity = newVel;
        comps.entityStats.blocks.Remove(Blocks.MOVE);
    }
}
