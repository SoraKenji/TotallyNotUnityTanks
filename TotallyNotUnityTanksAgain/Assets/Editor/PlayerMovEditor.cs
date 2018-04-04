
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //PlayerMovement plMov = (PlayerMovement)target;

        //if(GUILayout.Button("Make it spin"))
        //{
        //    plMov.makeItFlip();
        //}
    }
}
