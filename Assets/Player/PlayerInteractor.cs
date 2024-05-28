using UnityEngine;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [HideInInspector] public PlayerEvents events;
        IInteractor _interactor;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractor>(out IInteractor interactor))
            {
                _interactor = interactor;
            }
        }

        public void Update()
        {
            if (_interactor != null && Input.GetButtonDown("Use"))
                _interactor.StartEvent();
        }

        public void OnTriggerExit2D(Collider2D collision) => _interactor = null;
    }
}