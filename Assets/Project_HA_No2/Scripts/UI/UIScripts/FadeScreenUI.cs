using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class FadeScreenUI : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void FadeOut() => animator.SetTrigger("FadeOut");
        public void FadeIn() => animator.SetTrigger("FadeIn");
    }
}
