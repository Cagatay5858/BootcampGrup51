using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterEndUIController : MonoBehaviour
{
    public Button continueButton;
    public TMP_Text buttonText;

    public void Setup(System.Action onContinue)
    {
        continueButton.onClick.AddListener(() => onContinue?.Invoke());
        buttonText.text = "anladim";
    }
}