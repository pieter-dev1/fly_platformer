using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckpointDetector : MonoBehaviour
{
    //[SerializeField]
    //private GameObject[] zonesToDisable = new GameObject[0];

    [SerializeField]
    private string optionalMusicPlayed;
    public List<Collider> dialogueTriggerCols;
    public GameObject tpButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Tags.PLAYER))
        {
            Challenge.respawnPoint = transform.position;
            Challenge.checkpointDialogueTriggers = dialogueTriggerCols;
            Challenge.progress++;
            other.GetComponent<PlayerPause>().AddTpButton(tpButton);

            var meter = other.GetComponent<EntityComponents>().entityStats.meter;
            meter.FillMeter(meter.maxMeter);

            if (!optionalMusicPlayed.Equals(string.Empty))
                FindObjectOfType<AudioManager>().PlayMusic(optionalMusicPlayed);

            gameObject.SetActive(false);
        }
    }
}
