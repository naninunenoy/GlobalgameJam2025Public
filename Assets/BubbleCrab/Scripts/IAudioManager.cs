namespace BubbleCrab
{
    public interface IAudioManager
    {
        void Mute();
        void Unmute();
        void PlayBGM();
        void PauseBGM();
        void SetBGMVolume(float volume);
        void PlaySeHitBubble();
        void PlaySeBreakBubble();
    }
}
