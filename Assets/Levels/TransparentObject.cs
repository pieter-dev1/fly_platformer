using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;
    [SerializeField]
    private bool transparent = true;
    [SerializeField]
    private bool executed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!executed && other.transform.tag.Equals(Tags.PLAYER))
        {
            executed = true;
            foreach (var obj in objects)
            {
                var mesh = obj.GetComponent<Renderer>();
                //print($"{mesh.material.GetTag("RenderTag", true)}, {mesh.sharedMaterial.GetTag("RenderType", true)}");
                if (transparent)
                {
                    mesh.material.SetOverrideTag("RenderType", "Transparent");
                    mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mesh.material.SetInt("_ZWrite", 0);
                    mesh.material.DisableKeyword("_ALPHATEST_ON");
                    mesh.material.EnableKeyword("_ALPHABLEND_ON");
                    mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mesh.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
                else
                {
                    mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    mesh.material.SetInt("_ZWrite", 1);
                    mesh.material.DisableKeyword("_ALPHATEST_ON");
                    mesh.material.DisableKeyword("_ALPHABLEND_ON");
                    mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mesh.material.renderQueue = -1;
                }

                var color = mesh.material.color;
                color.a = transparent ? 0.3f : 1;
                mesh.material.color = color;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (executed && other.transform.tag.Equals(Tags.PLAYER))
            executed = false;
    }
}
