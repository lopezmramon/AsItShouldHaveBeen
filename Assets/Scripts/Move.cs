using UnityEngine;
using System.Collections;

[System.Serializable]                                                         
public class Move
{
    public string moveName = "New Move";
    public int ID;
    public enum MoveType {
        Basic, Hard, Smushy, Swift
       

    }
    public MoveType moveType;
    public enum MoveQuality
    {
        Offensive, Defensive, Status, Mixed



    }
    public MoveQuality moveQuality;
    public string moveDescription = "What does this do?";
    public Sprite moveIcon = null;                                         
   
    public int movePower = 0;

}