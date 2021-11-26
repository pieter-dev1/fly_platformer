using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectItem : MonoBehaviour
{
    private int itemsCollected = 0;
    [SerializeField]
    private TextMeshProUGUI collectedTxt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(Tags.COLLECTABLE))
        {
            other.gameObject.SetActive(false);
            itemsCollected++;
            collectedTxt.text = itemsCollected.ToString();
        }
    }
}
