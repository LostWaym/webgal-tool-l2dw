using System;
using System.IO;

public static class PathHelper
{
    /// <summary>
    /// 计算从源文件到目标文件的相对路径
    /// </summary>
    /// <param name="sourceFilePath">源文件的完整路径</param>
    /// <param name="targetFilePath">目标文件的完整路径</param>
    /// <returns>源文件到目标文件的相对路径</returns>
    public static string GetRelativePath(string sourceFilePath, string targetFilePath)
    {
        // 检查输入路径是否有效
        if (string.IsNullOrEmpty(sourceFilePath))
            throw new ArgumentNullException(nameof(sourceFilePath));
        
        if (string.IsNullOrEmpty(targetFilePath))
            throw new ArgumentNullException(nameof(targetFilePath));

        // 将路径转换为绝对路径
        string sourceFullPath = Path.GetFullPath(sourceFilePath);
        string targetFullPath = Path.GetFullPath(targetFilePath);

        // 获取目录信息
        string sourceDir = Path.GetDirectoryName(sourceFullPath);
        string targetDir = Path.GetDirectoryName(targetFullPath);

        // 分解路径为目录数组
        string[] sourceDirs = sourceDir.Split(Path.DirectorySeparatorChar);
        string[] targetDirs = targetDir.Split(Path.DirectorySeparatorChar);

        // 找到共同的根目录
        int commonIndex = 0;
        while (commonIndex < sourceDirs.Length && commonIndex < targetDirs.Length 
               && string.Equals(sourceDirs[commonIndex], targetDirs[commonIndex], StringComparison.OrdinalIgnoreCase))
        {
            commonIndex++;
        }

        // 计算需要向上返回的目录数
        int upDirs = sourceDirs.Length - commonIndex;

        // 构建相对路径
        System.Text.StringBuilder relativePath = new System.Text.StringBuilder();

        // 添加向上导航的部分
        for (int i = 0; i < upDirs; i++)
        {
            relativePath.Append("..");
            relativePath.Append(Path.DirectorySeparatorChar);
        }

        // 添加向下导航到目标文件的部分
        for (int i = commonIndex; i < targetDirs.Length; i++)
        {
            relativePath.Append(targetDirs[i]);
            relativePath.Append(Path.DirectorySeparatorChar);
        }

        // 添加目标文件名
        relativePath.Append(Path.GetFileName(targetFullPath));

        string result = relativePath.ToString();
        
        // 如果是同一目录，确保没有多余的路径分隔符
        if (upDirs == 0 && commonIndex == targetDirs.Length)
        {
            return Path.GetFileName(targetFullPath);
        }

        return result.Replace("\\", "/");
    }
}
