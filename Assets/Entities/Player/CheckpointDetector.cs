using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDetector : MonoBehaviour
{
    //[SerializeField]
    //private GameObject[] zonesToDisable = new GameObject[0];

    [SerializeField]
    private string optionalMusicPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Tags.PLAYER))
        {
            Challenge.startPoint = transform.position;
            var meter = other.GetComponent<EntityComponents>().entityStats.meter;
            meter.FillMeter(meter.maxMeter);

            if (!optionalMusicPlayed.Equals(string.Empty))
                FindObjectOfType<AudioManager>().PlayMusic(optionalMusicPlayed);

            gameObject.SetActive(false);
        }
    }
}
