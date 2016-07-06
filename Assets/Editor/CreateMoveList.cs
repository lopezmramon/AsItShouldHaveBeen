using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateMoveList
{
    [MenuItem("Assets/Create/Move List")]
    public static MoveList Create()
    {
        MoveList asset = ScriptableObject.CreateInstance<MoveList>();

        AssetDatabase.CreateAsset(asset, "Assets/MoveListObject.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
