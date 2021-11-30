using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class DialogueDetector : MonoBehaviour
{
    public static string currentDialogueObj;
    [SerializeField] private Image dialogueSphere;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Collider collider;
    [SerializeField] private List<Line> lines;

    [SerializeField] private float startUpTime;
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
            collider.enabled = false;
            currentDialogueObj = transform.name;
            linesQ = new Queue<Line>(lines);
            EnableDialogueObjects(delay: startUpTime);
            StartCoroutine(ShowNextLine(startUpTime));
        }
    }

    private IEnumerator EnableDialogueObjects(bool enable = true, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        //dialogueSphere.enabled = enable;
        if (!enable) textUI.text = string.Empty;
        textUI.enabled = enable;
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
            yield return null;
        }

        //To next element or cancel dialogue
        if (!linesQ.Any())
        {
            StartCoroutine(EnableDialogueObjects(false, currLine.delayToNext));
        }
        else
        {
            StartCoroutine(ShowNextLine(currLine.delayToNext));
        }
    }
}
