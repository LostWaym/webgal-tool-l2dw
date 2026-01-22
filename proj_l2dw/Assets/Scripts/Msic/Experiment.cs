
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class Experiment
{
    public static CommandInfo Parse(string command)
    {
        // Trim trailing semicolon if present.
        // Assumes `command` argument is not null or whitespace (checked before this selection part).
        string processedCommand = command.Trim();
        if (processedCommand.EndsWith(";"))
        {
            processedCommand = processedCommand.Substring(0, processedCommand.Length - 1);
        }

        var commandInfo = new CommandInfo();
        // Initialize otherParameters, as it's used regardless of parsing path.
        commandInfo.otherParameters = new Dictionary<string, string>();

        int firstColonIndex = processedCommand.IndexOf(':');

        if (firstColonIndex == -1) // No colon found
        {
            // Treat the whole string as the command name.
            // No main parameter or other parameters will be parsed by this logic.
            // This behavior is an improvement over original code that might crash.
            commandInfo.command = processedCommand;
            commandInfo.commandParam = "";
            // otherParameters remains empty.
            // The `return commandInfo;` is outside the selection, so this flow will reach it.
            // To make it explicit that parsing stops here for this case:
             return commandInfo; 
        }
        
        if (firstColonIndex == 0) // Starts with colon, e.g., ":param"
        {
            commandInfo.command = ""; // Command name is empty
        }
        else
        {
            commandInfo.command = processedCommand.Substring(0, firstColonIndex);
        }
        
        // Extract string part after the first colon for parameters
        string remainingAfterCommand = (firstColonIndex == processedCommand.Length - 1) 
                                       ? "" 
                                       : processedCommand.Substring(firstColonIndex + 1).TrimStart();

        // Tokenize remainingAfterCommand by space, respecting braces for complex parameters
        List<string> tokens = new List<string>();
        if (!string.IsNullOrEmpty(remainingAfterCommand))
        {
            StringBuilder currentToken = new StringBuilder();
            int braceLevel = 0;
            for (int i = 0; i < remainingAfterCommand.Length; i++)
            {
                char c = remainingAfterCommand[i];
                if (c == '{')
                {
                    braceLevel++;
                }
                else if (c == '}')
                {
                    // Decrement braceLevel only if it's positive, to handle mismatched braces gracefully.
                    if (braceLevel > 0)
                    {
                        braceLevel--;
                    }
                }
                
                // 空格后是否紧接着-
                var isBeforeArg = (i + 1) < remainingAfterCommand.Length
                    && remainingAfterCommand[i + 1] == '-';
                if (c == ' ' && isBeforeArg && braceLevel == 0) // Token boundary
                {
                    if (currentToken.Length > 0)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }
                }
                else
                {
                    currentToken.Append(c);
                }
            }
            if (currentToken.Length > 0) // Add the last token
            {
                tokens.Add(currentToken.ToString());
            }
        }

        // Assign commandParam and parse other parameters from tokens
        int currentTokenIndex = 0;
        if (tokens.Count > 0 && !tokens[0].StartsWith("-"))
        {
            commandInfo.commandParam = tokens[0];
            currentTokenIndex = 1; // Next token will be processed as an option
        }
        else
        {
            commandInfo.commandParam = ""; // No main parameter, or the first token looks like an option
        }

        for (int i = currentTokenIndex; i < tokens.Count; i++)
        {
            string part = tokens[i];
            if (part.StartsWith("-"))
            {
                if (part.Length == 1) // Just a hyphen "-", ignore.
                {
                    continue; 
                }

                int equalsIndex = part.IndexOf('=');
                
                // Handles cases like "-=value" where key is empty after '-'.
                // These are considered malformed options and skipped.
                if (equalsIndex == 1) 
                {
                    continue;
                }

                if (equalsIndex > 1) // Format -key=value or -key=
                {
                    string key = part.Substring(1, equalsIndex - 1);
                    // If equalsIndex is the last char, value is empty. Otherwise, take substring.
                    string value = (equalsIndex < part.Length - 1) ? part.Substring(equalsIndex + 1) : "";
                    commandInfo.otherParameters[key] = value;
                }
                else // No '=', format -key (flag type parameter)
                {
                    string key = part.Substring(1);
                    commandInfo.otherParameters[key] = ""; 
                }
            }
            // Tokens not starting with '-' and not identified as commandParam are ignored.
            // Example: "command:mainParam strayToken -option=value" -> "strayToken" is ignored.
        }

        return commandInfo;
    }

    public static JSONObject ParseJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }
        var jsonObject = new JSONObject(json, false);
        return jsonObject;
    }

    public static JSONObject GetJsonObjectByPath(JSONObject jsonObject, string path)
    {
        var parts = path.Split('.');
        foreach (var part in parts)
        {
            if (jsonObject.IsObject)
            {
                jsonObject = jsonObject[part];
            }
            else
            {
                return null;
            }
        }
        return jsonObject;
    }

    // chinese regex
    private static readonly Regex chineseRegex = new Regex("[\u4e00-\u9fa5]");
    public static int GetSayDuration(string content, int chineseDuration = 250, int otherDuration = 100)
    {
        int totalDuration = 0;
        foreach (var c in content)
        {
            if (chineseRegex.IsMatch(c.ToString()))
            {
                totalDuration += chineseDuration;
            }
            else
            {
                totalDuration += otherDuration;
            }
        }
        
        return totalDuration;
    }
}

public class CommandInfo
{
    public string command;
    public string commandParam;
    public Dictionary<string, string> otherParameters = new Dictionary<string, string>();

    public bool HasParameter(string key)
    {
        return otherParameters.ContainsKey(key);
    }

    public string GetParameter(string key)
    {
        return otherParameters.TryGetValue(key, out var value) ? value : null;
    }

    public void SetParameter(string key, string value = "")
    {
        otherParameters[key] = value;
    }

    public void RemoveParameter(string key)
    {
        otherParameters.Remove(key);
    }

    public void RemoveEmptyParameter()
    {
        otherParameters.ToList().Where(p => string.IsNullOrWhiteSpace(p.Value)).ToList().ForEach(p => RemoveParameter(p.Key));
    }

    public string GetInstruction()
    {
        var list = otherParameters.ToList();


        var paramStr = string.Join(" ", list.Select(p => GetParamString(p.Key, p.Value)));
        return $"{command}:{commandParam} {paramStr};";
    }

    public string GetParamText()
    {
        var list = otherParameters.ToList();
        var paramStr = string.Join(" ", list.Select(p => GetParamString(p.Key, p.Value)));
        return $"{paramStr}";
    }
    string GetParamString(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return $"-{key}";
        }

        return $"-{key}={value}";
    }
}