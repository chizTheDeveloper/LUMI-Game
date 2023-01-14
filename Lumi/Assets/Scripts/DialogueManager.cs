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
    QuestTracker questTracker;
    string who;
    string currentProgressStep;

    void Start()
    {
        // Queue for dialogue
        text = new Queue<string>();

        // Get the Quest Tracker to access variables from it
        questTracker = player.GetComponent<QuestTracker>();
    }

    // Function for triggering the start of dialogue
    public void StartDialogue(Dialogue dialogue, string speakingWith){
        // Variable to save who the player is speaking with, which is provided by the QuestTracker script
        who = speakingWith;

        if(who == "npc"){
            nameText.text = "Octopus";
            currentProgressStep = "firstEncounter";
            // Dialogue for octopus npc
            dialogue.text.Add("Here is some test example text that loads initially when the dialogue is triggered.");
            dialogue.text.Add("Some more text to show that pressing the Enter/Return key on your keyboard goes to the next piece of dialogue.");
            dialogue.text.Add("Yay, it works!");
        }
        else if(who == "krill"){
            nameText.text = "Krill";
            // If intro has been passed, trigger the start of the quest system
            if(!questTracker.progress.Contains("intro")){
                currentProgressStep = "intro";
                dialogue.text.Add("Hello mate. Yes, yes, it's me. The tiny krill beside you. Don't panic, I'm here to help!");
                dialogue.text.Add("'What happened?' you ask? Oh boy, that's a long story that, honestly, I don't know too many details of. I'll try to give you a quick rundown.");
                dialogue.text.Add("Let's just say humans don't have the best reputation, and it seems they angered someone powerful. Very powerful. Powerful enough that they banished your entire species to live underwater as animal spirits.");
                dialogue.text.Add("So that's how you got here. I've been here a little longer. One day I was bartending at the pub and then next... well, I woke up as a krill.");
                dialogue.text.Add("Of course, I was freaked out at first too! But it's not so bad down here. The blue ocean, the corals, the GIANT fish. Well, I guess they're less giant to you. I'm just a krill, after all.");
                dialogue.text.Add("Anyways, how about we do a little exporing! Swim around, see what the ocean has to offer. I will be right by your side, mate.");
            }
            // If player has finished their first encounter with the octopus npc
            else if(!questTracker.progress.Contains("test")){

            }
        }

        // Clear the queue
        text.Clear();

        // Queue up the dialogue sentences
        foreach(string sentence in dialogue.text){
            text.Enqueue(sentence);
        }

        // Clear and display the next set of dialogue
        dialogue.text.Clear();
        DisplayNext();
    }

    public void DisplayNext(){
        // If there are no dialogue sentences left in the queue, trigger the end of the dialogue
        if(text.Count == 0){
            EndDialogue();
            return;
        }

        // Dequeue the dialogue sentence
        string sentence = text.Dequeue();

        // Stop coroutines in case player presses enter before the sentence has finished being typed out
        StopAllCoroutines();

        // Start coroutine to type each letter of the dialogue sentence one at a time
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence){
        // Clear current text
        dialogueText.text = "";

        // Cycle through each letter of the dialogue and add it to the displayed text
        foreach (char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue(){
        // Hide the dialogue UI
        nameText.enabled = false;
        dialogueText.enabled = false;
        contText.enabled = false;

        // Update the players progress in Quest Tracker
        if(currentProgressStep == "intro"){
            questTracker.progress.Add("intro");
        }
        else if(currentProgressStep == "firstEncounter"){
            questTracker.progress.Add("firstNpcEncounter");
        }
    }
}
