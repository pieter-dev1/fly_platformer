using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(Tags.COLLECTABLE))
        {
            other.gameObject.SetActive(false);
        }
    }
}
