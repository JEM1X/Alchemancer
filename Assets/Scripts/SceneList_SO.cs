using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList_SO", menuName = "Scriptable Objects/SceneList_SO")]
public class SceneList_SO : ScriptableObject
{
    public SceneAsset[] scenes;
}
