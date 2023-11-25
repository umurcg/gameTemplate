
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "DeviceNameDictionary", menuName = "DeviceNameDictionary", order = 0)]
public class DeviceNameDictionary : ScriptableObject
{
    public DeviceMarketingName[] devices;

    public bool FindDeviceName(string device_name, out string marketing_name)
    {
        //Just get last part of device name
        device_name = device_name.Split(' ').Last();
        foreach (var device in devices)
        {
            if (device.deviceName == device_name)
            {
                marketing_name = device.marketingName;
                return true;
            }
        }
        
        marketing_name = "Unkown";
        return false;
    }

#if UNITY_EDITOR
    // Read CSV and import dictionary
    [ContextMenu("Import from CSV")]
    public void ImportCSV(TextAsset csvFile)
    {
        string[] lines = csvFile.text.Split('\n');
        List<DeviceMarketingName> deviceMarketingNames = new List<DeviceMarketingName>();

        for (int i = 1; i < lines.Length; i++) // Skipping the header row
        {
            string[] columns = ParseCSVRow(lines[i]);

            if (columns.Length >= 4)
            {
                DeviceMarketingName deviceMarketingName = new DeviceMarketingName
                {
                    deviceName = columns[3].Trim(),
                    marketingName = columns[1].Trim()
                };
                deviceMarketingNames.Add(deviceMarketingName);
            }
        }

        devices = deviceMarketingNames.ToArray();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    private string[] ParseCSVRow(string line)
    {
        // Matches comma-separated values in a CSV row, taking into account quoted values with commas
        Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        string[] columns = csvParser.Split(line);

        for (int i = 0; i < columns.Length; i++)
        {
            columns[i] = columns[i].Trim('"');
        }

        return columns;
    }
}
