using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusDebugger : MonoBehaviour
{
    public PlayerStatusSO playerStatusSO;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI flatBonusText;

    private const string redColorTagStart = "<color=red>";
    private const string colorTagEnd = "</color>";

    void Update()
    {
        // Build multiplier text
        multiplierText.text = "Multipliers:\n" + BuildStatText(
            playerStatusSO.multipliers,
            key => 1f // Original multiplier value for comparison
        );

        // Build flat bonus text
        flatBonusText.text = "Flat Bonuses:\n" + BuildStatText(
            playerStatusSO.flatBonuses,
            key => 0f // Original flat bonus value for comparison
        );
    }

    private string BuildStatText<T>(Dictionary<T, float> statDictionary, System.Func<T, float> getOriginalValue)
    {
        System.Text.StringBuilder textBuilder = new System.Text.StringBuilder();

        foreach (var pair in statDictionary)
        {
            float originalValue = getOriginalValue(pair.Key);
            string line = $"{pair.Key}: {pair.Value:F2}";
            if (pair.Value != originalValue)
            {
                line = $"{redColorTagStart}{line}{colorTagEnd}";
            }
            textBuilder.AppendLine(line);
        }

        return textBuilder.ToString();
    }
}
