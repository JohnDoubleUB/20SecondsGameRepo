using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

public class SessionData
{
    //List of all the codes (2 digit sections)
    string[] Codes;
    char[] ValidCharacters;
    static Random RandomInstance;
    string CompleteCode = string.Empty;
    int CurrentCodeIndex = 0;

    public SessionData(int codeCount, int codeLength, int? seed = null, char[] validChar = null)
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

        Codes = GenerateCodes(codeLength, codeCount, out CompleteCode);
    }

    private string[] GenerateCodes(int codeLength, int codeCount, out string completeCode) 
    {
        completeCode = string.Empty;

        string[] newArray = new string[codeCount];

        for(int row = 0; row < codeCount; row++)
        {
            for(int col = 0; col < codeLength; col++) 
            {
                char newCharacter = GetRandomCodeCharacter();
                newArray[row] += newCharacter;
                completeCode += newCharacter;
            }
        }

        return newArray;
    }

    public bool TryGetNextUnusedCode(out string code) 
    {
        if (CurrentCodeIndex < Codes.Length) 
        {
            code = Codes[CurrentCodeIndex];
            CurrentCodeIndex++;
            return true;
        }

        code = null;
        return false;
    }

    public bool AllCodesGiven() 
    { 
        return CurrentCodeIndex >= Codes.Length; 
    }

    private char GetRandomCodeCharacter() 
    {
        return ValidCharacters[RandomInstance.Next(0, ValidCharacters.Length)];
    }

    public string[] GetAllCodes() 
    {
        return Codes;
    }

    public string GetCompleteCode() 
    {
        return CompleteCode;
    }
}
