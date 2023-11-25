using System;


[Serializable]
public class GeekbenchData
{
    public string name;
    public string description;
    public string family;
    public int score;
}

[Serializable]
public class GeekbenchDeviceListWrapper
{
    public GeekbenchData[] devices;
}