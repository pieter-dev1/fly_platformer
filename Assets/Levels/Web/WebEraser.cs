using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Image eraserMeter;

    [SerializeField]
    private float revertTime = 1;

    private bool activated = false;
    private float maxWidth = 70;
    private FauxAttractor playerAttractor;

    //private void Update()
    //{
    //    print(activated);
    //    if (activated)
    //    {
    //        var scale = eraserMeter.transform.localScale;
    //        scale.x += maxWidth / revertTime * Time.deltaTime;
    //        eraserMeter.transform.localScale = scale;
    //    }
    //}

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

            eraserMeter.enabled = true;
            eraserMeter.color = mesh.sharedMaterial.GetColor("_EmissionColor");

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

        //activated = !enable;
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
