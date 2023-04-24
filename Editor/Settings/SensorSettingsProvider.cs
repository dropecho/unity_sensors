using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dropecho {
  static class SensorSettingsProvider {
    [SettingsProvider]
    public static SettingsProvider CreateSensorSettingsProvider() {
      var provider = new SettingsProvider("Project/Dropecho/SensorSettings", SettingsScope.Project, new[] { "Sensor" }) {
        label = "Sensor Settings",

        // activateHandler is called when the user clicks on the Settings item in the Settings window.
        activateHandler = (searchContext, rootElement) => {
          BuildGUI(rootElement);
        }
      };

      return provider;
    }

    static void BuildGUI(VisualElement rootElement) {
      var settings = SensorSettings.GetOrCreateSettings();
      var editor = Editor.CreateEditor(settings);
      rootElement.AddChildren(new IMGUIContainer(() => editor.DrawDefaultInspector()));
    }
  }
}
