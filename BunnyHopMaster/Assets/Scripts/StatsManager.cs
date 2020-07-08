using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static void TryUpdateStat(string levelName, float newCompletionTime)
    {
        var levelCompletionTime = Steamworks.SteamUserStats.GetStatFloat(levelName);

        if(newCompletionTime < levelCompletionTime || levelCompletionTime == 0)
        {
            Steamworks.SteamUserStats.SetStat(levelName, newCompletionTime);
        }

        Steamworks.SteamUserStats.StoreStats();
    }
}
