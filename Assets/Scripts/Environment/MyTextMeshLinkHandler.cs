using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MyTextMeshLinkHandler : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        var text = GetComponent<TextMeshProUGUI>();
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
            if (linkIndex > -1)
            {
                var linkInfo = text.textInfo.linkInfo[linkIndex];
                var linkId = linkInfo.GetLinkID();

                Debug.Log(linkId);
            }
        }
    }
}