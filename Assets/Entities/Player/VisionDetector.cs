using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionDetector : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.tag == Tags.PLAYER || (other.tag == Tags.GROUND && other.transform.position.y < player.position.y))
            return;

        var mesh = other.GetComponent<MeshRenderer>();
        if (mesh != null)
            mesh.material.color = Color.red;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.PLAYER || (other.tag == Tags.GROUND && other.transform.position.y < player.position.y))
            return;

        var mesh = other.GetComponent<MeshRenderer>();
        if (mesh != null)
            mesh.material.color = Color.white;
    }
}
