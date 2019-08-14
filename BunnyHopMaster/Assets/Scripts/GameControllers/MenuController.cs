using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void OnClickHumanCharacter(int character)
    {
        if(PlayerInfo.PI != null)
        {
            PlayerInfo.PI.selectedHumanCharacter = character;
            PlayerPrefs.SetInt("HumanCharacter", character);
        }
    }

    public void OnClickInfectedCharacter(int character)
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.selectedInfectedCharacter = character;
            PlayerPrefs.SetInt("InfectedCharacter", character);
        }
    }
}
