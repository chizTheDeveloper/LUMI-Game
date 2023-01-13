using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> text;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    // Start is called before the first frame update
    void Start()
    {
        text = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){

        nameText.text = dialogue.name;
        text.Clear();

        foreach(string sentence in dialogue.text){
            text.Enqueue(sentence);
        }

        DisplayNext();
    }

    public void DisplayNext(){
        if(text.Count == 0){
            EndDialogue();
            return;
        }

        string sentence = text.Dequeue();
        Debug.Log(sentence);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue(){

    }
}
