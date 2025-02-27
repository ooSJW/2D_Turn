using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class BlinkImage : MonoBehaviour // Data Field
{
    [SerializeField] private Image[] blinkImageArray;

    public float minAlpha;
    public float maxAlpha;
    public float basicAlpha;

    public float timer;
    public float intervalTime;

    private int blinkNumber = 0;

    private bool isBlink = false;
    public bool IsBlink
    {
        get => isBlink;
        private set
        {
            if (isBlink != value)
                isBlink = value;
            if (!isBlink)
            {
                for (int i = 0; i < blinkImageArray.Length; i++)
                {
                    Color color = blinkImageArray[i].color;
                    color.a = basicAlpha;
                    blinkImageArray[i].color = color;
                }
            }
        }
    }
}
public partial class BlinkImage : MonoBehaviour // Initlaize
{
    public void Initialize()
    {
        IsBlink = false;
        timer = 0;
    }
    public void Initialize(bool isBlinkValue)
    {
        IsBlink = isBlinkValue;
        timer = 0;
    }

    private float Blink()
    {
        if (blinkNumber >= int.MaxValue)
            blinkNumber = 0;

        blinkNumber++;
        if (blinkNumber % 2 == 0)
            return minAlpha;
        else
            return maxAlpha;
    }
}
public partial class BlinkImage : MonoBehaviour // Main
{
    void Update()
    {
        if (IsBlink)
        {
            timer += Time.deltaTime;
            if (timer > intervalTime)
            {
                float alpha = Blink();
                for (int i = 0; i < blinkImageArray.Length; i++)
                {
                    Color color = blinkImageArray[i].color;
                    color.a = alpha;
                    blinkImageArray[i].color = color;
                }
                timer = 0;
            }
        }
    }
}
