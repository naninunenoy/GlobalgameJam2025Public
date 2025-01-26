using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BubbleCrab.n5y
{
    public class BubbleLifetimeBehaviour : MonoBehaviour
    {
        static readonly int shrink = Animator.StringToHash("shrink");
        static readonly int property = Animator.StringToHash("break");

        bool _willDestroy = false;

        float _lifetime;
        float _force;
        Animator _animator;
        IAudioManager _audioManager;

        public void NotifySpawned(float lifetime, float power)
        {
            _lifetime = lifetime;
            _force = power;
            _animator = GetComponentInChildren<Animator>(true);
            _audioManager = FindAnyObjectByType<AudioManager>();
        }

        void OnCollisionEnter(Collision other)
        {
            if (_willDestroy) return;

            if (other.gameObject.CompareTag("Player"))
            {
                _willDestroy = true;
                var othersGameObject = other.gameObject;
                _animator.SetTrigger(shrink);
                _audioManager.PlaySeHitBubble();

                //10秒ごに消える
                UniTask.Void(async () =>
                {
                    // 小さいほど寿命が短い
                    await UniTask.Delay(TimeSpan.FromSeconds(_lifetime), cancellationToken: destroyCancellationToken);
                    var player = othersGameObject.GetComponent<CrabBehaviour>();
                    // 破裂の衝撃をPlayerに与える
                    var playerRigidbody = othersGameObject.GetComponent<Rigidbody>();
                    var distance = Vector3.Distance(player.transform.position, transform.position);
                    var direction = (player.transform.position - transform.position).normalized;
                    var force = direction * _force / distance;
                    playerRigidbody.AddForce(force, ForceMode.Impulse);

                    _audioManager.PlaySeBreakBubble();
                    _animator.SetTrigger(property);
                    var effect = gameObject.GetComponentInChildren<ParticleSystem>();
                    effect.Play();
                    // パーティクルが発生してから消す
                    await UniTask.Delay(500, cancellationToken: destroyCancellationToken);
                    Destroy(gameObject);
                });
            }
        }
    }
}
