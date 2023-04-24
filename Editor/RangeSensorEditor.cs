using UnityEngine;
using UnityEditor;

namespace Dropecho.AI {
  [CustomEditor(typeof(RangeSensor)), CanEditMultipleObjects]
  public class RangeSensorEditor : Editor {

    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoForRangeSensor(RangeSensor sensor, GizmoType gizmoType) {
      if (!sensor) {
        return;
      }
      var settings = SensorSettings.Instance;
      var transform = sensor.gameObject.transform;
      var position = transform.position;

      Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
      var color = sensor.sensedObjects.Count > 0 ?
        settings.SensorDetectionsColor :
        settings.SensorNoDetectionsColor;

      switch (sensor.shape) {
        case SensorShape.Capsule:
          using (new Handles.DrawingScope(color, Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale))) {
            HandlesExtras.DrawWireCapsule(sensor.origin, sensor.offset, sensor.radius);
          }
          break;
        case SensorShape.Sphere:
          using (new Handles.DrawingScope(color)) {
            Handles.DrawSolidDisc(transform.position, transform.up, sensor.radius);
          }
          break;
        case SensorShape.Cube:
          color.a = Mathf.Max(color.a, 0.5f);
          using (new Handles.DrawingScope(color, Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale))) {
            Handles.DrawWireCube(Vector3.zero, sensor.size);
          }
          break;
      }

      Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
      Handles.color = settings.LineToDetectedObjectsColor;
      var thickness = settings.LineToDetectedObjectsThickness;

      foreach (var obj in sensor.sensedObjects) {
        var planarEndPos = new Vector3(obj.transform.position.x, position.y, obj.transform.position.z);
        Handles.DrawAAPolyLine(thickness, position, planarEndPos);
        Handles.DrawAAPolyLine(thickness, planarEndPos, obj.transform.position);
      }
    }
  }
}
