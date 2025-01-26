using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace BubbleCrab.n5y
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineCamera virtualCamera;

        public void OnCrabFall(bool isFalling)
        {
            // 落下中はカメラのフォローの縦方向のオフセットを加算し、下が見える様にする
            var offset = virtualCamera.GetComponent<CinemachineFollow>();
            var targetOffset = isFalling ? -1.0f : 0.3f;
            DOTween.To(() => offset.FollowOffset.y, x => offset.FollowOffset.y = x, targetOffset, 0.5f);
        }
    }
}
