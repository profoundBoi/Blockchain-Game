using TMPro;
using UnityEngine;

public class GateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private Gate gate;
    [SerializeField] private KeyInventory playerInventory;

    private void Update()
    {
        if (gate == null || playerInventory == null)
            return;

        int collected = gate.GetCollectedKeyCount(playerInventory);
        int required = gate.RequiredKeyCount;

        keyText.text = collected + " / " + required + " KEYS";
    }
}