
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "GeekbenchDeviceList", menuName = "GeekbenchDeviceList", order = 0)]
public class GeekbenchDeviceList : ScriptableObject
{
    public GeekbenchData[] devices;
    public DeviceNameDictionary deviceNameDictionary;
    public TextAsset testDevices;

#if UNITY_EDITOR
    [ContextMenu("Test")]
#endif
    public void Test()
    {
        string[] device_names = testDevices.text.Split('\n');
        foreach (string deviceName in device_names)
        {
            var benchData = GetBenchDataWithName(deviceName);
            if (benchData != null)
            {
                deviceNameDictionary.FindDeviceName(deviceName, out string marketingName);
                Debug.Log("Device: " + deviceName + " Marketing name: " + marketingName + "Bench Name :" +
                          benchData.name + " Score: " + benchData.score);
            }
        }
    }

    public GeekbenchData GetBenchDataOfDevice()
    {
        string deviceModel = SystemInfo.deviceModel;
        return GetBenchDataWithName(deviceModel);
    }

    public GeekbenchData GetBenchDataWithName(string deviceName)
    {
        if (deviceNameDictionary.FindDeviceName(deviceName, out string marketingName))
        {
            return GetBenchDataWithMarketingName(marketingName);
        }

        Debug.LogError("Device not found in marketing dictionary: " + deviceName);
        return null;
    }

    public GeekbenchData GetBenchDataWithMarketingName(string marketingName)
    {
        var inputWithoutSpaces = Regex.Replace(marketingName, @"\s+", "");

        foreach (var device in devices)
        {
            var deviceName = device.name;

            //Trim out first word (Apple, Samsung, etc) if it exists
            var firstSpace = deviceName.IndexOf(' ');
            if (firstSpace != -1)
            {
                deviceName = deviceName.Substring(firstSpace + 1);
            }

            var nameWithoutSpaces = Regex.Replace(deviceName, @"\s+", "");
            var result = nameWithoutSpaces.CompareTo(inputWithoutSpaces);
            if (result == 0)
            {
                return device;
            }
        }

        Debug.LogError("Device not found: " + marketingName + "Input: " + inputWithoutSpaces);
        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("ImportJson")]
#endif
    public void ImportJson(TextAsset json)
    {
        string jsonText = json.text;
        devices = JsonUtility.FromJson<GeekbenchDeviceListWrapper>(jsonText).devices;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
