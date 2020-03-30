using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "audioContainer", menuName = "ScriptableObjects/audioContainer")]
public class AudioClipContainer : ScriptableObject
{
    public AudioClip[] audioClips;
}
