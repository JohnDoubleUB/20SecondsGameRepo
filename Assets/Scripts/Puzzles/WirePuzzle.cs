using UnityEngine;

public class WirePuzzle : Puzzle
{

    public void SetPuzzle(int[] wireIndexes) 
    {
        for (int i = 0; i < wireIndexes.Length; i++) 
        {
            //index i connect with point wireIndexes[i]
            //Boom, shuffled wire connections
        }
    }

    public override void ButtonValue(int value)
    {
        //IDK actually, add this in tomorrow
    }
}
