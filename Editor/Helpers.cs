using UnityEngine;

namespace Dropecho {
  public static class Helpers {
    public static Vector3 DirectionFromAngle(float angle, float initial = 0) {
      var dirAngle = angle + initial;
      return new Vector3(Mathf.Sin(dirAngle * Mathf.Deg2Rad), 0, Mathf.Cos(dirAngle * Mathf.Deg2Rad));
    }
  }
}