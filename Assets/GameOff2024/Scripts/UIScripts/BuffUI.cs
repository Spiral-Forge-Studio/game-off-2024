using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public GameObject BuffItemPrefab; // Prefab with Image and Text for a buff
    public Transform DiscoveredBuffsParent;
    public Transform UndiscoveredBuffsParent;

    private void PopulateUI()
    {
        foreach (Transform child in DiscoveredBuffsParent) Destroy(child.gameObject);
        foreach (Transform child in UndiscoveredBuffsParent) Destroy(child.gameObject);
        /*
        foreach (var buff in BuffManager)
        {
            GameObject newBuffItem = Instantiate(BuffItemPrefab);
            if (buff.IsDiscovered)
            {
                newBuffItem.transform.SetParent(DiscoveredBuffsParent, false);
                newBuffItem.GetComponentInChildren<Text>().text = buff.Name;
                newBuffItem.GetComponentInChildren<Image>().sprite = buff.Icon;
            }
            else
            {
                newBuffItem.transform.SetParent(UndiscoveredBuffsParent, false);
                newBuffItem.GetComponentInChildren<Text>().text = "???";
                //newBuffItem.GetComponentInChildren<Image>().sprite = PlaceholderSprite;
            }
        }*/
    }
}
