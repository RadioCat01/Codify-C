using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private float typingSpeed = 0.05f;

    private string fullText;
    private int currentCharacterIndex;

    private void Awake()
    {
        fullText = textMesh.text;
        textMesh.text = ""; 
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char character in fullText)
        {
            textMesh.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
