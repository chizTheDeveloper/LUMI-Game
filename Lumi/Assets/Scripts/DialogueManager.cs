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
    public TextMeshProUGUI contText;
    public GameObject player;
    string who;

    // Start is called before the first frame update
    void Start()
    {
        text = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, string speakingWith){
        who = speakingWith;

        if(who == "npc"){
            nameText.text = dialogue.name;
            dialogue.text.Add("Here is some test example text that loads initially when the dialogue is triggered.");
            dialogue.text.Add("Some more text to show that pressing the Enter/Return key on your keyboard goes to the next piece of dialogue.");
            dialogue.text.Add("Yay, it works!");
        }
        else if(who == "intro"){
            nameText.text = "Krill";
            dialogue.text.Add("Hello mate. Yes, yes, it's me. The tiny krill beside you. Don't panic, I'm here to help!");
            dialogue.text.Add("'What happened?' you ask? Oh boy, that's a long story that, honestly, I don't know too many details of. I'll try to give you a quick rundown.");
            dialogue.text.Add("Let's just say humans don't have the best reputation, and it seems they angered someone powerful. Very powerful. Powerful enough that they banished your entire species to live underwater as animal spirits.");
            dialogue.text.Add("So that's how you got here. I've been here a little longer. One day I was bartending at the pub and then next... well, I woke up as a krill.");
            dialogue.text.Add("Of course, I was freaked out at first too! But it's not so bad down here. The blue ocean, the corals, the GIANT fish. Well, I guess they're less giant to you. I'm just a krill, after all.");
            dialogue.text.Add("Anyways, how about we do a little exporing! Swim around, see what the ocean has to offer. I will be right by your side, mate.");
        }

        text.Clear();

        foreach(string sentence in dialogue.text){
            text.Enqueue(sentence);
        }

        dialogue.text.Clear();
        DisplayNext();
    }

    public void DisplayNext(){
        if(text.Count == 0){
            EndDialogue();
            return;
        }

         Debug.Log(text.Count);

        string sentence = text.Dequeue();
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
        nameText.enabled = false;
        dialogueText.enabled = false;
        contText.enabled = false;

        QuestTracker questTracker = player.GetComponent<QuestTracker>();

        if(who == "intro"){
            questTracker.progress.Add("intro");
        }
        else if(who == "npc"){
            questTracker.progress.Add("firstNpcEncounter");
        }
    }
}
