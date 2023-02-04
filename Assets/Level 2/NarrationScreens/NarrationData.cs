using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class NarrationData {
    public string text;
    public float duration;
}

[System.Serializable]
public class NarrationSequence {
    public Sprite image;
    public Color imageColorMultiplier;
    public Color textColor;
    public NarrationData[] narrationDatas;
    public UnityEvent onFinished;
}