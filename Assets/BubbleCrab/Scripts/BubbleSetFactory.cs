using UnityEngine;

namespace BubbleCrab.n5y
{
    public class BubbleSetFactory : MonoBehaviour
    {
        [SerializeField] float interval = 10.0f;
        [SerializeField] GameObject[] BubbleSetsPrefabs;
        [SerializeField] BubbleSettings BubbleSettings;

        bool _isFirstSpawn = true;
        float _timer = 0.0f;
        IStateManager _stateManager;

        void Start()
        {
            _stateManager = FindAnyObjectByType<StateManager>();
        }

        void Update()
        {
            if (_stateManager.GetCurrentState() != GameState.Main) return;
            if (_isFirstSpawn)
            {
                // 最初はAをSpawn
                SpawnSet(0);
                _isFirstSpawn = false;
                return;
            }

            _timer += Time.deltaTime;
            if (_timer > interval)
            {
                _timer = 0.0f;
                var index = Random.Range(0, BubbleSetsPrefabs.Length);
                SpawnSet(index);
            }
        }

        void SpawnSet(int index)
        {
            var set = Instantiate(BubbleSetsPrefabs[index], transform.position, Quaternion.identity, transform);
            var param = BubbleSettings.GetParams(BubbleSizeType.Medium);
            foreach (var x in set.GetComponentsInChildren<BubbleBehaviour>())
            {
                x.NotifySpawned(param.Scale, param.Speed);
                x.GetComponent<BubbleLifetimeBehaviour>().NotifySpawned(param.Lifetime, param.Power);
            }
        }
    }
}
