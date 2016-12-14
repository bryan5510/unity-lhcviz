using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {

	private Vector3[] points;

	public void SetAllControlPoints(Vector3 p1,Vector3 p2,Vector3 p3,Vector3 p4){
		points = new Vector3[4] {p1,p2,p3,p4};
	}

	public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * (points.Length - 1) / 3;
			i = (int)t;
			t -= i;
			i *= 3;
		}


		t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		Vector3 v = OneMinusT * OneMinusT * OneMinusT * points[i] + 3f * OneMinusT * OneMinusT * t * points[i + 1] + 3f * OneMinusT * t * t * points[i + 2] + t * t * t * points[i + 3];

		return transform.TransformPoint(v);
	}

	public void MakeSpline(int frequency, Transform[] items){
		if (frequency > 0 && items != null && items.Length != 0) {
			float stepSize = frequency * items.Length;
			if (stepSize == 1) {
				stepSize = 1f / stepSize;
			} else {
				stepSize = 1f / (stepSize - 1);
			}
			for (int p = 0, f = 0; f < frequency; f++) {
				for (int i = 0; i < items.Length; i++, p++) {
					Transform item = Instantiate (items [i]) as Transform;
					Vector3 position = GetPoint (p * stepSize);
					item.transform.localPosition = position;
					item.transform.parent = transform;
				}
			}
		}
	}

}