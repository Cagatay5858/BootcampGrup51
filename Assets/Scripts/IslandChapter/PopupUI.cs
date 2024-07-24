using System.Collections;
using TMPro;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI notEnoughText;
    public TextMeshProUGUI stickCountText;
    public TextMeshProUGUI plantCountText;

    private void Start()
    {
        HidePopup();
    }

    public void ShowPopup(int stickCount, int requiredStickCount, int plantCount, int requiredPlantCount)
    {
        string stickMessage = stickCount < requiredStickCount
            ? $"Not enough sticks! ({stickCount} / {requiredStickCount})\n"
            : $"Sticks: {stickCount} / {requiredStickCount}\n";

        string plantMessage = plantCount < requiredPlantCount
            ? $"Not enough plants! ({plantCount} / {requiredPlantCount})"
            : $"Plants: {plantCount} / {requiredPlantCount}";

        notEnoughText.text = stickMessage + plantMessage;

        popupPanel.SetActive(true);
        StartCoroutine(HidePopupAfterDelay(2.5f));
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePopup();
    }
}