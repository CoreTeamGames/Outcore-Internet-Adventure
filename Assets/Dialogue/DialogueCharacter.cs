using UnityEngine;

namespace OutcoreInternetAdventure.DialogueSystem
{
    public class DialogueCharacter : MonoBehaviour, IInteractor
    {
        #region Variables
        [HideInInspector] public bool speaked = false;
        public bool canSpeakAgain = false;
        public bool canMovingOnDialogue = false;

        [SerializeField] Dialogue dialogueForStart;
        DialogueGameWindow dialogueWindow;
        #endregion

        #region Code
        public void Awake() => dialogueWindow = GameObject.FindGameObjectWithTag("Dialogue window").GetComponent<DialogueGameWindow>();

        public void StartEvent()
        {
            if (!speaked)
            {
                dialogueWindow.dialogueScript = dialogueForStart;
                dialogueWindow.StartDialogue();
                speaked = true;
            }
            dialogueWindow.onDialogueEndEvent += SetSpeaked;
        }

        void SetSpeaked() => speaked = !canSpeakAgain;
        #endregion
    }
}