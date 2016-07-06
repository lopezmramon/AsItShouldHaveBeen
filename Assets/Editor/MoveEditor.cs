using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MoveEditor : EditorWindow
{
   public  Move.MoveQuality selectedQuality;
    public Move.MoveType selectedType;
    public MoveList moveList;
    private int viewIndex = 1;

    [MenuItem("Window/Move Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(MoveEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            moveList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(MoveList)) as MoveList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Move Editor", EditorStyles.boldLabel);
        if (moveList != null)
        {
            if (GUILayout.Button("Show Move List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = moveList;
            }
        }
        if (GUILayout.Button("Open Move List"))
        {
            OpenMoveList();
        }
        if (GUILayout.Button("New Move List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = moveList;
        }
        GUILayout.EndHorizontal();

        if (moveList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Move List", GUILayout.ExpandWidth(false)))
            {
                CreateNewMoveList();
            }
            if (GUILayout.Button("Open Existing Move List", GUILayout.ExpandWidth(false)))
            {
                OpenMoveList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (moveList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < moveList.moveList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Move", GUILayout.ExpandWidth(false)))
            {
                AddMove();
            }
            if (GUILayout.Button("Delete Move", GUILayout.ExpandWidth(false)))
            {
                DeleteMove(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (moveList.moveList == null)
                Debug.Log("wtf");
            if (moveList.moveList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Move", viewIndex, GUILayout.ExpandWidth(false)), 1, moveList.moveList.Count);
                //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
                EditorGUILayout.LabelField("of   " + moveList.moveList.Count.ToString() + "  moves", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                moveList.moveList[viewIndex - 1].ID = viewIndex;
                    EditorGUILayout.SelectableLabel(moveList.moveList[viewIndex - 1].ID.ToString());
                moveList.moveList[viewIndex - 1].moveName = EditorGUILayout.TextField("Move Name", moveList.moveList[viewIndex - 1].moveName as string);
                moveList.moveList[viewIndex - 1].moveIcon = EditorGUILayout.ObjectField("Move Icon", moveList.moveList[viewIndex - 1].moveIcon, typeof(Sprite), false) as Sprite;
                moveList.moveList[viewIndex - 1].moveType = (Move.MoveType) EditorGUILayout.EnumPopup("Move Type", moveList.moveList[viewIndex - 1].moveType);
                

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                moveList.moveList[viewIndex - 1].moveQuality = (Move.MoveQuality)EditorGUILayout.EnumPopup("Move Quality", moveList.moveList[viewIndex - 1].moveQuality);



                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                moveList.moveList[viewIndex - 1].moveDescription = EditorGUILayout.TextArea(moveList.moveList[viewIndex - 1].moveDescription, GUILayout.Height( 150));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();

                moveList.moveList[viewIndex - 1].movePower = EditorGUILayout.IntField("Move Power", moveList.moveList[viewIndex - 1].movePower, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
            else
            {
                GUILayout.Label("This Move List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(moveList);
        }
    }

    void CreateNewMoveList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        moveList = CreateMoveList.Create();
        if (moveList)
        {
            moveList.moveList = new List<Move>();
            string relPath = AssetDatabase.GetAssetPath(moveList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenMoveList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Move List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            moveList = AssetDatabase.LoadAssetAtPath(relPath, typeof(MoveList)) as MoveList;
            if (moveList.moveList == null)
                moveList.moveList = new List<Move>();
            if (moveList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddMove()
    {
        Move newMove = new Move();
        newMove.moveName = "New Move";
        moveList.moveList.Add(newMove);
        viewIndex = moveList.moveList.Count;
    }

    void DeleteMove(int index)
    {
        moveList.moveList.RemoveAt(index);
    }
}