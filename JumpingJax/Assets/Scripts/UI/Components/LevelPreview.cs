using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreview : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    public Image previewImage;
    public Transform scrollViewContent;

    public void Init(Level level)
    {
        previewImage.sprite = level.previewSprite;

        // make HTTP request from steam for leaderboard data
    }

}
