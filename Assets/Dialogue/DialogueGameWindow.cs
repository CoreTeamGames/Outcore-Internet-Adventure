using System.Collections;
using TMPro;
using UnityEngine;

namespace OutcoreInternetAdventure.DialogueSystem
{
    public class DialogueGameWindow : MonoBehaviour
    {
        #region Events
        public delegate void onPlayerSkipWrite();
        public event onPlayerSkipWrite onPlayerSkipWriteEvent;

        public delegate void onNewLineStartsWriting();
        public event onNewLineStartsWriting onNewLineStartsWritingEvent;

        public delegate void onDialogueStarts();
        public event onDialogueStarts onDialogueStartsEvent;

        public delegate void onDialogueEnd();
        public event onDialogueEnd onDialogueEndEvent;
        #endregion

        #region Variables

        #region Public variables
        //Text asset of dialogue.
        public TMP_Text dialogueText;
        //Text asset for left character name.
        public TMP_Text lNameText;
        //Text asset for right character name.
        public TMP_Text rNameText;
        //Audio source for character voices.
        public AudioSource _source;
        //Script for making dialogs.
        public Dialogue dialogueScript;
        //Localizator script for get strings(dialogue lines or character names).
        public Localization localizator;
        //Bool for checking dialogue line is output, or not.
        public static bool isStarted = false;
        //


        //Variable for checks step of dialogue.
        [Tooltip("If this equals 0, dialogue window or dialog not opened")]
        public int currentStep = 0;
        //
        public GameObject window;
        public Player.PlayerInputManager _input;


        [HideInInspector]
        public bool canDash;
        [HideInInspector]
        public bool canWalk;
        [HideInInspector]
        public bool canFly;

        #endregion

        #region Internal variables
        //Internal bool for check, can player skip line output to end.
        private static bool canSkip = true;
        //Internal variable for keep loaded dialogue line.
        private string _string;
        //
        [SerializeField] bool dialogueStarted = false;
        #endregion

        #endregion

        #region Dialogue script
        void GetVars()
        {
            //Get vars from localization script.
            //lNameText.text = localizator.characterNames[dialogueScript.dialogueSteps[currentStep].lNameID];
            //rNameText.text = localizator.characterNames[dialogueScript.dialogueSteps[currentStep].rNameID];
            canSkip = dialogueScript.DialogueLines[currentStep].CanSkipOutput;
        }

        public void StartDialogue()
        {
            onDialogueStartsEvent?.Invoke();
            currentStep = 0;
            window.SetActive(true);
            dialogueStarted = true;
            GetVars();
            WriteLine();
        }
        void WriteLine()
        {
            onNewLineStartsWritingEvent?.Invoke();
            //_string = localizator.dialogueLines[dialogueScript.dialogueSteps[currentStep].stringID];
            //Starting a output of dialogue line.
            StartCoroutine(CharOutput(_string, dialogueScript.DialogueLines[currentStep].OutputSpeed, dialogueScript.DialogueLines[currentStep].EnableVoice, dialogueScript.DialogueLines[currentStep].DialogueCharacter.Voice));
        }

        public void EndDialogue()
        {
            onDialogueEndEvent?.Invoke();
            dialogueStarted = false;
            window.SetActive(false);
        }


        public void Update()
        {
            if (dialogueStarted)
            {
                //If player press button(s) for skip line & this line is outputs & player can skip this line to end of output, make this.
                if (isStarted && Input.GetButtonDown("SkipTextLine") && canSkip)
                {
                    isStarted = false;
                    StopAllCoroutines();
                    _source.Stop();
                    dialogueText.text = _string;
                }
                //If player press button(s) for skip line & this line isn't outputs, set next step of dialogue.
                else if (!isStarted && Input.GetButtonDown("SkipTextLine"))
                {
                    if (dialogueScript.DialogueLines.Count - 1 > currentStep)
                    {
                        currentStep++;
                        WriteLine();
                    }
                    else
                    { EndDialogue(); }
                }
            }
        }

        #endregion

        #region Functions for script
        //Output of line.
        IEnumerator CharOutput(string text, float delay = 0.02f, bool voiceEnabled = false, AudioClip clip = null)
        {
            isStarted = true;
            dialogueText.text = "";

            foreach (var sym in text)
            {
                print(sym);
                dialogueText.text += sym;
                if (voiceEnabled && clip != null)
                    _source.PlayOneShot(clip);

                yield return new WaitForSeconds(delay);
            }

            isStarted = false;
        }
        #endregion
    }
}