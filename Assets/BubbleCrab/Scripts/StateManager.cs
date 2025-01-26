using UnityEngine;

namespace BubbleCrab
{
    public enum GameState { Title, Main, Result }
    public class StateManager : MonoBehaviour, IStateManager
    {
        [SerializeField] GameObject[] TitleShowObjects;
        [SerializeField] GameObject[] TitleUnshowObjects;
        [SerializeField] GameObject[] ResultShowObjects;
        GameState _currentState = GameState.Title;
        public GameState GetCurrentState() => _currentState;

        public void ToMain()
        {
            foreach (var x in TitleShowObjects)
            {
                x.SetActive(false);
            }
            foreach (var x in TitleUnshowObjects)
            {
                x.SetActive(true);
            }
            foreach (var x in ResultShowObjects)
            {
                x.SetActive(false);
            }
            _currentState = GameState.Main;
        }

        public void ToResult()
        {
            foreach (var x in ResultShowObjects)
            {
                x.SetActive(true);
            }
            _currentState = GameState.Result;
        }

        public void ToTitle()
        {
            foreach (var x in TitleShowObjects)
            {
                x.SetActive(true);
            }
            foreach (var x in TitleUnshowObjects)
            {
                x.SetActive(false);
            }
            foreach (var x in ResultShowObjects)
            {
                x.SetActive(false);
            }
            _currentState = GameState.Title;
        }
    }
}
