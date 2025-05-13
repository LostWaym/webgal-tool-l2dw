

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Live2dMotionData
{
    public class Track
    {
        public string name;
        public Dictionary<int, float> keyFrames = new Dictionary<int, float>();
        public bool HasKeyFrame(int frame)
        {
            return keyFrames.ContainsKey(frame);
        }
    }
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
                float frame = param.Value[i];
                track.keyFrames[i] = frame;
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

    public void BakeFrames(string trackName)
    {
        var track = TryGetTrack(trackName, true);
        // 使用线性过渡处理关键帧
        var keyFrames = track.keyFrames.OrderBy(kv => kv.Key).ToList();
        var frames = new List<float>();

        for (int i = 0; i < keyFrames.Count; i++)
        {
            frames.Add(keyFrames[i].Value);

            // 如果不是最后一个关键帧，添加线性过渡帧
            if (i < keyFrames.Count - 1)
            {
                int currentFrame = keyFrames[i].Key;
                int nextFrame = keyFrames[i + 1].Key;
                float currentValue = keyFrames[i].Value;
                float nextValue = keyFrames[i + 1].Value;

                // 在两个关键帧之间添加线性过渡
                for (int frame = currentFrame + 1; frame < nextFrame; frame++)
                {
                    float t = (float)(frame - currentFrame) / (nextFrame - currentFrame);
                    float interpolatedValue = Mathf.Lerp(currentValue, nextValue, t);
                    frames.Add(interpolatedValue);
                }
            }
        }
        info.keyFrames[trackName] = frames;
    }

    public string Save()
    {
        var jsonobject = new JSONObject(JSONObject.Type.OBJECT);
        jsonobject.AddField("name", "name");
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
                keyFrameObject.AddField("value", keyFrame.Value);
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
                var frame = (int)keyFrame.GetField("frame").f;
                var value = keyFrame.GetField("value").f;
                track.keyFrames[frame] = value;
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
            if (line.StartsWith("$"))
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
            var values = param.Value.Select(v => v.ToString("F3")).ToList();
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
}
