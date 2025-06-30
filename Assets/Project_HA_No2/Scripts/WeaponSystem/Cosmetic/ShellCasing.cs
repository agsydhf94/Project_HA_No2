using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Represents a physical shell casing that is ejected from a weapon.
    /// Applies random force and torque, spins visually, plays a bounce sound upon hitting the ground,
    /// and returns itself to the object pool after a short delay.
    /// </summary>
    public class ShellCasing : MonoBehaviour
    {
        [Header("Random Force Range")]
        public float minX, maxX;
        public float minY, maxY;
        public float minZ, maxZ;

        [Header("Torque & Spin")]
        public float minTorque, maxTorque;
        public float spinSpeed = 2500f;

        [Header("Sound")]
        public string[] shellSoundIDs;
        public LayerMask groundLayer;

        [Header("Casing Prefab Key")]
        public string casingPrefabKey;      // Key for identifying this casing in the object pool

        private IObjectReturn returner;
        private Rigidbody rb;
        private bool hasPlayedSound = false;
        private CancellationTokenSource cts;


        /// <summary>
        /// Initializes the casing with randomized physics behavior on spawn.
        /// </summary>
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.AddRelativeTorque(
                Random.Range(minTorque, maxTorque),
                Random.Range(minTorque, maxTorque),
                Random.Range(minTorque, maxTorque),
                ForceMode.Impulse);

            rb.AddRelativeForce(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                Random.Range(minZ, maxZ),
                ForceMode.Impulse);

            transform.rotation = Random.rotation;
            returner = ObjectManager.Instance;
        }


        /// <summary>
        /// Called when the casing is enabled. Starts a return timer.
        /// </summary>
        private void OnEnable()
        {
            hasPlayedSound = false;
            cts = new CancellationTokenSource();
            WaitAndReturn().Forget();
        }


        /// <summary>
        /// Cancels the return timer if the object is disabled early.
        /// </summary>
        private void OnDisable()
        {
            cts?.Cancel();
            cts?.Dispose();
        }


        /// <summary>
        /// Applies a continuous spin effect for visual feedback.
        /// </summary>
        private void FixedUpdate()
        {
            transform.Rotate(Vector3.right, spinSpeed * Time.deltaTime);
            transform.Rotate(Vector3.down, spinSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Waits for a fixed time before returning the casing to the pool.
        /// </summary>
        private async UniTask WaitAndReturn()
        {
            await UniTask.Delay(2000);
            returner.Return(casingPrefabKey, this);
        }


        /// <summary>
        /// Plays a shell collision sound once upon hitting a valid ground surface.
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (hasPlayedSound) return;

            if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                hasPlayedSound = true;

                if (shellSoundIDs != null && shellSoundIDs.Length > 0)
                {
                    string id = shellSoundIDs[Random.Range(0, shellSoundIDs.Length)];
                    SoundManager.Instance.PlaySound(id, transform.position);
                }
            }
        }
    }
}
