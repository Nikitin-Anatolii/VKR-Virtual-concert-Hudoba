using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioAnalysis
{
    public int bpm { get; set; }
    public List<Frame> frames { get; set; }
} 