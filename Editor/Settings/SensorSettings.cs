using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dropecho {
  // Create a new type of Settings Asset.
  class SensorSettings : ScriptableObject {
    static SensorSettings _instance;
    public static SensorSettings Instance {
      get {
        if (_instance == null) {
          _instance = GetOrCreateSettings();
        }
        return _instance;
      }
    }

    public Color SensorNoDetectionsColor;
    public Color SensorDetectionsColor;
    public Color LineToDetectedObjectsColor;
    [Range(1, 64)]
    public int LineToDetectedObjectsThickness = 1;

    public static readonly string SettingsPath = "Assets/Settings/SensorSettings.asset";

    internal static SensorSettings GetSettings() {
      return AssetDatabase.LoadAssetAtPath<SensorSettings>(SettingsPath);
    }

    internal static SensorSettings GetOrCreateSettings() {
      var settings = AssetDatabase.LoadAssetAtPath<SensorSettings>(SettingsPath);
      return settings == null ? CreateSettings() : settings;
    }

    private static SensorSettings CreateSettings() {
      if (!AssetDatabase.IsValidFolder("Assets/Settings")) {
        AssetDatabase.CreateFolder("Assets", "Settings");
      }
      var settings = ScriptableObject.CreateInstance<SensorSettings>();
      AssetDatabase.CreateAsset(settings, SettingsPath);
      AssetDatabase.SaveAssets();
      return settings;
    }

    internal static SerializedObject GetSerializedSettings() {
      return new SerializedObject(GetOrCreateSettings());
    }
  }
}
