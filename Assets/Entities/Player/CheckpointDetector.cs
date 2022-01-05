using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDetector : MonoBehaviour
{
    //[SerializeField]
    //private GameObject[] zonesToDisable = new GameObject[0];

    [SerializeField]
    private string optionalMusicPlayed;
    [SerializeField]
    private List<Collider> dialogueTriggerCols;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Tags.PLAYER))
        {
            Challenge.respawnPoint = transform.position;
            Challenge.checkpointDialogueTriggers = dialogueTriggerCols;
            Challenge.progress++;
            var meter = other.GetComponent<EntityComponents>().entityStats.meter;
            meter.FillMeter(meter.maxMeter);

            if (!optionalMusicPlayed.Equals(string.Empty))
                FindObjectOfType<AudioManager>().PlayMusic(optionalMusicPlayed);

            gameObject.SetActive(false);
        }
    }
}
