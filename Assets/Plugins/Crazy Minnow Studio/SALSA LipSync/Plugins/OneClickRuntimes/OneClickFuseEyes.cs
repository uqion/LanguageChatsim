using UnityEngine;
using System.Text.RegularExpressions;

namespace CrazyMinnow.SALSA.OneClicks
{
	public class OneClickFuseEyes : MonoBehaviour
	{
		public static void Setup(GameObject go)
		{
			string body = "body";
			string eyelash = "eyelash";
			string head = "head";
			string eyeL = "lefteye";
			string eyeR = "righteye";
			string blinkL = "blink_left";
			string blinkR = "blink_right";

			if (go)
			{
				Eyes eyes = go.GetComponent<Eyes>();
				if (eyes == null)
				{
					eyes = go.AddComponent<Eyes>();
				}
				else
				{
					DestroyImmediate(eyes);
					eyes = go.AddComponent<Eyes>();
				}
				QueueProcessor qp = go.GetComponent<QueueProcessor>();
				if (qp == null) qp = go.AddComponent<QueueProcessor>();

				// System Properties
                eyes.characterRoot = go.transform;
                eyes.queueProcessor = qp;

                // Heads - Bone_Rotation
                eyes.BuildHeadTemplate(Eyes.HeadTemplates.Bone_Rotation_XY);
                eyes.heads[0].expData.controllerVars[0].bone = Eyes.FindTransform(eyes.characterRoot, head);
                eyes.headTargetOffset.y = 0.052f;
				eyes.FixAllTransformAxes(ref eyes.heads, false);
				eyes.FixAllTransformAxes(ref eyes.heads, true);

                // Eyes - Bone_Rotation
                eyes.BuildEyeTemplate(Eyes.EyeTemplates.Bone_Rotation);
                eyes.eyes[0].expData.controllerVars[0].bone = Eyes.FindTransform(eyes.characterRoot, eyeL);
                eyes.eyes[1].expData.controllerVars[0].bone = Eyes.FindTransform(eyes.characterRoot, eyeR);
				eyes.FixAllTransformAxes(ref eyes.eyes, false);
				eyes.FixAllTransformAxes(ref eyes.eyes, true);

                // Eyelids - Bone_Rotation
                eyes.BuildEyelidTemplate(Eyes.EyelidTemplates.BlendShapes); // includes left/right eyelid
                eyes.AddEyelidShapeExpression(); // add eyelash left
                eyes.AddEyelidShapeExpression(); // add eyelash right
                eyes.SetEyelidShapeSelection(Eyes.EyelidSelection.Upper);
                float blinkMax = 0.75f;
                // Left eyelid
                eyes.eyelids[0].referenceIdx = 0;
                eyes.eyelids[0].expData.controllerVars[0].smr = Eyes.FindTransform(eyes.characterRoot, body).GetComponent<SkinnedMeshRenderer>();
				eyes.eyelids[0].expData.controllerVars[0].blendIndex = Eyes.FindBlendIndex(eyes.eyelids[0].expData.controllerVars[0].smr, blinkL);
                eyes.eyelids[0].expData.controllerVars[0].maxShape = blinkMax;
                // Right eyelid
                eyes.eyelids[1].referenceIdx = 1;
                eyes.eyelids[1].expData.controllerVars[0].smr = eyes.eyelids[0].expData.controllerVars[0].smr;
                eyes.eyelids[1].expData.controllerVars[0].blendIndex = Eyes.FindBlendIndex(eyes.eyelids[1].expData.controllerVars[0].smr, blinkR);
                eyes.eyelids[1].expData.controllerVars[0].maxShape = blinkMax;
                // Left eyelash
                eyes.eyelids[2].referenceIdx = 0;
                eyes.eyelids[2].expData.controllerVars[0].smr = Eyes.FindTransform(eyes.characterRoot, eyelash).GetComponent<SkinnedMeshRenderer>();
                eyes.eyelids[2].expData.controllerVars[0].blendIndex = Eyes.FindBlendIndex(eyes.eyelids[2].expData.controllerVars[0].smr, blinkL);
                eyes.eyelids[2].expData.controllerVars[0].maxShape = blinkMax;
                // Right eyelash
                eyes.eyelids[3].referenceIdx = 1;
                eyes.eyelids[3].expData.controllerVars[0].smr = eyes.eyelids[2].expData.controllerVars[0].smr;
                eyes.eyelids[3].expData.controllerVars[0].blendIndex = Eyes.FindBlendIndex(eyes.eyelids[3].expData.controllerVars[0].smr, blinkR);
                eyes.eyelids[3].expData.controllerVars[0].maxShape = blinkMax;

                // Initialize the Eyes module
                eyes.Initialize();
			}
		}
	}
}