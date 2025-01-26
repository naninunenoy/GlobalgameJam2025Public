using System;
using UnityEngine;

namespace BubbleCrab
{
    [CreateAssetMenu(fileName = "BubbleSettings", menuName = "BubbleCrab/BubbleSettings")]
    public class BubbleSettings : ScriptableObject
    {
        [SerializeField] BubbleParams tiny;
        [SerializeField] BubbleParams small;
        [SerializeField] BubbleParams medium;
        [SerializeField] BubbleParams large;

        public BubbleParams GetParams(BubbleSizeType sizeType)
        {
            return sizeType switch
            {
                BubbleSizeType.Tiny => tiny,
                BubbleSizeType.Small => small,
                BubbleSizeType.Medium => medium,
                BubbleSizeType.Large => large,
                _ => throw new Exception("Invalid BubbleSizeType"),
            };
        }
    }
}
