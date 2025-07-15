

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Live2dMotionData
{
    public int m_state_curFrameIndex = 0;

    public class Track
    {
        public string name;
        public Dictionary<int, TrackKeyFrameData> keyFrames = new Dictionary<int, TrackKeyFrameData>();
        public bool HasKeyFrame(int frame)
        {
            return keyFrames.ContainsKey(frame);
        }

        public TrackKeyFrameData GetKeyFrame(int frame)
        {
            return keyFrames.TryGetValue(frame, out var data) ? data : null;
        }

        public void SetNormalKeyFrame(int frame, float value)
        {
            keyFrames[frame] = new TrackKeyFrameData()
            {
                frame = frame,
                value = value,
            };
        }

        public void SetBezierKeyFrame(int frame, float value, Vector2 controlPoint1, Vector2 controlPoint2)
        {
            keyFrames[frame] = new TrackKeyFrameData()
            {
                frame = frame,
                value = value,
                controlPoint1 = controlPoint1,
                controlPoint2 = controlPoint2,
            };
        }

        public void SetKeyFrameValue(int frame, float value)
        {
            if (keyFrames.TryGetValue(frame, out var data))
            {
                data.value = value;
            }
            else
            {
                SetNormalKeyFrame(frame, value);
            }
        }

        public void SetKeyFrameData(int frame, TrackKeyFrameData data, bool doClone = false)
        {
            if (doClone)
            {
                var data2 = data.Clone();
                data2.frame = frame;
                keyFrames[frame] = data2;
            }
            else
            {
                data.frame = frame;
                keyFrames[frame] = data;
            }
        }
    }

    public class TrackKeyFrameData
    {
        public int frame;
        public float value;
        public Vector2 controlPoint1 = Vector2.zero;
        public Vector2 controlPoint2 = Vector2.one;

        public TrackKeyFrameData()
        {

        }

        public TrackKeyFrameData(bool isLinear)
        {
            if (!isLinear)
            {
                controlPoint1 = new Vector2(0.5f, 0f);
                controlPoint2 = new Vector2(0.5f, 1f);
            }
        }

        public TrackKeyFrameData Clone()
        {
            var obj = MemberwiseClone() as TrackKeyFrameData;
            return obj;
        }
    }

    public string motionDataName = "未命名";
    public Live2dMotionInfo info;
    public Dictionary<string, Track> tracks = new Dictionary<string, Track>();

    public Track TryGetTrack(string name, bool createIfNotExists = true)
    {
        if (tracks.TryGetValue(name, out var track))
        {
            return track;
        }
        if (createIfNotExists)
        {
            track = new Track();
            track.name = name;
            tracks[name] = track;
        }
        return track;
    }

    public void Build(Live2dMotionInfo info)
    {
        this.info = info;
        tracks.Clear();
        foreach (var param in info.keyFrames)
        {
            var trackName = param.Key;
            var track = TryGetTrack(trackName, true);
            for (int i = 0; i < param.Value.Count; i++)
            {
                float value = param.Value[i];
                track.keyFrames[i] = new TrackKeyFrameData()
                {
                    frame = i,
                    value = value,
                };
            }
        }
    }

    public void BakeAllFrames()
    {
        info.keyFrames.Clear();
        foreach (var track in tracks.Values)
        {
            BakeFrames(track.name);
        }
    }

    private bool IsSimilar(float a, float b, float threshold = 0.001f)
    {
        return Mathf.Abs(a - b) < threshold;
    }

    public void UnBakeAllFramesByTrend()
    {
        foreach (var track in tracks.Values)
        {
            var frames = info.GetKeyFrames(track.name);
            if (frames == null || frames.Count == 0)
            {
                continue;
            }
            track.keyFrames.Clear();
            
            // 记录第一帧
            if (frames.Count > 0)
            {
                track.keyFrames[0] = new TrackKeyFrameData(false)
                {
                    frame = 0,
                    value = frames[0],
                };
            }
            
            int trend = 2; // 初始趋势设为增加
            float prevDiff = 0;
            
            // 从第二帧开始检查趋势变化
            for (int i = 1; i < frames.Count; i++)
            {
                float diff = frames[i] - frames[i-1];
                
                // 第一次计算差值时初始化趋势
                if (i == 1)
                {
                    trend = diff > 0 ? 2 : (diff < 0 ? 0 : 1);
                    prevDiff = diff;
                    continue;
                }
                
                // 计算当前趋势
                int curTrend = diff > 0 ? 2 : (diff < 0 ? 0 : 1);
                if (IsSimilar(diff, 0))
                {
                    curTrend = 1;
                }
                
                // 检查趋势是否改变
                if (curTrend != trend)
                {
                    // 趋势改变,记录关键帧
                    track.keyFrames[i-1] = new TrackKeyFrameData(false)
                    {
                        frame = i-1,
                        value = frames[i-1],
                    };
                    trend = curTrend; // 更新趋势
                }
                
                prevDiff = diff;
            }

            // // 删除相邻的相似帧（容差0.01）
            // var keyFrames = track.keyFrames.OrderBy(kv => kv.Key).ToList();
            // for (int i = keyFrames.Count - 1; i > 0; i--)
            // {
            //     float curValue = keyFrames[i].Value;
            //     float prevValue = keyFrames[i - 1].Value;
            //     if (IsSimilar(curValue, prevValue, 0.01f))
            //     {
            //         track.keyFrames.Remove(keyFrames[i].Key);
            //     }
            // }
            
            // 记录最后一帧
            if (frames.Count > 0)
            {
                track.keyFrames[frames.Count - 1] = new TrackKeyFrameData(false)
                {
                    frame = frames.Count - 1,
                    value = frames[frames.Count - 1],
                };
            }
        }

        BakeAllFrames();
    }

    public void BakeFrames(string trackName)
    {
        var track = TryGetTrack(trackName, true);
        // 使用线性过渡处理关键帧
        var keyFrames = track.keyFrames.OrderBy(kv => kv.Key).ToList();
        var frames = new List<float>();

        for (int i = 0; i < keyFrames.Count; i++)
        {
            frames.Add(keyFrames[i].Value.value);

            // 如果不是最后一个关键帧，添加线性过渡帧
            if (i < keyFrames.Count - 1)
            {
                var data = keyFrames[i].Value;
                var nextData = keyFrames[i + 1].Value;

                int currentFrame = data.frame;
                int nextFrame = nextData.frame;
                float currentValue = data.value;
                float nextValue = nextData.value;

                bool isBezier = data.controlPoint1 != Vector2.zero || data.controlPoint2 != Vector2.one;
                // 在两个关键帧之间添加线性过渡
                for (int frame = currentFrame + 1; frame < nextFrame; frame++)
                {
                    float t = (float)(frame - currentFrame) / (nextFrame - currentFrame);
                    if (isBezier)
                    {
                        float t2 = L2DWUtils.GetBezierYValueAtX(Vector2.zero, data.controlPoint1, data.controlPoint2, Vector2.one, t);
                        t = t2;
                    }
                    float interpolatedValue = Mathf.Lerp(currentValue, nextValue, t);
                    frames.Add(interpolatedValue);
                }
            }
        }

        var frameCount = info.frameCount;
        if (frames.Count > frameCount)
        {
            frames.RemoveRange(frameCount, frames.Count - frameCount);
        }

        info.keyFrames[trackName] = frames;
    }

    public string Save()
    {
        var jsonobject = new JSONObject(JSONObject.Type.OBJECT);
        jsonobject.AddField("name", motionDataName);
        jsonobject.AddField("fps", info.fps);
        jsonobject.AddField("fadein", info.fadein);
        jsonobject.AddField("fadeout", info.fadeout);
        jsonobject.AddField("frameCount", info.frameCount);
        var keyFramesObject = new JSONObject(JSONObject.Type.OBJECT);
        foreach (KeyValuePair<string, Track> track in tracks)
        {
            var trackKeyFrames = track.Value.keyFrames;
            if (trackKeyFrames.Count == 0)
            {
                continue;
            }
            var trackObject = new JSONObject(JSONObject.Type.OBJECT);
            trackObject.AddField("name", track.Key);
            var trackArray = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var keyFrame in trackKeyFrames)
            {
                var keyFrameObject = new JSONObject(JSONObject.Type.OBJECT);
                keyFrameObject.AddField("frame", keyFrame.Key);
                keyFrameObject.AddField("value", keyFrame.Value.value);
                if (keyFrame.Value.controlPoint1 != Vector2.zero)
                {
                    keyFrameObject.AddField("controlPoint1_x", keyFrame.Value.controlPoint1.x);
                    keyFrameObject.AddField("controlPoint1_y", keyFrame.Value.controlPoint1.y);
                }
                if (keyFrame.Value.controlPoint2 != Vector2.one)
                {
                    keyFrameObject.AddField("controlPoint2_x", keyFrame.Value.controlPoint2.x);
                    keyFrameObject.AddField("controlPoint2_y", keyFrame.Value.controlPoint2.y);
                }
                trackArray.Add(keyFrameObject);
            }
            trackObject.AddField("keyFrames", trackArray);

            keyFramesObject.AddField(track.Key, trackObject);
        }
        jsonobject.AddField("keyFrames", keyFramesObject);
        return jsonobject.print(true);
    }

    public void Load(string text)
    {
        var jsonobject = new JSONObject(text);
        info = new Live2dMotionInfo();
        tracks.Clear();
        motionDataName = jsonobject.GetField("name").str;
        info.fps = (int)jsonobject.GetField("fps").f;
        info.fadein = (int)jsonobject.GetField("fadein").f;
        info.fadeout = (int)jsonobject.GetField("fadeout").f;
        info.frameCount = (int)jsonobject.GetField("frameCount").f;
        var keyFramesObject = jsonobject.GetField("keyFrames");
        foreach (var trackKey in keyFramesObject.keys)
        {
            var trackObject = keyFramesObject.GetField(trackKey);
            var trackName = trackObject.GetField("name").str;
            var trackArray = trackObject.GetField("keyFrames").list;
            var track = TryGetTrack(trackName, true);
            foreach (var keyFrame in trackArray)
            {
                var data = new TrackKeyFrameData();
                var frame = (int)keyFrame.GetField("frame").f;
                var value = keyFrame.GetField("value").f;
                var controlPoint1_x = keyFrame.GetField("controlPoint1_x")?.f ?? 0;
                var controlPoint1_y = keyFrame.GetField("controlPoint1_y")?.f ?? 0;
                var controlPoint2_x = keyFrame.GetField("controlPoint2_x")?.f ?? 1;
                var controlPoint2_y = keyFrame.GetField("controlPoint2_y")?.f ?? 1;
                data.controlPoint1 = new Vector2(controlPoint1_x, controlPoint1_y);
                data.controlPoint2 = new Vector2(controlPoint2_x, controlPoint2_y);
                data.value = value;
                data.frame = frame;
                track.keyFrames[frame] = data;
            }
        }

        BakeAllFrames();
    }

    public static Live2dMotionData Create()
    {
        var data = new Live2dMotionData();
        var info = new Live2dMotionInfo();
        info.fps = 30;
        info.fadein = 500;
        info.fadeout = 500;
        info.frameCount = 30;
        data.Build(info);
        return data;
    }

    public static Live2dMotionData Create(string text)
    {
        var data = new Live2dMotionData();
        var info = new Live2dMotionInfo();
        info.Parse(text);
        data.Build(info);
        return data;
    }
}

public class Live2dMotionInfo
{
    //FORMAT
    //$fps=30
    //$fadein=100
    //$fadeout=100
    //$fadein:%name=100
    //$fadeout:%name=100
    //$keyName=1,2,3


    public int fps;
    public int fadein;
    public int fadeout;
    public int frameCount;

    public Dictionary<string, int> paramFadeout = new Dictionary<string, int>();
    public Dictionary<string, int> paramFadein = new Dictionary<string, int>();

    public Dictionary<string, List<float>> keyFrames = new Dictionary<string, List<float>>();

    public List<float> GetKeyFrames(string name)
    {
        return keyFrames.TryGetValue(name, out var frames) ? frames : null;
    }

    public bool TryGetKeyFrameValue(string name, int frame, out float value)
    {
        var frames = GetKeyFrames(name);
        if (frames == null || frames.Count == 0)
        {
            value = 0;
            return false;
        }

        if (frames.Count <= frame)
        {
            value = frames[frames.Count - 1];
            return true;
        }

        value = frames[frame];
        return true;
    }

    private List<float> TryGetKeyFrames(string name)
    {
        if (keyFrames.TryGetValue(name, out var frames))
        {
            return frames;
        }

        return keyFrames[name] = new List<float>();
    }

    public void Parse(string text)
    {
        var lines = text.Replace("\r", "").Split('\n').Where(line => !string.IsNullOrWhiteSpace(line));
        foreach (var line in lines)
        {
            if (line.StartsWith("#"))
            {
                continue;
            }
            else if (line.StartsWith("$"))
            {
                var parts = line.Split('=');
                var nameParts = parts[0].Split(':');
                var name = nameParts[0];
                var subName = nameParts.Length > 1 ? nameParts[1] : null;
                bool hasSubName = subName != null;
                var value = parts[1];

                if (name == "$fps")
                {
                    fps = int.Parse(value);
                }
                else if (name == "$fadein")
                {
                    var val = int.Parse(value);
                    if (hasSubName)
                    {
                        paramFadein[subName] = val;
                    }
                    else
                    {
                        fadein = val;
                    }
                }
                else if (name == "$fadeout")
                {
                    var val = int.Parse(value);
                    if (hasSubName)
                    {
                        paramFadeout[subName] = val;
                    }
                    else
                    {
                        fadeout = val;
                    }
                }
            }
            else
            {
                var parts = line.Split('=');
                if (parts.Length != 2)
                {
                    continue;
                }
                var name = parts[0];
                var value = parts[1];

                var valueParts = value.Split(',').Select(float.Parse).ToList();

                var frames = TryGetKeyFrames(name);
                frames.AddRange(valueParts);
                frameCount = Mathf.Max(frameCount, frames.Count);
            }
        }
    }

    public void Parse2(string text)
    {
        var paramDict = text
            .Replace("\r", "")
            .Split('\n')
            .Select(line => line.Split("#")[0])
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                var parts = line.Split('=');
                if (parts.Length != 2)
                {
                    return null;
                }
                var name = parts[0];
                var value = parts[1];
                var valueParts = value.Split(',').Select(float.Parse).ToList();
                return new { name, valueParts };
            })
            .Where(x => x != null)
            .ToDictionary(x => x.name, x => x.valueParts);

        var fadeinDict = paramDict.Where(x => x.Key.StartsWith("$fadein:")).ToDictionary(x => x.Key[8..], x => (int)x.Value[0]);
        var fadeoutDict = paramDict.Where(x => x.Key.StartsWith("$fadeout:")).ToDictionary(x => x.Key[9..], x => (int)x.Value[0]);

        // fadein fadeout fps都是在paramdict的
        this.fadein = (int)paramDict["$fadein"][0];
        this.fadeout = (int)paramDict["$fadeout"][0];
        this.fps = (int)paramDict["$fps"][0];
        this.keyFrames = paramDict.Where(x => !x.Key.StartsWith("$")).ToDictionary(x => x.Key, x => x.Value);
        this.paramFadein = fadeinDict;
        this.paramFadeout = fadeoutDict;
    }

    public string Print()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# frameCount={frameCount}");
        sb.AppendLine();
        sb.AppendLine($"$fps={fps}");
        sb.AppendLine();
        sb.AppendLine($"$fadein={fadein}");
        sb.AppendLine();
        sb.AppendLine($"$fadeout={fadeout}");
        sb.AppendLine();
        foreach (var param in paramFadein)
        {
            sb.AppendLine($"$fadein:{param.Key}={param.Value}");
            sb.AppendLine();
        }
        foreach (var param in paramFadeout)
        {
            sb.AppendLine($"$fadeout:{param.Key}={param.Value}");
            sb.AppendLine();
        }
        foreach (var param in keyFrames)
        {
            if (param.Value.Count == 0)
                continue;
            var values = param.Value.Select(v => L2DWUtils.GetShortNumberString(v)).ToList();
            sb.AppendLine($"{param.Key}={string.Join(",", values)}");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public float GetFrame(string name, int frame)
    {
        var frames = TryGetKeyFrames(name);
        if (frames.Count <= frame)
        {
            return 0;
        }
        return frames[frame];
    }

    public bool TryGetFrame(string name, int frame, out float value)
    {
        var frames = TryGetKeyFrames(name);
        if (frames.Count <= frame)
        {
            value = 0;
            return false;
        }
        value = frames[frame];
        return true;
    }
}
