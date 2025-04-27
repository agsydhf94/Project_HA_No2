using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HA
{
    public class CheckPopUpUI : MonoBehaviour
    {
        public static CheckPopUpUI instance;
        public static CheckPopUpUI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CheckPopUpUI>();
                }
                return instance;
            }
        }

        public TMP_Text textComponent;
        public TMP_Text tempTMP;
        public string textData;
        public float showingSpeed;
        private int currentTextIndex = 0;
        float timer = 0f;


        public Image blinkingImage;
        public Color blinkingColor;
        public float blinkSpeed;

        public Image upperBarImage;
        public Image backGroundImage;

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            textComponent.text = "";
        }

        public void WriteText(string textInput)
        {
            while (currentTextIndex < textInput.Length)
            {
                timer += Time.deltaTime;
                if (timer > showingSpeed)
                {
                    textComponent.text += textInput[currentTextIndex].ToString();
                    currentTextIndex++;
                    timer = 0f;
                }
            }
            currentTextIndex = 0;
        }

        public void ImageBlink()
        {
            // ���� ���� PingPong���� ����
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 0.66f) + 0.01f;

            Color newColor = blinkingImage.color;
            newColor.a = alpha;
            blinkingImage.color = newColor;
        }



        // UI �ִϸ��̼� �������� �����ϴ� �޼���
        public void StartUIAnimationSequence(string textInput, Color blinkingColorInput)
        {
            StartCoroutine(UIAnimationSequence(textInput, blinkingColorInput));
        }

        // UI �ִϸ��̼� ������
        private IEnumerator UIAnimationSequence(string textInput, Color blinkingColorInput)
        {
            // 1. upperBarImage�� backGroundImage ä���
            yield return StartCoroutine(FillImage(upperBarImage, 1f));
            yield return StartCoroutine(FillImage(backGroundImage, 1f));

            Debug.Log(textInput);
            textComponent.maxVisibleCharacters = textInput.Length;
            WriteText(textInput);


            // 2. �̹��� �������� 3�� ���� ����
            float blinkTimer = 3f;

            Color newColor = blinkingColorInput;
            blinkingImage.color = newColor;

            while (blinkTimer > 0f)
            {
                ImageBlink();
                blinkTimer -= Time.deltaTime;
                yield return null;
            }

            // �����Ÿ��� ���߱� ���� ���� ���� 1�� ����
            Color temp_Transparent = blinkingColorInput;
            temp_Transparent.a = 0f;
            blinkingImage.color = temp_Transparent;



            yield return StartCoroutine(FadeOutText(textInput));


            // 3. backGroundImage�� upperBarImage�� õõ�� ���̱�
            yield return StartCoroutine(FillImage(backGroundImage, 0f));
            yield return StartCoroutine(FillImage(upperBarImage, 0f));
        }

        // fillAmount�� ���������� ����/���ҽ�Ű�� �ڷ�ƾ
        private IEnumerator FillImage(Image image, float targetFillAmount)
        {
            float startFillAmount = image.fillAmount;
            float elapsed = 0f;
            float duration = 0.1f; // ���� �ð� ���� ����

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsed / duration);
                yield return null;
            }

            image.fillAmount = targetFillAmount; // Ÿ�ٿ� ��Ȯ�� ���߱�
        }

        private IEnumerator FadeOutText(string textInput)
        {
            int totalCharacters = textInput.Length;
            tempTMP.text = textInput;
            for (int i = totalCharacters; i >= 0; i--)
            {

                textComponent.maxVisibleCharacters = i; // ǥ���� �ִ� ������ ������ ���� ����
                yield return new WaitForSeconds(0.025f); // ���ڰ� ������� �ӵ� ����
            }
            tempTMP.text = "";
            textComponent.text = "";
        }
    }
}

