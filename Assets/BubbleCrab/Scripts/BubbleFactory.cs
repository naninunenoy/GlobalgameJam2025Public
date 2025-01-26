using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BubbleCrab.n5y
{
    public class BubbleFactory : MonoBehaviour
    {
        [SerializeField] float SpawnInterval = 1.0f;
        [SerializeField] float PlayerDistance = 2.0f;
        [SerializeField] Transform[] SpawnPoints;
        [SerializeField] GameObject BubblePrefab;
        [SerializeField] BubbleSettings BubbleSettings;

        float timer = 0.0f;
        float[] spawnPointsX;
        int _lastSpawnPointIndex = -1;

        void Awake()
        {
            // 泡は5箇所から出現
            spawnPointsX = new float[SpawnPoints.Length];
            for (var i = 0; i < SpawnPoints.Length; i++)
            {
                spawnPointsX[i] = SpawnPoints[i].position.x;
            }
        }

        void Start()
        {
            var pointIndex = Random.Range(0, SpawnPoints.Length);
            SpawnBubble(pointIndex);
            _lastSpawnPointIndex = pointIndex;
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer > SpawnInterval)
            {
                timer = 0.0f;
                // 連続して同じ場所に出現しないようにする
                var indexes = Enumerable.Range(0, SpawnPoints.Length).Where(i => i != _lastSpawnPointIndex).ToArray();
                var pointIndex = indexes[Random.Range(0, indexes.Length)];
                SpawnBubble(pointIndex);
                _lastSpawnPointIndex = pointIndex;
            }
            // フレイヤーのややしたから泡を生み出す
            FollowPlayer();
        }

        void SpawnBubble(int pointIndex)
        {
            var spawnPos = new Vector3(spawnPointsX[pointIndex], transform.position.y, 0);
            var obj = Instantiate(BubblePrefab, spawnPos, Quaternion.identity, transform.parent);
            var bubble = obj.GetComponent<BubbleBehaviour>();
            var sizeType = Random.value < 0.5f ? BubbleSizeType.Tiny : BubbleSizeType.Small;
            var param = BubbleSettings.GetParams(sizeType);
            bubble.NotifySpawned(param.Scale, param.Speed);
            bubble.GetComponent<BubbleLifetimeBehaviour>().NotifySpawned(param.Lifetime, param.Power);
        }

        void FollowPlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
            var targetPos = new Vector3(0, player.transform.position.y - PlayerDistance, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }
    }
}
