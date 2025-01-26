using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleCrab.n5y
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] CrabBehaviour crab;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI heightText;
        [SerializeField] TextMeshProUGUI maxHeightText;
        [SerializeField] Slider heightSlider;
        [SerializeField] TextMeshProUGUI pauseText;
        [SerializeField] TextMeshProUGUI resultTimeText;
        [SerializeField] Transform guideView;

        public void SetHeight(float height, bool isFall)
        {
            heightText.text = $"{Mathf.Max(height, 0F):F1}m";
            heightText.color = isFall ? Color.red : Color.white;
        }

        public void SetHeightRatio(float ratio)
        {
            heightSlider.value = ratio;
        }

        public void SetTime(float time)
        {
            timeText.text = $"{TimeSpan.FromSeconds(time):mm\\:ss}";
        }

        public void SetMaxHeight(float height)
        {
            maxHeightText.text = $"/{height:F1}m";
        }

        public void SetPause(bool isPause)
        {
            pauseText.gameObject.SetActive(isPause);
        }

        public void ShowResult(float time)
        {
            resultTimeText.text = $"Time: {TimeSpan.FromSeconds(time):mm\\:ss}";
        }

        public void SwitchGuideShowing(bool isShow)
        {
            guideView.gameObject.SetActive(isShow);
        }
    }
}
