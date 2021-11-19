using System.Collections;
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
    public (Transform transform, bool cubeShaped) currentSurface;
    private RaycastHit newHit;

    private void Start()
    {
        comps = GetComponent<EntityComponents>();
        comps.rigidbody.useGravity = false;
        enabled = false;
        currentSurface = (mainGround, true);
    }

    private void Update()
    {
        var pos = transform.position;
        var upAxisIndex = comps.entityStats.upAxis.index;
        var verticalRotationAxis = upAxisIndex == 0 ? 2 : upAxisIndex - 1;
        var raycastStart = pos;
        raycastStart[upAxisIndex] += 0.5f;
        Debug.DrawRay(raycastStart, Quaternion.Euler(transform.right * 30) * transform.forward, Color.red, 1f);
        if (Physics.Raycast(raycastStart, Quaternion.Euler(transform.right * 30) * transform.forward, out newHit, 1f) && (newHit.transform.gameObject != currentSurface.transform.gameObject || !comps.entityMovement.allowRot))
        {
            if (newHit.transform.tag.Equals(Tags.WALL))
            {
                if (comps.entityJump.jumped)
                {
                    //Temp fix for bug where new gravity wouldnt be applied when jumping to another wall. 
                    comps.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    comps.rigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
                }
                print($"{newHit.transform.name}: {newHit.normal}");
                onWall = true;
                currentSurface = (newHit.transform, newHit.transform.GetComponent<BoxCollider>() != null);
                comps.entityStats.groundUp = newHit.normal;
            }
        }

        //WALKING AROUND CORNER NEEDS ADDITIONAL TESTING
        //else if (Physics.Raycast(transform.position, -transform.up, out newHit) && comps.entityStats.groundUp != newHit.normal)
        //{
        //    if (newHit.transform.tag.Equals(Tags.WALL))
        //    {
        //        onWall = true;
        //        currentSurface = newHit.transform;
        //        comps.entityStats.groundUp = newHit.normal;
        //        print(comps.entityStats.groundUp);
        //        var rot = transform.rotation.eulerAngles;
        //        rot[comps.entityStats.upAxis.index] = 45;
        //        transform.rotation = Quaternion.LookRotation(rot, comps.entityStats.groundUp);
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer.Equals(Layers.GROUND))
        {
            var normal = collision.contacts[0].normal;
            var angle = Vector3.Angle(normal, transform.forward);
            if(normal.y > 0 && Mathf.Approximately(angle, 90))
            {
                onWall = false;
                currentSurface = (collision.transform, true);
                comps.entityStats.groundUp = EntityStats.defaultGroundUp;
            }
        }

        if(enabled && (collision.collider.gameObject.layer.Equals(Layers.GROUND) || collision.collider.tag.Equals(Tags.WALL)))
        {
            var rot = Vector3.zero;
            if (comps.entityStats.upAxis.index != MoveAxis.VERTICAL)
            {
                rot[comps.entityStats.horAxis] = -90;
            }
            if (rot != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(rot, comps.entityStats.groundUp); //rotation
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.Equals(currentSurface.transform.gameObject) && currentSurface.cubeShaped)
        {
            //WHEN WALKING AROUND CORNERS ENABLED
            //comps.entityMovement.allowRot = true;
            if((collision.collider.tag.Equals(Tags.WALL) && (comps.entityJump == null || !comps.entityJump.jumped)))
            {
                CompletelyCancelWallRun();
            }
        }
    }

    public void CompletelyCancelWallRun()
    {
        comps.rigidbody.velocity = Vector3.zero;
        comps.playerInput.CancelSprint();
    }

    public void CancelCustomGravity(bool disableAttractor = true)
    {
        currentSurface = (mainGround, true);
        if (comps.entityStats.groundUp != EntityStats.defaultGroundUp)
            comps.entityStats.groundUp = EntityStats.defaultGroundUp;
        enabled = !disableAttractor;
        onWall = false;
    }
}
