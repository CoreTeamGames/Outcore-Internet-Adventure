using UnityEngine;

namespace OutcoreInternetAdventure.DialogueSystem
{
    public class DialogueCharacter : MonoBehaviour, IInteractor
    {
        #region Variables
        [SerializeField] bool _speaked = false;
        [SerializeField] bool _canSpeakAgain = false;
        [SerializeField] bool _canMovingOnDialogue = false;
        [SerializeField] string dialogueForStart;
        DialogueGameWindow dialogueWindow;

        public bool CanInteract => !_speaked;
        #endregion

        #region Code
        public void Awake() => dialogueWindow = GameObject.FindGameObjectWithTag("Dialogue window").GetComponent<DialogueGameWindow>();

        public void StartEvent()
        {
            if (!_speaked)
            {
                dialogueWindow.StartDialogue(dialogueForStart);
                _speaked = _canSpeakAgain == true ? false : true;
            }
        }
        #endregion
    }
}