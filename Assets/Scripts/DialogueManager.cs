using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> text;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI contText;
    public TextMeshProUGUI contTextA;
    public GameObject player;
    public GameObject jelly;
    public GameObject crab;
    QuestTracker questTracker;
    QuestTracker2 questTracker2;
    QuestTracker3 questTracker3;
    QuestTrackerBoss questTrackerBoss;
    string who;
    string currentProgressStep;
    string whoIsTalking;

    AudioSource krillVoice;
    AudioSource octoVoice;
    AudioSource sideQuestVoice;
    public GameObject krillAudio;
    public GameObject octoAudio;
    public GameObject sideQuestAudio;

    void Start()
    {
        // Queue for dialogue
        text = new Queue<string>();

        // Get the Quest Tracker to access variables from it
        questTracker = player.GetComponent<QuestTracker>();

        questTracker2 = player.GetComponent<QuestTracker2>();

        questTracker3 = player.GetComponent<QuestTracker3>();

        questTrackerBoss = player.GetComponent<QuestTrackerBoss>();

        whoIsTalking = "";

        
        krillVoice = krillAudio.GetComponent<AudioSource>();
        octoVoice = octoAudio.GetComponent<AudioSource>();

        if(questTrackerBoss == null){
            sideQuestVoice = sideQuestAudio.GetComponent<AudioSource>();
        }
    }

    // Function for triggering the start of dialogue
    public void StartDialogue(Dialogue dialogue, string speakingWith){
        // Variable to save who the player is speaking with, which is provided by the QuestTracker script
        who = speakingWith;

        if(who == "npc"){
            nameText.text = "Octopus";
            whoIsTalking = "octo";
            octoVoice.Play();

            // If the player hasn't had their first encounter with the octopus NPC yet
            if(!questTracker.progress.Contains("firstNpcEncounter")){
                currentProgressStep = "firstEncounter";
                dialogue.text.Add("Oh thank goodness! I’ve found you at last! I have been searching so long for you!");
                dialogue.text.Add("The remanence of your soul is fading and soon the sea’s last hope will fade with it. It’s up to you to save us all.");
                dialogue.text.Add("Visit all three realms of the sea and reunite all the lost orbs. Only then will we save all the lost souls.");
                dialogue.text.Add("Feel free to come find me if you forget your goal.");
            }
            else if(questTracker.progress.LastOrDefault() == "secondNpcEncounter"){
                currentProgressStep = "secondEncounter";
                dialogue.text.Add("Nice job so far!");
                dialogue.text.Add("Remember, we need to find the orb! Eyes on the track!");
                dialogue.text.Add("If you need me just call out my name, I’ll be there.");
            }
            else if(questTracker.progress.LastOrDefault() == "complete"){
                currentProgressStep = "complete";
                dialogue.text.Add("Good job! You found the first orb.");
                dialogue.text.Add("There are two orbs left to find. Keep looking. Remember what’s at stake!");
                dialogue.text.Add("The door to the next realm has opened. Feel free to continue your journey when ready by following the blue coral to the next realm, good luck!");
            }
            else if(questTracker.progress.LastOrDefault() == "congratsJelly"){
                currentProgressStep = "allJelliesCaught";
                dialogue.text.Add("As a reward for bringing me 3 jellyfish, I will give you ____.");
            }
            else if(questTracker.progress.LastOrDefault() == "octoTip"){
                currentProgressStep = "givingTip";
                dialogue.text.Add("You must find the orb hidden in this realm in order to move onto the next one. Keep exploring, with your hard work we can unite all three orbs to lift the curse.");
            }
            /*else if(!questTracker.progress.Contains("progressComplete")){
                currentProgressStep = "exitLevel";
            }*/
        }
        else if(who == "krill"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            // If intro has been passed, trigger the start of the quest system
            if(!questTracker.progress.Contains("intro")){
                currentProgressStep = "intro";
                krillVoice.Play();
                dialogue.text.Add("Hello mate. It's me. The tiny krill beside you. Don't panic, I'm here to help!");
                dialogue.text.Add("'What happened?' you ask? Humans angered someone very powerful, who banished our entire species to live underwater as animal spirits, turning you into a manta ray.");
                dialogue.text.Add("Same thing happened to me. One day I was bartending at the pub and then next... well, I woke up as a krill.");
                //dialogue.text.Add("It's not so bad down here. The blue ocean, the corals, the GIANT fish. Well, I guess they're less giant to you. I'm just a krill, after all.");
                dialogue.text.Add("How about we do some exploring? Wait... who's that octopus fella over there?");
            }
            // If player has finished their first encounter with the octopus npc
            /*
            else if(questTracker.progress.LastOrDefault() == "howToHunt"){
                currentProgressStep = "howToHunt";
                dialogue.text.Add("Watch out, a kordillion lurks ahead! Get closer and click to fire a bubble shot to take the big lad out.");
            }
            else if(questTracker.progress.LastOrDefault() == "foundKey"){
                currentProgressStep = "krillKey";
                dialogue.text.Add("Would you look at that, a key was hiding in that kordillion. I wonder what that could be used for...");
            }
            */
            else if(questTracker.progress.LastOrDefault() == "nearSecretArea"){
                currentProgressStep = "accessDenied";
                krillVoice.Play();
                dialogue.text.Add("Hmmm what could be in here? There must be a way to get inside.");
            }
            else if(questTracker.progress.LastOrDefault() == "triedToExit"){
                currentProgressStep = "exitDenied";
                krillVoice.Play();
                dialogue.text.Add("You need to collect the orb before you can get to the next realm, keep going mate");
            }
            else if(questTracker.progress.LastOrDefault() == "pickedUpKey"){
                currentProgressStep = "keyObtaained";
                krillVoice.Play();
                dialogue.text.Add("I think I just heard something open up.");
            }
            else if(questTracker.progress.LastOrDefault() == "foundHeart"){
                currentProgressStep = "gotHeart";
                krillVoice.Play();
                dialogue.text.Add("Would you look at that, an extra life! Those kordillion fellas better watch out.");
            }
            /*
            else if(questTracker.progress.LastOrDefault() == "foundXP"){
                currentProgressStep = "firstXP";
                dialogue.text.Add("You just found your first special power. Collect 5 of them to gain 1 extra strong bubble shot when fighting enemies.");
            }
            */
        }
        else if(who == "racingFish"){
            nameText.text = "Angie the Angler Fish";
            whoIsTalking = "npc";
            sideQuestVoice.Play();

            // If intro has been passed, trigger the start of the quest system
            if(questTracker3.progress.LastOrDefault() == "preRace"){
                currentProgressStep = "introRace";
                dialogue.text.Add("You there! I challenge you to a race.");
                dialogue.text.Add("We'll race through the coral trail to the rocks. If you win, I'll give you a reward.");
                dialogue.text.Add("We start at GO!");
            }
            else if(questTracker3.progress.LastOrDefault() == "lostRace"){
                currentProgressStep = "lost";
                dialogue.text.Add("You lost this one. When you boost, I can boost faster! If you'd like to try again, meet me back at the start.");
            }
            else if(questTracker3.progress.LastOrDefault() == "wonRace"){
                currentProgressStep = "won";
                dialogue.text.Add("Good job, you won! As promised, here is your reward. Your bubble shot now has a shorter cooldown!");
            }
        }
        else if(who == "npcL2"){
            nameText.text = "Cedric the Crab";
            whoIsTalking = "npc";
            sideQuestVoice.Play();

            if(questTracker2.progress.LastOrDefault() == "blobStart"){
                crab.GetComponent<NPC>().talking = true;
                currentProgressStep = "introBlob";
                dialogue.text.Add("You there! You must help me, there seems to be giant glowing ink blobs floating above me!");
                dialogue.text.Add("You have 30 seconds to get rid of them before its too late! Look above to the giant blobs! 3...2...1....GO");
            }
        }
        else if(who == "npcL2_Fail"){
            nameText.text = "Cedric the Crab";
            whoIsTalking = "npc";
            sideQuestVoice.Play();

            if(questTracker2.progress.LastOrDefault() == "blobFail"){
                crab.GetComponent<NPC>().talking = true;
                currentProgressStep = "failBlob";
                dialogue.text.Add("Oh no the Blobs are still there! Come talk to me to try again.");
            }
        }
        else if(who == "npcL2_Pass"){
            nameText.text = "Cedric the Crab";
            whoIsTalking = "npc";
            sideQuestVoice.Play();

            if(questTracker2.progress.LastOrDefault() == "blobPass"){
                crab.GetComponent<NPC>().talking = true;
                currentProgressStep = "passBlob";
                dialogue.text.Add("You did it! The Blobs are no longer bothering me.");
                dialogue.text.Add("Because of your help, here is your reward! Your bubble shot now travels a farther distance.");
            }
        }
        else if(who == "npcL1"){
            nameText.text = "Jolene the Jellyfish";
            whoIsTalking = "npc";
            sideQuestVoice.Play();

            if(questTracker.progress.LastOrDefault() == "jellyStart"){
                jelly.GetComponent<NPCJelly>().talking = true;
                currentProgressStep = "introJelly";
                dialogue.text.Add("Hi there! My kids have gone missing!");
                dialogue.text.Add("They are around here somewhere...please collect 3 of them and bring them back to me. I'll give you a reward.");
            }
        }
        else if(who == "npcL1_end"){
            nameText.text = "Jolene the Jellyfish";
            whoIsTalking = "npc";
            sideQuestVoice.Play();
            
            if(questTracker.progress.LastOrDefault() == "jellyEnd"){
                currentProgressStep = "outroJelly";
                dialogue.text.Add("You found my kids, thank you so much! As your reward, I have granted you extra bubble strength! Your bubble now does +1 damage!");
            }
        }
        else if(who == "krillL2"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            krillVoice.Play();
            if(questTracker2.progress.LastOrDefault() == "L2intro"){
                currentProgressStep = "introL2";
                dialogue.text.Add("Looks like we're getting deeper into the ocean. Keep exploring, seems like only you can lift the curse, mate!");
            }
            else if(questTracker2.progress.LastOrDefault() == "triedToExit"){
                currentProgressStep = "exitDenied";
                krillVoice.Play();
                dialogue.text.Add("You need to collect the orb before you can get to the next realm, keep going mate");
            }
        }
        else if(who == "krillL3"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            krillVoice.Play();
            if(questTracker3.progress.LastOrDefault() == "L3intro"){
                currentProgressStep = "introL3";
                dialogue.text.Add("The final orb await you. We're getting close, I can feel it.");
            }
            else if(questTracker3.progress.LastOrDefault() == "triedToExit"){
                currentProgressStep = "exitDenied";
                krillVoice.Play();
                dialogue.text.Add("You need to collect the orb before you can get to the next realm, keep going mate");
            }
        }
        else if(who == "octoL2"){
            nameText.text = "Octopus";
            whoIsTalking = "octo";
            octoVoice.Play();

            if(questTracker2.progress.LastOrDefault() == "secondOrbFound"){
                currentProgressStep = "outroL2";
                dialogue.text.Add("Thank you for collecting the second orb. There is one left, hurry, humanity needs you!");
            }
            else if(questTracker2.progress.LastOrDefault() == "octoTipL2"){
                currentProgressStep = "tipL2";
                dialogue.text.Add("You found the first orb, it’s time to search for the next one. Search for the second orb in this deeper realm, I can feel its power…");
            }
        }
        else if(who == "octoL3"){
            nameText.text = "Octopus";
            whoIsTalking = "octo";
            octoVoice.Play();

            if(questTracker3.progress.LastOrDefault() == "octoTipL3"){
                currentProgressStep = "tipL3";
                dialogue.text.Add("Only one orb remains. Once all three orbs are united, we can lift the curse! Look hard, I know you will find it.");
            }
        }
        else if(who == "krillL3end"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            krillVoice.Play();

            if(questTracker3.progress.LastOrDefault() == "allOrbsFound"){
                currentProgressStep = "krillEnd";
                dialogue.text.Add("You did it! You collected all the orbs. Humankind will be freed from the curse and can return to the land. I can’t wait to get back to the pub!");
                dialogue.text.Add("Now let’s get out of here. Oh, here comes the octopus. He must be here to help us.");
            }
        }
        else if(who == "octoL3Betray"){
            nameText.text = "Octopus";
            whoIsTalking = "octo";
            octoVoice.Play();

            if(questTracker3.progress.LastOrDefault() == "octoL3BetrayMessage"){
                currentProgressStep = "endL3";
                dialogue.text.Add("At last, all three orbs have been united! I was doubtful at first, but you’ve proven yourself along this journey. In fact, you did all the work for me.");
                dialogue.text.Add("I may have been… dishonest with you. These orbs have the power to reverse the curse, but they also have the power to turn me into the strongest creature to ever rule the sea. It’s a power that I cannot pass up on.");
                dialogue.text.Add("With this new strength, I can complete my transformation into the kraken and take over not only the ocean, but all of mankind who will be trapped under the sea forever.");
                dialogue.text.Add("But you have shown yourself to be a worthy opponent. I cannot just let you leave, you’re the only one who could possibly stand in my way. We must now battle. Let the strongest creature of the ocean win!");
            }
        }
        else if(who == "krillL3end2"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            krillVoice.Play();
            if(questTracker3.progress.LastOrDefault() == "shrimpGoodLuck"){
                currentProgressStep = "endL3Krill";
                dialogue.text.Add("Well, I certainly didn’t see this coming, mate. Hope is not lost though! You’ve grown stronger on this journey and I’m confident you can defeat him.");
                dialogue.text.Add("Let’s follow him into the cave. You must fight him to take back the power and lift the curse. This is your chance; humanity depends on you! Now quick, let’s go.");
            }
        }
        else if(who == "krillBoss"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            krillVoice.Play();
            if(questTrackerBoss.progress.LastOrDefault() == "bossIntro"){
                currentProgressStep = "krillBossIntro";
                dialogue.text.Add("The octopus has transformed into the kraken. It's up to you to defeat him.");
            }
        }
        else if(who == "krillDefeat"){
            nameText.text = "Kenny the Krill";
            whoIsTalking = "krill";
            krillVoice.Play();
            if(questTrackerBoss.progress.LastOrDefault() == "dead"){
                currentProgressStep = "krillGameOutro";
                dialogue.text.Add("The curse... it's lifted! You did it. See you on the other side, mate!");
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

        if(questTracker3 != null){
            if(questTracker3.progress.LastOrDefault() == "octoL3BetrayMessage" && text.Count == 2){
                questTracker3.progress.Add("showOrbs");
            }
        }
        
        // Dequeue the dialogue sentence
        string sentence = text.Dequeue();
        Debug.Log(sentence);

        // Stop coroutines in case player presses enter before the sentence has finished being typed out
        StopAllCoroutines();

        // Start coroutine to type each letter of the dialogue sentence one at a time
        StartCoroutine(TypeSentence(sentence));

        // Playing the speech audio for the different characters
        /*
        if(whoIsTalking == "krill"){
            krillVoice.Play();
        }
        else if(whoIsTalking == "octo"){
            octoVoice.Play();
        }
        else if(whoIsTalking == "npc"){
            sideQuestVoice.Play();
        }
        */
    }

    IEnumerator TypeSentence(string sentence){
        // Clear current text
        dialogueText.text = "";

        // Cycle through each letter of the dialogue and add it to the displayed text
        foreach (char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
        }

        /*
        if(whoIsTalking == "krill"){
            krillVoice.Stop();
        }
        else if(whoIsTalking == "octo"){
            octoVoice.Stop();
        }
        else if(whoIsTalking == "npc"){
            sideQuestVoice.Stop();
        }
        */
    }

    public void EndDialogue(){
        // Hide the dialogue UI
        nameText.enabled = false;
        dialogueText.enabled = false;

        if(questTrackerBoss == null){
            contText.enabled = false;
            contTextA.enabled = false;
        }

        /*
        if(whoIsTalking == "krill"){
            krillVoice.Stop();
        }
        else if(whoIsTalking == "octo"){
            octoVoice.Stop();
        }
        else if(whoIsTalking == "npc"){
            sideQuestVoice.Stop();
        }
        */

        whoIsTalking = "";

        // Update the players progress in Quest Tracker
        if(currentProgressStep == "intro"){
            questTracker.progress.Add("intro");
        }
        else if(currentProgressStep == "firstEncounter"){
            questTracker.progress.Add("firstNpcEncounter");
        }
        else if(currentProgressStep == "howToHunt"){
            questTracker.progress.Add("learnedHunt");
        }
        else if(currentProgressStep == "complete"){
            questTracker.progress.Add("nextLevel");
        }
        else if(currentProgressStep == "allJelliesCaught"){
            questTracker.progress.Add("noMoreJellies");
        }
        else if(currentProgressStep == "jellyHints"){
            questTracker.progress.Add("jellyTipGiven");
        }
        else if(currentProgressStep == "introRace"){
            questTracker3.progress.Add("raceTime");
        }
        else if(currentProgressStep == "lost"){
            questTracker3.progress.Add("sendBack");
        }
        else if(currentProgressStep == "won"){
            questTracker3.progress.Add("givePrize");
        }
        else if(currentProgressStep == "introBlob"){
            questTracker2.progress.Add("startedBlob");
        }
        else if(currentProgressStep == "introJelly"){
            questTracker.progress.Add("startedJelly");
        }
        else if(currentProgressStep == "outroJelly"){
            questTracker.progress.Add("endedJelly");
        }
        else if(currentProgressStep == "introL2"){
            questTracker2.progress.Add("finishedIntro");
        }
        else if(currentProgressStep == "introL3"){
            questTracker3.progress.Add("finishedIntroL3");
        }
        else if(currentProgressStep == "failBlob"){
            questTracker2.progress.Add("failedBlob");
        }
        else if(currentProgressStep == "passBlob"){
            questTracker2.progress.Add("passedBlob");
        }
        else if(currentProgressStep == "outroL2"){
            questTracker2.progress.Add("nextLevel2");
        }
        else if(currentProgressStep == "tipL2"){
            questTracker2.progress.Add("continue");
        }
        else if(currentProgressStep == "tipL3"){
            questTracker2.progress.Add("continue");
        }
        else if(currentProgressStep == "endL3"){
            questTracker3.progress.Add("shrimpGoodLuck");
            questTracker3.interactingWith = "shrimpGL";
        }
        else if(currentProgressStep == "endL3Krill"){
            questTracker3.progress.Add("doneL3");
        }
        else if(currentProgressStep == "krillEnd"){
            questTracker3.progress.Add("octoBetrays");
            questTracker3.interactingWith = "octoL3";
        }
        else if(currentProgressStep == "krillBossIntro"){
            questTrackerBoss.progress.Add("fight");
        }
        else if(currentProgressStep == "krillGameOutro"){
            questTrackerBoss.progress.Add("rollCredits");
        }
        else if(currentProgressStep == "exitDenied"){
            questTrackerBoss.progress.Add("continue");
        }

        crab.GetComponent<NPC>().talking = false;
        jelly.GetComponent<NPCJelly>().talking = false;
    }
}
