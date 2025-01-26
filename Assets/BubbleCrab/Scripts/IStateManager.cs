namespace BubbleCrab
{
    public interface IStateManager
    {
        void ToMain();
        void ToResult();
        void ToTitle();
        GameState GetCurrentState();
    }
}
