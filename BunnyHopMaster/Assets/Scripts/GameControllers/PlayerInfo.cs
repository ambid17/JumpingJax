using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    public int selectedHumanCharacter;
    public int selectedInfectedCharacter;

    public GameObject[] humanCharacters;
    public GameObject[] infectedCharacters;

    void Awake()
    {

        //TODO: understand else-if
        if(PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else if (PlayerInfo.PI == this)
        {
            Destroy(PlayerInfo.PI.gameObject);
            PlayerInfo.PI = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("InfectedCharacter"))
        {
            selectedInfectedCharacter = PlayerPrefs.GetInt("InfectedCharacter");
        }
        else
        {
            selectedInfectedCharacter = 0;
            PlayerPrefs.SetInt("InfectedCharacter", selectedInfectedCharacter);
        }

        if (PlayerPrefs.HasKey("HumanCharacter"))
        {
            selectedHumanCharacter = PlayerPrefs.GetInt("HumanCharacter");
        }
        else
        {
            selectedHumanCharacter = 0;
            PlayerPrefs.SetInt("HumanCharacter", selectedHumanCharacter);
        }
    }
}
