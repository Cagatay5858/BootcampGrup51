using System.Collections;
using TMPro;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI notEnoughText;
    public TextMeshProUGUI stickCountText;

    private void Start()
    {
        HidePopup();
    }

    public void ShowPopup(int stickCount, int requiredStickCount)
    {
        notEnoughText.text = "Not enough sticks!";
        stickCountText.text = "Sticks: " + stickCount + " / " + requiredStickCount;
        popupPanel.SetActive(true);
        StartCoroutine(HidePopupAfterDelay(2.0f)); 
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