


// using live2d;
// using live2d.framework;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;

// public class MygoLive2D : MonoBehaviour
// {
//     public string file;
//     public string File { set => file = value; }
//     public string output;
//     private Live2DModelUnity live2DModel;
//     private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();
//     //private L2DTargetPoint dragMgr = new L2DTargetPoint();
//     private L2DPhysics physics;
//     private Matrix4x4 live2DCanvasPos;

//     public List<MotionPair> motionPairs = new List<MotionPair>();
//     public MotionQueueManager motionMgr;

//     public List<ExpPair> expPairs = new List<ExpPair>();
//     private MygoExp curExp;
//     private MygoExp noneExp;

//     private int expIndex, motionIndex;

//     public Dictionary<string, float> defExpValue;
//     public bool enabledEyeClose;

//     private void Start()
//     {
//         Live2D.init();
//         defExpValue = new Dictionary<string, float>();
//     }

//     [ContextMenu("Load File")]
//     public void LoadFile()
//     {
//         var config = Live2dLoadUtils.LoadConfig(file);
//         LoadConfig(config);
//         Resources.UnloadUnusedAssets();
//     }

//     [ContextMenu("Load File2")]
//     public void LoadFile2()
//     {
//         var paths = SFB.StandaloneFileBrowser.OpenFilePanel("Ñ¡Ôñlive2dÎÄ¼þ", PlayerPrefs.GetString("live2d_path", "."), "json", false);
//         if (paths == null || paths.Length == 0)
//             return;

//         var path = paths[0];
//         if (string.IsNullOrEmpty(path))
//             return;

//         PlayerPrefs.SetString("live2d_path", Path.GetDirectoryName(path));

//         var config = Live2dLoadUtils.LoadConfig(path);
//         LoadConfig(config);
//         Resources.UnloadUnusedAssets();
//     }

//     public void LoadConfig(MygoConfig config)
//     {
//         live2DModel = Live2DModelUnity.loadModel(config.model);
//         for (int i = 0; i < config.textures.Count; i++)
//         {
//             var texture = config.textures[i];
//             live2DModel.setTexture(i, texture);
//         }

//         float modelWidth = live2DModel.getCanvasWidth();
//         live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

//         physics = L2DPhysics.load(config.physics);
//         motionMgr = new MotionQueueManager();
//         motionPairs.Clear();
//         foreach (var item in config.motions)
//         {
//             motionPairs.Add(new MotionPair()
//             {
//                 name = item.Key,
//                 motion = Live2DMotion.loadMotion(item.Value)
//             });
//         }
//         motionPairs.Sort((a, b) =>
//         {
//             return a.name.CompareTo(b.name);
//         });

//         expPairs.Clear();
//         foreach (var item in config.expressions)
//         {
//             MygoExp exp = new MygoExp()
//             {
//                 data = item.Value,
//             };
//             expPairs.Add(new ExpPair()
//             {
//                 name = item.Key,
//                 exp = exp
//             });
//         }
//         expPairs.Sort((a, b) =>
//         {
//             return a.name.CompareTo((b.name));
//         });

//         defExpValue.Clear();
//         noneExp = new MygoExp()
//         {
//             data = MygoExpJson.Create()
//         };
//         HashSet<string> isMult = new HashSet<string>();
//         foreach (var item in expPairs)
//         {
//             if (item.exp.data.keyDatas == null)
//                 continue;
//             foreach (var keyData in item.exp.data.keyDatas)
//             {
//                 var val = live2DModel.getParamFloat(keyData.id);
//                 defExpValue[keyData.id] = val;
//                 if (keyData.calc == "mult")
//                     isMult.Add(keyData.id);
//             }
//         }

//         foreach (var item in defExpValue)
//         {
//             Debug.Log($"key={item.Key}, value={item.Value:F2}");
//             //noneExp.AddExtraData(item.Key, item.Value, (isMult.Contains(item.Key) ? "mult":""));
//         }

//         expPairs.Insert(0, new ExpPair()
//         {
//             name = "none",
//             exp = noneExp
//         });

//         curExp = noneExp;
//         curExp.CacheValues(live2DModel);
//         expIndex = 0;
//         motionIndex = -1;

//         rollbackValues.Clear();
//         foreach (var item in defExpValue)
//         {
//             rollbackValues[item.Key] = live2DModel.getParamFloat(item.Key);
//         }

//         Live2dLoadUtils.PrintAllParamsAndRange(live2DModel);
//     }

//     private Vector2 motionSV, expSV;
//     private void OnGUI()
//     {
//         if (Application.isPlaying)
//         {
//             Color color = GUI.color;
//             GUILayout.BeginVertical("box", GUILayout.Height(Screen.height));
//             motionSV = GUILayout.BeginScrollView(motionSV, true, false, GUILayout.Width(550));
//             GUILayout.BeginHorizontal("box", GUILayout.Height(Screen.height * 0.5f));
//             int i = 0;
//             for (; i < motionPairs.Count; i++)
//             {
//                 if (i % 10 == 0)
//                 {
//                     GUILayout.BeginVertical("box");
//                 }
//                 var pair = motionPairs[i];
//                 if (i == motionIndex)
//                 {
//                     GUI.color = Color.green;
//                 }
//                 if (GUILayout.Button(pair.name))
//                 {
//                     motionIndex = i;
//                     motionMgr.startMotion(pair.motion);
//                 }
//                 GUI.color = color;
//                 if (i % 10 == 9)
//                 {
//                     GUILayout.EndVertical();
//                 }
//             }
//             if (i != 0 && i % 10 != 0)
//             {
//                 GUILayout.EndVertical();
//             }
//             GUILayout.EndHorizontal();
//             GUILayout.EndScrollView();


//             expSV = GUILayout.BeginScrollView(expSV, true, false, GUILayout.Width(550));
//             GUILayout.BeginHorizontal("box", GUILayout.Height(Screen.height * 0.5f));
//             i = 0;
//             for (; i < expPairs.Count; i++)
//             {
//                 if (i % 10 == 0)
//                 {
//                     GUILayout.BeginVertical("box");
//                 }
//                 var pair = expPairs[i];
//                 if (i == expIndex)
//                 {
//                     GUI.color = Color.green;
//                 }
//                 if (GUILayout.Button(pair.name))
//                 {
//                     expIndex = i;
//                     SwitchExp(pair.exp);
//                 }
//                 GUI.color = color;
//                 if (i % 10 == 9)
//                 {
//                     GUILayout.EndVertical();
//                 }
//             }
//             if (i != 0 && (i % 10 != 0))
//             {
//                 GUILayout.EndVertical();
//             }
//             GUILayout.EndHorizontal();
//             GUILayout.EndScrollView();


//             GUILayout.EndVertical();
//         }
//     }

//     public string GetOutputText()
//     {
//         List<string> list = new List<string>();
//         if (motionIndex >= 0)
//         {
//             list.Add($"-motion={motionPairs[motionIndex].name}");
//         }
//         if (expIndex > 0)
//         {
//             list.Add($"-expression={expPairs[expIndex].name}");
//         }
//         string output = string.Join(" ", list);
//         return output;
//     }

//     private HashSet<string> newKeys = new HashSet<string>();
//     public void SwitchExp(MygoExp newExp)
//     {
//         if (curExp == newExp)
//             return;

//         newKeys.Clear();
//         if (newExp != null)
//         {
//             if (newExp.data.keyDatas != null)
//             {
//                 foreach (var item in newExp.data.keyDatas)
//                 {
//                     newKeys.Add(item.id);
//                 }
//             }
//             newExp.extraDatas.Clear();
//         }

//         if (newExp != noneExp && curExp.data.keyDatas != null)
//         {
//             foreach (var keyData in curExp.data.keyDatas)
//             {
//                 if (!newKeys.Contains(keyData.id))
//                 {
//                     Debug.Log("not has " + keyData.id);
//                     if (defExpValue.TryGetValue(keyData.id, out var value))
//                     {
//                         newExp.AddExtraData(keyData.id, value);
//                     }
//                 }
//             }
//         }

//         var oldExp = curExp;
//         curExp = newExp;
//         if (curExp != null)
//         {
//             curExp.Reset();
//             curExp.CacheValues(live2DModel, oldExp);
//         }

//         rollbackValues.Clear();
//         foreach (var item in defExpValue)
//         {
//             rollbackValues[item.Key] = live2DModel.getParamFloat(item.Key);
//         }
//     }

//     private Dictionary<string, float> rollbackValues = new Dictionary<string, float>();
//     void Update()
//     {
//         if (live2DModel == null)
//             return;

//         live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);
//         if (!Application.isPlaying)
//         {
//             live2DModel.update();
//             return;
//         }
        

//         //var pos = Input.mousePosition;
//         //if (Input.GetMouseButtonDown(0))
//         //{
//         //    //
//         //}
//         //else if (Input.GetMouseButton(0))
//         //{
//         //    dragMgr.Set(pos.x / Screen.width * 2 - 1, pos.y / Screen.height * 2 - 1);
//         //}
//         //else if (Input.GetMouseButtonUp(0))
//         //{
//         //    dragMgr.Set(0, 0);
//         //}


//         //dragMgr.update();
//         //live2DModel.setParamFloat("PARAM_ANGLE_X", dragMgr.getX() * 30);
//         //live2DModel.setParamFloat("PARAM_ANGLE_Y", dragMgr.getY() * 30);

//         //live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", dragMgr.getX() * 10);

//         //live2DModel.setParamFloat("PARAM_EYE_BALL_X", -dragMgr.getX());
//         //live2DModel.setParamFloat("PARAM_EYE_BALL_Y", -dragMgr.getY());

//         double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
//         double t = timeSec * 2 * Math.PI;
//         live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

//         if (physics != null)
//             physics.updateParam(live2DModel);

//         motionMgr.updateParam(live2DModel);

//         if (curExp != null)
//         {
//             if (!motionMgr.isFinished())
//             {
//                 rollbackValues.Clear();
//                 foreach (var item in defExpValue)
//                 {
//                     rollbackValues[item.Key] = live2DModel.getParamFloat(item.Key);
//                 }
//             }
//             if (enabledEyeClose)
//                 eyeBlink.setParam(live2DModel);

//             curExp.Update(Time.deltaTime);
//             curExp.Apply(live2DModel);
//         }
//         else
//         {
//             if (enabledEyeClose)
//                 eyeBlink.setParam(live2DModel);
//         }

//         live2DModel.update();

//         if (rollbackValues.Count > 0)
//         {
//             foreach (var item in defExpValue)
//             {
//                 live2DModel.setParamFloat(item.Key, rollbackValues[item.Key]);
//             }
//         }
//     }


//     void OnRenderObject()
//     {
//         if (live2DModel == null)
//             return;
//         if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW)
//             live2DModel.draw();
//     }
// }

