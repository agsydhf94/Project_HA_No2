using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace HA
{
    /// <summary>
    /// Smoothly fills or unfills a UI Image's fillAmount over a specified duration.
    /// Useful for progress bars or UI transitions.
    /// </summary>
    public class ImageFiller : MonoBehaviour
    {
        public Image targetImage;
        public float fillDuration = 0.1f;

        public async UniTask FillTo(float target, CancellationToken ct = default)
        {
            float start = targetImage.fillAmount;
            float elapsed = 0f;

            while (elapsed < fillDuration)
            {
                if (ct.IsCancellationRequested) break;

                elapsed += Time.deltaTime;
                targetImage.fillAmount = Mathf.Lerp(start, target, elapsed / fillDuration);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            targetImage.fillAmount = target;
        }
    }
}
