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
        if(other.tag.Equals(Tags.MAIN_GROUND))
            return;
        if (comps.fauxAttractor.currentSurface == other)
        {
            MakeObjTransparent(other, false);
            return;
        }
        else if (other.tag == Tags.PLAYER || (other.tag == Tags.GROUND && comps.fauxAttractor.currentSurface == other))
            return;

        MakeObjTransparent(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.PLAYER || (other.tag == Tags.GROUND && comps.fauxAttractor.currentSurface == other))
            return;

        MakeObjTransparent(other, false);
    }

    private void MakeObjTransparent(Collider obj, bool transparent = true)
    {
        var meshes = GetObjMeshes(obj.gameObject);
        if (!meshes.Any())
            return;
        //print($"{mesh.material.GetTag("RenderTag", true)}, {mesh.sharedMaterial.GetTag("RenderType", true)}");
        var color = Color.white;
        if (transparent)
        {
            foreach (var mesh in meshes)
            {
                //mesh.material.SetOverrideTag("RenderType", "Transparent");
                //mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                //mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                //mesh.material.SetInt("_ZWrite", 0);
                //mesh.material.DisableKeyword("_ALPHATEST_ON");
                //mesh.material.EnableKeyword("_ALPHABLEND_ON");
                //mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                //mesh.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                color = mesh.material.color;
                color.a = 0.2f;
                //mesh.material.color = color;
                transparentMeshes.Add((mesh.gameObject, mesh.material));
                mesh.material = transparentMaterial;
            }

            transparentObjects.Add(obj.gameObject);
        }
        else
        {
            foreach (var mesh in meshes)
            {
                //mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                //mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                //mesh.material.SetInt("_ZWrite", 1);
                //mesh.material.DisableKeyword("_ALPHATEST_ON");
                //mesh.material.DisableKeyword("_ALPHABLEND_ON");
                //mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                //mesh.material.renderQueue = -1;

                color = mesh.material.color;
                color.a = 1;
                //mesh.material.color = color;
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

        return meshes;
    }
}
