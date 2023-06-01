using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasBehaviour : Singleton<CanvasBehaviour>
{
    public GameObject quantityOfPinsText;
    public GameObject readyToShootText;
    public GameObject strikeText;
    public GameObject almostStrikeText;

    public void updateCurrentPins(int quantityOfPinsDown, int remainingAttempts)
    {
        quantityOfPinsText.GetComponent<TextMeshProUGUI>().text = "Quantity of Pins down:  " + quantityOfPinsDown + "\nRemaining Attempts: "+remainingAttempts;
    }

    public void shootNow()
    {
        readyToShootText.GetComponent<TextMeshProUGUI>().text = "Shoot Now";
    }

    public void holdOn()
    {
        readyToShootText.GetComponent<TextMeshProUGUI>().text = "Hold on";
    }

    public void performStrike()
    {
        strikeText.GetComponent<Animator>().Play("TremendoPlenoAnimation");
    }

    public void performAlmostStrike()
    {
        almostStrikeText.GetComponent<Animator>().Play("semiPlenoAnimation");
    }
}
