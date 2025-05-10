using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using System.Linq;

public static class UIScriptGenerator
{
    public static GameObject rootObject;

    // 定义命名前缀与组件类型的映射，方便扩展
    private static Dictionary<string, string> prefixToComponentType = new Dictionary<string, string>
    {
        { "m_btn", typeof(Button).Name },
        { "m_lbl", typeof(Text).Name },
        { "m_item", typeof(Transform).Name },
        { "m_tf", typeof(Transform).Name },
        { "m_ipt", typeof(InputField).Name },
        { "m_toggle", typeof(Toggle).Name },
        { "m_img", typeof(Image).Name },
        { "m_scroll", typeof(ScrollRect).Name },
        { "m_style", typeof(MonoUIStyle).Name },
        { "m_slider", typeof(Slider).Name },
        { "m_go", typeof(GameObject).Name },
        { "m_rect", typeof(RectTransform).Name },
        { "m_dropdown", typeof(Dropdown).Name },
    };

    private static Dictionary<string, string> reserverPrefixToComponentType = new Dictionary<string, string>();

    static UIScriptGenerator()
    {
        foreach (var prefix in prefixToComponentType.Keys)
        {
            reserverPrefixToComponentType[prefixToComponentType[prefix]] = prefix;
        }
    }

    private static string GetEventMemberName(string name, string type)
    {
        // replace m_xx to XX
        if (reserverPrefixToComponentType.TryGetValue(type, out var prefix))
        {
            name = name.Replace(prefix, "");
            return name.Substring(0, 1).ToUpper() + name.Substring(1);
        }
        return name;
    }

    private static HashSet<string> ignorePrefixes = new HashSet<string>
    {
        "m_item",

    };

    //右键GameObject上下文菜单
    [MenuItem("GameObject/生成UI脚本")]
    public static void GenerateCode()
    {
        rootObject = Selection.activeGameObject;
        if (rootObject == null)
        {
            Debug.LogError("请选择一个GameObject");
            return;
        }
        GenerateCode(rootObject);
    }

    public static void GenerateCode(GameObject target)
    {
        ResetAllStringBuilder();
        var root = target.transform;
        GenerateRecursive(root, root);

        sbCode.AppendLine("\t#region auto generated members");
        sbCode.Append(sbMember.ToString());
        sbCode.AppendLine("\t#endregion");
        sbCode.AppendLine();

        sbCode.AppendLine("\t#region auto generated binders");
        sbCode.AppendLine("\tprotected override void CodeGenBindMembers()");
        sbCode.AppendLine("\t{");
        sbCode.AppendLine(sbMemberBinder.ToString());
        sbCode.Append(sbEventBinder.ToString());
        sbCode.AppendLine("\t}");
        sbCode.AppendLine("\t#endregion");
        sbCode.AppendLine();

        sbCode.AppendLine("\t#region auto generated events");
        sbCode.Append(sbEvent.ToString());
        sbCode.AppendLine("\t#endregion");
        sbCode.AppendLine();


        var result = sbCode.ToString().Replace("\t", "    ");
        Debug.Log(result);
        GUIUtility.systemCopyBuffer = result;
    }

    private static void ResetAllStringBuilder()
    {
        sbCode.Clear();
        sbMember.Clear();
        sbMemberBinder.Clear();
        sbEventBinder.Clear();
        sbEvent.Clear();
    }

    private static StringBuilder sbCode = new StringBuilder();
    private static StringBuilder sbMember = new StringBuilder();
    private static StringBuilder sbMemberBinder = new StringBuilder();
    private static StringBuilder sbEventBinder = new StringBuilder();
    private static StringBuilder sbEvent = new StringBuilder();
    private static void GenerateRecursive(Transform root, Transform target)
    {
        foreach (Transform child in target)
        {
            var type = GetComponentType(child.name);
            var relativePath = GetPathFromRoot(root, child);
            string name = child.name;
            if (type != null)
            {
                GenerateMember(type, name, relativePath);
                GenerateMemberBinder(type, name, relativePath);
                GenerateEventBinder(type, name);
            }

            if (ignorePrefixes.Any(prefix => child.name.StartsWith(prefix)))
            {
                continue;
            }
            GenerateRecursive(root, child);
        }
    }

    private static void GenerateMember(string type, string name, string relativePath)
    {
        switch (type)
        {
            default:
                sbMember.AppendLine($"\tprivate {type} {name};");
                break;
        }
    }

    private static void GenerateMemberBinder(string type, string name, string relativePath)
    {
        switch (type)
        {
            case "GameObject":
                sbMemberBinder.AppendLine($"\t\t{name} = transform.Find(\"{relativePath}\").gameObject;");
                break;
            default:
                sbMemberBinder.AppendLine($"\t\t{name} = transform.Find(\"{relativePath}\").GetComponent<{type}>();");
                break;
        }
    }

    private static void GenerateEventBinder(string type, string name)
    {
        string eventMemberName = GetEventMemberName(name, type);
        string funcName = "";
        string param = "";
        void AppendDefaultEventBlock()
        {
            sbEvent.AppendLine($"\tprivate void {funcName}({param})");
            sbEvent.AppendLine("\t{");
            sbEvent.AppendLine($"\t\tDebug.Log(\"{funcName}\");");
            sbEvent.AppendLine("\t}");
        }
        switch (type)
        {
            case "Button":
                funcName = $"On{type}{eventMemberName}Click";
                param = "";
                sbEventBinder.AppendLine($"\t\t{name}.onClick.AddListener({funcName});");
                AppendDefaultEventBlock();
                break;
            case "Toggle":
                funcName = $"On{type}{eventMemberName}Change";
                param = "bool value";
                sbEventBinder.AppendLine($"\t\t{name}.onValueChanged.AddListener({funcName});");
                AppendDefaultEventBlock();
                break;
            case "InputField":
                funcName = $"On{type}{eventMemberName}Change";
                param = "string value";
                sbEventBinder.AppendLine($"\t\t{name}.onValueChanged.AddListener({funcName});");
                AppendDefaultEventBlock();

                funcName = $"On{type}{eventMemberName}EndEdit";
                param = "string value";
                sbEventBinder.AppendLine($"\t\t{name}.onEndEdit.AddListener({funcName});");
                AppendDefaultEventBlock();
                break;
            case "Slider":
                funcName = $"On{type}{eventMemberName}Change";
                param = "float value";
                sbEventBinder.AppendLine($"\t\t{name}.onValueChanged.AddListener({funcName});");
                AppendDefaultEventBlock();
                break;
            case "Dropdown":
                funcName = $"On{type}{eventMemberName}Change";
                param = "int value";
                sbEventBinder.AppendLine($"\t\t{name}.onValueChanged.AddListener({funcName});");
                AppendDefaultEventBlock();
                break;
        }
    }

    private static string GetComponentType(string objectName)
    {
        foreach (var prefix in prefixToComponentType.Keys)
        {
            if (objectName.StartsWith(prefix))
            {
                return prefixToComponentType[prefix];
            }
        }
        return null;
    }

    private static string GetPathFromRoot(Transform root, Transform target)
    {
        List<string> pathParts = new List<string>();
        Transform current = target;
        while (current != root)
        {
            pathParts.Insert(0, current.name);
            current = current.parent;
        }
        return string.Join("/", pathParts);
    }
}