using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Makes a UI Image blink by animating its alpha using Mathf.PingPong.
    /// Intended for attention-grabbing visual feedback.
    /// </summary>
    public class ImageBlinker : MonoBehaviour
    {
        public Image targetImage;
        public float blinkSpeed = 2f;
        public float maxAlpha = 0.66f;

        public async UniTask BlinkForSeconds(float seconds, CancellationToken ct = default)
        {
            float timer = 0f;
            Color color = targetImage.color;

            while (timer < seconds)
            {
                if (ct.IsCancellationRequested) break;

                float alpha = Mathf.PingPong(Time.time * blinkSpeed, maxAlpha);
                color.a = alpha;
                targetImage.color = color;

                timer += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            color.a = 0f;
            targetImage.color = color;
        }
    }
}
