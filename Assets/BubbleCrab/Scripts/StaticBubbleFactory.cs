using System.Linq;
using BubbleCrab.n5y;
using UnityEngine;

namespace BubbleCrab
{
    public class StaticBubbleFactory : MonoBehaviour
    {
        [SerializeField] float xMin = -3.5f;
        [SerializeField] float xMax = 3.5f;
        [SerializeField] float distance = 20.0f;
        [SerializeField] float maxHeight = 180.0f;
        [SerializeField] GameObject bubblePrefab;
        [SerializeField] BubbleSettings bubbleSettings;

        void Start()
        {
            // 一定間隔でBig泡を生成
            for (var currentHeight = distance; currentHeight < maxHeight; currentHeight += distance)
            {
                var x = Random.Range(xMin, xMax);
                var spawnPos = new Vector3(x, currentHeight, 0);
                var obj = Instantiate(bubblePrefab, spawnPos, Quaternion.identity, transform.parent);
                // デカ玉はPlayerとの位置関係で削除しない
                obj.AddComponent<DontDestroyBubble>();
                var bubble = obj.GetComponent<BubbleBehaviour>();
                var param = bubbleSettings.GetParams(BubbleSizeType.Large);
                bubble.NotifySpawned(param.Scale, param.Speed);
                bubble.GetComponent<BubbleLifetimeBehaviour>().NotifySpawned(param.Lifetime, param.Power);
            }
        }
    }
}
