using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryController))]
public class InventoryControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventoryController invContr = (InventoryController)target;
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Length", invContr.Length.ToString());

        for(int i = 0; i < invContr.Length; i++) {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("---", "---");
            Item item = invContr.GetItem(i);
            if(item != null) {
                EditorGUILayout.LabelField("Name", item.itemInfo.itemName);
                EditorGUILayout.LabelField("Value", "" + item.itemInfo.value);
                EditorGUILayout.LabelField("Stack", "" + item.stackAmount + "/" + item.itemInfo.maxStackAmount);
            }
            else {
                EditorGUILayout.LabelField("Name", "Empty");
            }
        }
    }
}
