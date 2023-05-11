using UnityEditor;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// This custom editor (which shows nothing), 
    /// is to avoid that evil people mess up 
    /// the TowerOfHanoiDisksData scriptable object.
    /// Changes to it should only be made by interacting with
    /// the TowerOfHanoiCreator script inspector.
    /// Nonetheless, you can still mess it up if you 
    /// enable the Debug mode.
    /// </summary>
    [CustomEditor(typeof(TowerOfHanoiDisksData))]
    public class TowerOfHanoiDisksDataEditor : Editor
    {
        public override void OnInspectorGUI()
        { }
    }
}