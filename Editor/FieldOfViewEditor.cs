using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Dropecho.AI {
  [CustomEditor(typeof(FieldOfViewSensor)), CanEditMultipleObjects]
  public class FieldOfViewSensorEditor : Editor {
    static bool DrawDebug2d = false;

    public override VisualElement CreateInspectorGUI() {
      var root = new VisualElement();
      InspectorElement.FillDefaultInspector(root, serializedObject, this);

      var draw2dButton = new Button() { text = "Toggle Draw2D: " + (DrawDebug2d ? "On" : "Off") };
      draw2dButton.clicked += () => {
        DrawDebug2d = !DrawDebug2d;
        draw2dButton.text = "Toggle Draw2D: " + (DrawDebug2d ? "On" : "Off");
        SceneView.RepaintAll();
      };

      var header = new VisualElement();
      header.style.borderTopColor = Color.gray;
      header.style.borderTopWidth = 2;
      header.style.marginTop = 2;
      header.style.paddingTop = 2;
      root.Add(header);
      root.Add(draw2dButton);
      return root;
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Active, typeof(FieldOfViewSensor))]
    static void DrawGizmoForFieldOfView(FieldOfViewSensor sensor, GizmoType gizmoType) {
      var transform = sensor.transform;
      var headPos = transform.position;
      // if (sensor.TryGetComponent<Animator>(out var animator) && animator.isHuman) {
      //   headPos = animator.GetBoneTransform(HumanBodyBones.Head).position;
      // }

      Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
      Handles.color = sensor.sensedObjects.Count > 0 ?
          SensorSettings.Instance.SensorDetectionsColor :
          SensorSettings.Instance.SensorNoDetectionsColor;

      if (DrawDebug2d) {
        Draw2dDebug(sensor, headPos);
      } else {
        Draw3dDebug(sensor, headPos);
      }
      Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

      Handles.color = SensorSettings.Instance.LineToDetectedObjectsColor;
      var thickness = SensorSettings.Instance.LineToDetectedObjectsThickness;

      foreach (var obj in sensor.sensedObjects) {
        var planarEndPos = new Vector3(obj.transform.position.x, headPos.y, obj.transform.position.z);
        Handles.DrawAAPolyLine(thickness, headPos, planarEndPos);
        Handles.DrawAAPolyLine(thickness, planarEndPos, obj.transform.position);
      }
    }

    private static void Draw2dDebug(FieldOfViewSensor sensor, Vector3 headPos) {
      var transform = sensor.transform;
      var start = Quaternion.AngleAxis(-sensor.angle / 2, transform.up) * transform.forward;
      var end = Quaternion.AngleAxis(sensor.angle / 2, transform.up) * transform.forward;

      if (sensor.angle < 360 && sensor.angle > 0) {
        Handles.DrawAAPolyLine(headPos, headPos + start * sensor.range);
        Handles.DrawAAPolyLine(headPos, headPos + end * sensor.range);
      }

      Handles.DrawSolidArc(headPos, transform.up, start, sensor.angle, sensor.range);
      Handles.DrawWireArc(headPos, transform.up, start, sensor.angle, sensor.range);
    }

    private static void Draw3dDebug(FieldOfViewSensor sensor, Vector3 headPos) {
      var transform = sensor.transform;
      for (var i = 0; i < 360; i++) {
        var ang = i;
        var up = Quaternion.AngleAxis(ang, transform.forward) * transform.up;
        var start = Quaternion.AngleAxis(-sensor.angle / 2, up) * transform.forward;
        var end = Quaternion.AngleAxis(sensor.angle / 2, up) * transform.forward;

        if (sensor.angle < 360 && sensor.angle > 0) {
          Handles.DrawAAPolyLine(headPos, headPos + start * sensor.range);
          Handles.DrawAAPolyLine(headPos, headPos + end * sensor.range);
        }

        Handles.DrawWireArc(headPos, up, start, sensor.angle, sensor.range);
      }
    }
  }
}
