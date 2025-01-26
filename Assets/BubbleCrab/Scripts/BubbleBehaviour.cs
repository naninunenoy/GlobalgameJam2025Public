using UnityEngine;

namespace BubbleCrab.n5y
{
    public class BubbleBehaviour : MonoBehaviour
    {
        [SerializeField] Rigidbody _rigidbody;

        float _speed = 1.0f;
        Renderer _renderer;
        bool _isDestroyByDistance = true;
        CrabBehaviour _player;

        public void NotifySpawned(float scale, float speed)
        {
            _speed = speed;
            transform.localScale = scale * Vector3.one;
            _renderer = GetComponent<Renderer>();
            _player = FindAnyObjectByType<CrabBehaviour>();
            _isDestroyByDistance = GetComponent<DontDestroyBubble>() == null;
        }

        void Update()
        {
            // DontDestroyBubble(デカ玉)は削除しない
            if (_isDestroyByDistance)
            {
                // playerより上かつ描画範囲外で削除
                if (_player != null && _renderer != null)
                {
                    if (!_renderer.isVisible && transform.position.y > _player.transform.position.y)
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
            // 高さ200以上で削除
            if (transform.position.y > 200)
            {
                Destroy(gameObject);
                return;
            }
            // 自動で上に上がる等速直線運動
            _rigidbody.AddForce((_speed - _rigidbody.linearVelocity.y) * Vector3.up, ForceMode.Acceleration);
        }
    }
}
