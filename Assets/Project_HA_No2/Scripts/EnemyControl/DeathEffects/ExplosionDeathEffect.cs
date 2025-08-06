using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Applies an explosion visual effect when a character dies.
    /// 
    /// This class implements the IDeathEffect interface and plays a VFX
    /// at the target's position using the provided vfxKey. The VFX is 
    /// triggered via the VFXManager using an object pool for performance.
    /// </summary>
    public class ExplosionDeathEffect : MonoBehaviour, IDeathEffect
    {
        [SerializeField] private string vfxKey;

        public ExplosionDeathEffect(string vfxKey)
        {
            this.vfxKey = vfxKey;
        }

        /// <summary>
        /// Plays the explosion VFX at the target's position with a short delay.
        /// </summary>
        /// <param name="target">The GameObject that died, used to determine the VFX position.</param>
        public async UniTask PlayAsync(GameObject target)
        {
            await UniTask.Delay(1);

            Vector3 pos = target.transform.position;
            Quaternion rot = Quaternion.identity;

            VFXManager.Instance.PlayEffect(vfxKey, pos, rot, null, 2f, VFXSourceType.ObjectPool);
        }
    }
}
