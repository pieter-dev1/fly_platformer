using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebEraser : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private Collider coll;
    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private SpriteRenderer[] webs;
    [SerializeField]
    private GameObject[] surfaces;

    [SerializeField]
    private float revertTime = 1;

    private FauxAttractor playerAttractor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(Tags.PLAYER))
        {
            if (playerAttractor == null) playerAttractor = other.gameObject.GetComponent<FauxAttractor>();
            ToggleWebs();

            foreach (var surface in surfaces)
            {
                surface.layer = Layers.GROUND;
            }

            StartCoroutine(PutWebsBack());
        }
    }

    private void ToggleWebs(bool enable = false)
    {
        mesh.enabled = enable;
        coll.enabled = enable;
        particle.SetActive(enable);

        foreach (var web in webs)
        {
            web.enabled = enable;
        }
    }

    private IEnumerator PutWebsBack()
    {
        yield return new WaitForSeconds(revertTime);
        ToggleWebs(true);
        foreach (var surface in surfaces)
        {
            surface.layer = Layers.DEFAULT;
        }
        playerAttractor.CompletelyCancelWallRun();
    }
}
