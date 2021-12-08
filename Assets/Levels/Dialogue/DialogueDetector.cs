using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class DialogueDetector : MonoBehaviour
{
    public static DialogueDetector currentDialogueObj;
    [SerializeField] private Image dialogueSphere;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Collider collider;
    [SerializeField] private List<Line> lines;

    [SerializeField] private float startUpTime;
    [SerializeField] private float writeTime;
    private Queue<Line> linesQ;

    [System.Serializable]
    public struct Line
    {
        public string txt;
        public float delayToNext;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Tags.PLAYER))
        {
            if (currentDialogueObj != this && currentDialogueObj != null)
            {
                currentDialogueObj.StopAllCoroutines();
                currentDialogueObj.enabled = false;
            }

            collider.enabled = false;
            currentDialogueObj = this;
            linesQ = new Queue<Line>(lines);
            textUI.enabled = true; //for some reason text didnt appear on screen without this line
            textUI.text = string.Empty;
            DisableDialogue(delay: startUpTime);
            StartCoroutine(ShowNextLine(startUpTime));
        }
    }

    private IEnumerator DisableDialogue(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        //dialogueSphere.enabled = enable;
        textUI.text = string.Empty;
        textUI.enabled = false;
    }

    private IEnumerator ShowNextLine(float showLineDelay = 0)
    {
        yield return new WaitForSeconds(showLineDelay);
        var currLine = linesQ.Dequeue();
        textUI.text = string.Empty;
        var letters = currLine.txt.ToCharArray();
        foreach (var letter in letters)
        {
            textUI.text += letter;
            yield return new WaitForSeconds(writeTime);
        }

        //To next element or cancel dialogue
        if (!linesQ.Any())
        {
            currentDialogueObj = null;
            StartCoroutine(DisableDialogue(currLine.delayToNext));
        }
        else
        {
            StartCoroutine(ShowNextLine(currLine.delayToNext));
        }
    }
}
