using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VisionDetector : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Transform cam;
    private EntityComponents comps;
    [SerializeField]
    private Material transparentMaterial;
    private List<GameObject> transparentObjects = new List<GameObject>();
    private List<(GameObject mesh, Material material)> transparentMeshes = new List<(GameObject mesh, Material material)>();

    private void Start()
    {
        comps = player.GetComponent<EntityComponents>();
        cam = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(Tags.MAIN_GROUND) || other.tag.Equals(Tags.COLLECTABLE))
            return;
        if (comps.fauxAttractor.currentSurface.transform == other)
        {
            MakeObjTransparent(other, false);
            return;
        }
        else if (other.tag == Tags.PLAYER || (other.tag == Tags.GROUND && comps.fauxAttractor.currentSurface.transform == other))
            return;

        MakeObjTransparent(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.PLAYER || other.tag == Tags.COLLECTABLE || (other.tag == Tags.GROUND && comps.fauxAttractor.currentSurface.transform == other))
            return;

        MakeObjTransparent(other, false);
    }

    private void MakeObjTransparent(Collider obj, bool transparent = true)
    {
        var meshes = GetObjMeshes(obj.gameObject);
        if (!meshes.Any())
            return;
        var color = Color.white;
        if (transparent)
        {
            foreach (var mesh in meshes)
            {
                transparentMeshes.Add((mesh.gameObject, mesh.material));
                mesh.material = transparentMaterial;
            }

            transparentObjects.Add(obj.gameObject);
        }
        else
        {
            foreach (var mesh in meshes)
            {
                var meshObj = transparentMeshes.First(x => x.mesh == mesh.gameObject);
                mesh.material = meshObj.material;
            }

            transparentObjects.Remove(obj.gameObject);
        }


    }

    private List<MeshRenderer> GetObjMeshes(GameObject obj)
    {
        //On object itself
        var meshes = new List<MeshRenderer>();
        var onObj = obj.GetComponent<MeshRenderer>();
        if (onObj != null)
        {
            meshes.Add(onObj);
            return meshes;
        }

        //In hierarchy
        foreach(Transform child in obj.transform)
        {
            var childMesh = child.GetComponent<MeshRenderer>();
            if (childMesh != null)
                meshes.Add(childMesh);
        }

        //In nearest parent with mesh
        if (!meshes.Any())
        {
            var parentMesh = FindParentMesh(obj.transform);
            if (parentMesh != null)
                meshes.Add(parentMesh);
        }

        return meshes;
    }

    private MeshRenderer FindParentMesh(Transform obj)
    {
        var parent = obj.parent;
        if (parent != null)
        {
            var parentMesh = parent.GetComponent<MeshRenderer>();
            if (parentMesh != null)
                return parentMesh;

            return FindParentMesh(parent);
        }
        return null;
    }
}
