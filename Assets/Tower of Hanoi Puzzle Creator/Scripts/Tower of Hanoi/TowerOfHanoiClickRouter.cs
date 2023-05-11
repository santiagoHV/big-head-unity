using UnityEngine;
using UnityEngine.Events;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// A simple script to redirect click events.
    /// It's used with the default "Rod Selection System".
    /// </summary>
    class TowerOfHanoiClickRouter : MonoBehaviour
    {
        public UnityEvent routedClickEvents;

        public void OnMouseUpAsButton()
        {
            routedClickEvents.Invoke();
        }
    }
}
