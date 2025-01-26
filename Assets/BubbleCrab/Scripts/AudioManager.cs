using UnityEngine;

namespace BubbleCrab
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] AudioSource bgm;
        [SerializeField] AudioSource se;
        [SerializeField] AudioClip[] audioClips;

        public void Mute()
        {
            bgm.mute = true;
            se.mute = true;
        }

        public void Unmute()
        {
            bgm.mute = false;
            se.mute = false;
        }

        public void PlayBGM()
        {
            if (bgm.clip == null) bgm.clip = audioClips[0];
            Debug.Log("Play");
            bgm.Play();
        }

        public void PauseBGM()
        {
            Debug.Log("Pause");
            bgm.Pause();
        }

        public void SetBGMVolume(float volume)
        {
            bgm.volume = volume;
        }

        public void PlaySeHitBubble()
        {
            se.PlayOneShot(audioClips[1], 1.5F);
        }

        public void PlaySeBreakBubble()
        {
            se.PlayOneShot(audioClips[2]);
         //   se.PlayOneShot(audioClips[3]);//jump
        }
    }
}
