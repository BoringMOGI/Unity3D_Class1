using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] AudioSource footstepAudio;         // ����� �ҽ�(����Ŀ)
    [SerializeField] AudioClip walkClip;                // �ȴ� �Ҹ�.
    [SerializeField] AudioClip runClip;                 // �ڴ� �Ҹ�.

    public void OnPlay(bool isWalk, bool isRun)
    {
        // �߼Ҹ� (Footstep)
        bool isPlayingAudio = footstepAudio.isPlaying;
        if (isWalk)
        {
            footstepAudio.clip = walkClip;
            if (isPlayingAudio == false)
                footstepAudio.Play();
        }
        else if (isRun)
        {
            footstepAudio.clip = runClip;
            if (isPlayingAudio == false)
                footstepAudio.Play();
        }
        else
        {
            footstepAudio.Stop();
        }
    }
}
