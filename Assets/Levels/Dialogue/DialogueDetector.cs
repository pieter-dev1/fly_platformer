using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class DialogueDetector : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Image dialogueSphere;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshProUGUI nextLineTxt;
    [SerializeField] private Collider collider;
    [SerializeField] private List<Line> lines;

    [SerializeField] private float startUpTime;
    [SerializeField] private float writeTime;
    [SerializeField] private int fontSize = 24;
    public Queue<Line> linesQ { get; private set; }
    private bool localPlayerFrozen = false;
    private bool globalPlayerFrozen = false;
    private Collider playerCol;

    public static DialogueDetector currDialogue { get; private set; }
    private Line currLine;
    public bool currLineWritten { get; private set; }

    [System.Serializable]
    public struct Line
    {
        public string txt;
        public float delayToNext;
        public string[] possibleSounds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Settings.DialogueEnabled && other.tag.Equals(Tags.PLAYER))
        {
            playerCol = other;
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        currDialogue = this;

        if (currDialogue != this && currDialogue != null)
        {
            currDialogue.StopAllCoroutines();
            currDialogue.enabled = false;
        }

        if (Settings.GetBoolSetting(Settings.FreezeOnDialogue))
        {
            playerCol.attachedRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            playerCol.GetComponent<PlayerInput>().OnDisable();
            globalPlayerFrozen = true;
        }


        collider.enabled = false;
        linesQ = new Queue<Line>(lines);
        print(linesQ.Count);
        textUI.enabled = true; //for some reason text didnt appear on screen without this line
        textUI.text = string.Empty;
        textUI.fontSize = fontSize;
        nextLineTxt.enabled = true;
        dialogueSphere.enabled = true;
        StartCoroutine(ShowNextLine(startUpTime));
    }

    //private IEnumerator DisableDialogue(float delay = 0)
    //{
    //    yield return new WaitForSeconds(delay);
    //    dialogueSphere.enabled = false;
    //    textUI.text = string.Empty;
    //    textUI.enabled = false;
    //}

    public IEnumerator ShowNextLine(float showLineDelay = 0)
    {
        yield return new WaitForSeconds(showLineDelay);

        if (globalPlayerFrozen && localPlayerFrozen)
        {
            playerCol.attachedRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
            playerCol.GetComponent<PlayerInput>().OnEnable();
            globalPlayerFrozen = false;
        }
        localPlayerFrozen = true;

        currLineWritten = false;
        currLine = linesQ.Dequeue();

        //Play sound
        var sounds = currLine.possibleSounds;
        var random = Random.Range(0, 3); //decides whether any sound should play at all
        if (sounds.Any() && random > 0)
        {
            var randomIndex = Random.Range(0, sounds.Length - 1);
            if (randomIndex < sounds.Length)
            {
                audioManager.PlaySound(sounds[randomIndex]);
            }
        }

        textUI.text = string.Empty;
        var letters = currLine.txt.ToCharArray();
        foreach (var letter in letters)
        {
            if (currLineWritten)
                break;

            textUI.text += letter;
            yield return new WaitForSeconds(writeTime);
        }
        currLineWritten = true;

        //To next element or cancel dialogue
        //if (!linesQ.Any())
        //{
        //    if (globalPlayerFrozen && localPlayerFrozen)
        //    {
        //        playerCol.attachedRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
        //        playerCol.GetComponent<PlayerInput>().OnEnable();
        //        globalPlayerFrozen = false;
        //    }
        //    currentDialogueObj = null;
        //    StartCoroutine(DisableDialogue(currLine.delayToNext));
        //}
        //else
        //{
        //    StartCoroutine(ShowNextLine(currLine.delayToNext));
        //}
    }

    public void FillLine()
    {
        currLineWritten = true;
        textUI.text = currLine.txt;
    }

    public void DisableDialogue()
    {
        if (globalPlayerFrozen && localPlayerFrozen)
        {
            playerCol.attachedRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
            playerCol.GetComponent<PlayerInput>().OnEnable();
            globalPlayerFrozen = false;
        }
        currDialogue = null;
        dialogueSphere.enabled = false;
        textUI.text = string.Empty;
        textUI.enabled = false;
        nextLineTxt.enabled = false;
    }
}
