using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Represents an individual tab button with hover and click animations.
    /// Coordinates with a TabGroupUI and plays sound feedback.
    /// </summary>
    public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Animation Settings")]
        [SerializeField] private float hoverOffset = 10f;
        [SerializeField] private float tweenDuration = 0.15f;

        private RectTransform rectTransform;
        private Vector3 baseLocalPosition;
        private Tween currentTween;

        private bool isSelected = false;
        private bool initialized = false;
        private bool pendingInitialize = false;

        private TabGroupUI tabGroup;
        private SoundManager soundManager;
        private AudioSource audioSource;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            audioSource = GetComponent<AudioSource>();
            soundManager = SoundManager.Instance;

            if (audioSource == null)
                Debug.LogWarning($"{name} has no AudioSource component.");
        }


        /// <summary>
        /// Called when the GameObject becomes enabled. 
        /// Sets a flag for delayed initialization.
        /// </summary>
        private void OnEnable()
        {
            if (!initialized)
                pendingInitialize = true;
        }


        /// <summary>
        /// Checks for pending initialization each frame and executes it if necessary.
        /// </summary>
        private void Update()
        {
            if (pendingInitialize)
            {
                pendingInitialize = false;
                LazyInitialize();
            }
        }


        /// <summary>
        /// Lazily initializes the tab button (e.g., base position caching).
        /// This is called once after the GameObject is enabled.
        /// </summary>
        public void LazyInitialize()
        {
            if (initialized) return;

            baseLocalPosition = rectTransform.localPosition;
            initialized = true;

            ApplyBasePosition();
        }


        /// <summary>
        /// Applies the base Y-position of the button, accounting for selection offset.
        /// </summary>
        private void ApplyBasePosition()
        {
            float y = isSelected ? baseLocalPosition.y + hoverOffset : baseLocalPosition.y;
            rectTransform.localPosition = new Vector3(baseLocalPosition.x, y, baseLocalPosition.z);
        }


        /// <summary>
        /// Sets the parent TabGroupUI for this tab button.
        /// </summary>
        /// <param name="group">The TabGroupUI that manages this button.</param>
        public void SetTabGroup(TabGroupUI group)
        {
            tabGroup = group;
        }


        /// <summary>
        /// Called when the pointer hovers over the button.
        /// Triggers a hover animation if the button is not selected.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!initialized || isSelected) return;

            AnimateTo(baseLocalPosition.y + hoverOffset);

            if (audioSource != null)
                soundManager.PlaySound("button_Tab");
        }


        /// <summary>
        /// Called when the pointer exits the button area.
        /// Resets the position if the button is not selected.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!initialized || isSelected) return;

            AnimateTo(baseLocalPosition.y);
        }


        /// <summary>
        /// Called when the button is clicked.
        /// Notifies the parent TabGroupUI and plays sound feedback.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!initialized) return;

            tabGroup?.SelectTab(this);

            if (audioSource != null)
                soundManager.PlaySound("button_Accept");
        }


        /// <summary>
        /// Updates the selection state of this tab.
        /// Plays an animation if selected, or resets immediately if deselected.
        /// </summary>
        /// <param name="selected">Whether this tab is selected.</param>
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            isSelected = false;

            if (!initialized) return;

            float targetY = baseLocalPosition.y + (selected ? hoverOffset : 0);

            currentTween?.Kill();

            if (selected)
            {
                currentTween = rectTransform.DOLocalMoveY(targetY, tweenDuration).SetEase(Ease.OutBack);
            }
            else
            {
                rectTransform.localPosition = new Vector3(baseLocalPosition.x, targetY, baseLocalPosition.z);
            }
        }


        /// <summary>
        /// Animates the tab's Y-position to a target offset.
        /// </summary>
        /// <param name="targetY">The target Y local position.</param>
        private void AnimateTo(float targetY)
        {
            currentTween?.Kill();

            currentTween = rectTransform.DOLocalMoveY(targetY, tweenDuration).SetEase(Ease.OutQuad);
        }
    }
}
