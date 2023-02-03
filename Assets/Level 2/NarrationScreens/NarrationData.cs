using UnityEngine;

[System.Serializable]
public class NarrationData {
    public string text;
    public float duration;
}

[System.Serializable]
public class NarrationSequence {
    public Sprite image;
    public Color imageColorMultiplier;
    public NarrationData[] narrationDatas;
}