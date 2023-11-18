using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisionCone))]
public class VisionConeEditor : Editor
{
    private void OnSceneGUI()
    {
        VisionCone cone = (VisionCone)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(cone.transform.position, Vector3.back, Vector3.right, 360, cone.ViewRadius);

        Vector3 viewAngleStart = cone.GetDirectionFromAngle(-cone.ViewAngle / 2, false);
        Vector3 viewAngleEnd = cone.GetDirectionFromAngle(cone.ViewAngle / 2, false);

        Handles.DrawLine(cone.transform.position, cone.transform.position + viewAngleStart * cone.ViewRadius);
        Handles.DrawLine(cone.transform.position, cone.transform.position + viewAngleEnd * cone.ViewRadius);

        Handles.color = Color.white;
        for (int i = 0; i < cone.VisibleTargets.Count; i++)
        {
            Handles.DrawLine(cone.transform.position, cone.VisibleTargets[i].position, 1f);
        }

        Handles.color = Color.red;
        float stepAngle = cone.ViewAngle / cone.CountOfRays;
        for (int i = 1; i < cone.CountOfRays; i++)
        {
            float angle = cone.transform.eulerAngles.z - cone.ViewAngle / 2 + stepAngle * i;
            var direction = cone.GetDirectionFromAngle(angle, true);
            Handles.DrawLine(cone.transform.position, cone.transform.position + cone.GetDirectionFromAngle(angle, true) * cone.ViewRadius);
        }
    }
}
