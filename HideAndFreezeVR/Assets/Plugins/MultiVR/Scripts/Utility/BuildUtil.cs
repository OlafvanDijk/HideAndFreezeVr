#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BuildUtil : MonoBehaviour
{
    // Android specific

    [MenuItem("Build/Android/Push Existing Build To Android Device")]
    public static void PushToAndroid()
    {
        string apkLocation = PlayerPrefs.GetString("APK location");

        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(apkLocation))
            apkLocation = EditorUtility.OpenFilePanel("Find APK", Environment.CurrentDirectory, "apk");

        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(apkLocation))
        {
            Debug.LogError("Cannot find .apk file.");
            return;
        }

        PlayerPrefs.SetString("APK location", apkLocation);

        string adbLocation = PlayerPrefs.GetString("Android debug bridge location");

        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(adbLocation))
            adbLocation = EditorUtility.OpenFilePanel("Android debug bridge", Environment.CurrentDirectory, "exe");

        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(adbLocation))
        {
            Debug.LogError("Cannot find adb.exe.");
            return;
        }

        PlayerPrefs.SetString("Android debug bridge location", adbLocation);

        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = adbLocation,
            Arguments = string.Format("install -r \"{0}\"", apkLocation),
            WorkingDirectory = Path.GetDirectoryName(adbLocation),
            RedirectStandardOutput = true,
        };

        Process.Start(info);
    }
}

#endif