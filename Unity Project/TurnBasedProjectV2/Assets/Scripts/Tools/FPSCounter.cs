using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Small utility class used for optimisation and testing purposes only (not in the final build)
 * https://www.youtube.com/watch?v=RTfMWE-NDTE <- Script available here, I take no credit for this script.
 */
public class FPSCounter : MonoBehaviour
{
    public int frameCounter = 0;
    private float timeCounter = 0.0f;
    private float refreshTime = 0.1f;

    private float maxFramerate = 0f;
    private float minFramerate = 1000f;
    
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI minFPSText;
    public TextMeshProUGUI maxFPSText;

    private void Start()
    {
        StartCoroutine(ResetMinFramerate());
    }

    private IEnumerator ResetMinFramerate()
    {
        yield return new WaitForSeconds(1f);
        minFramerate = 1000f;
    }

    private void Update()
    {
        if (timeCounter < refreshTime)
        {
            timeCounter += Time.deltaTime;
            frameCounter++;
        }
        else
        {
            float lastFramerate = frameCounter / timeCounter;

            if (minFramerate > lastFramerate) minFramerate = lastFramerate;

            if (maxFramerate < lastFramerate) maxFramerate = lastFramerate;
            
            frameCounter = 0;
            timeCounter = 0.0f;
            fpsText.text = "FPS: "+lastFramerate.ToString("F0");
            minFPSText.text = "Min: "+minFramerate.ToString("F0");
            maxFPSText.text = "Max: "+maxFramerate.ToString("F0");
        }
    }
}
