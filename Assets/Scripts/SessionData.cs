using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

public class SessionData
{
    //List of all the codes (2 digit sections)
    char[,] Codes;
    char[] ValidCharacters;
    static Random RandomInstance;

    public SessionData(int codeLength, int codeCount, int? seed = null, char[] validChar = null)
    {
        ValidCharacters = validChar == null ? new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } : validChar;

        if(seed == null) 
        { 
            RandomInstance = new Random(); 
        }
        else
        {
            RandomInstance = new Random(seed.Value);
        }

        Codes = GenerateCodes(codeLength, codeCount);
    }

    private char[,] GenerateCodes(int codeLength, int codeCount) 
    {
        char[,] newArray = new char[codeLength, codeCount];

        for(int row = 0; row < codeLength; row++)
        {
            for(int col = 0; col < codeCount; col++) 
            {
                newArray[row, col] = GetRandomCodeCharacter();
            }
        }

        return newArray;
    }

    private char GetRandomCodeCharacter() 
    {
        return ValidCharacters[RandomInstance.Next(0, ValidCharacters.Length)];
    }

    public char[,] GetAllCodes() 
    {
        return Codes;
    }
}
