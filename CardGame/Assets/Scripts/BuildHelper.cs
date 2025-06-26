using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

public class BuildHelper
{
    [MenuItem("Build/Build Doudizhu Game")]
    public static void BuildGame()
    {
        // 设置构建场景
        string[] scenes = { "Assets/doudizhubasic.unity" };
        
        // 构建选项
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = GetBuildPath();
        buildPlayerOptions.target = GetBuildTarget();
        buildPlayerOptions.options = BuildOptions.None;
        
        // 开始构建
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("构建成功: " + summary.outputPath);
            Debug.Log("构建大小: " + summary.totalSize + " bytes");
            
            // 在文件夹中显示构建结果
            #if UNITY_EDITOR_WIN
                System.Diagnostics.Process.Start("explorer.exe", "/select," + summary.outputPath.Replace("/", "\\"));
            #elif UNITY_EDITOR_OSX
                System.Diagnostics.Process.Start("open", "-R " + summary.outputPath);
            #endif
        }
        else
        {
            Debug.LogError("构建失败");
        }
    }
    
    static string GetBuildPath()
    {
        string projectPath = Application.dataPath.Replace("/Assets", "");
        string buildFolder = Path.Combine(projectPath, "Builds");
        
        if (!Directory.Exists(buildFolder))
        {
            Directory.CreateDirectory(buildFolder);
        }
        
        #if UNITY_EDITOR_WIN
            return Path.Combine(buildFolder, "DouDiZhu.exe");
        #elif UNITY_EDITOR_OSX
            return Path.Combine(buildFolder, "DouDiZhu.app");
        #elif UNITY_EDITOR_LINUX
            return Path.Combine(buildFolder, "DouDiZhu");
        #else
            return Path.Combine(buildFolder, "DouDiZhu");
        #endif
    }
    
    static BuildTarget GetBuildTarget()
    {
        #if UNITY_EDITOR_WIN
            return BuildTarget.StandaloneWindows64;
        #elif UNITY_EDITOR_OSX
            return BuildTarget.StandaloneOSX;
        #elif UNITY_EDITOR_LINUX
            return BuildTarget.StandaloneLinux64;
        #else
            return BuildTarget.StandaloneOSX;
        #endif
    }
    
    [MenuItem("Build/Build All Platforms")]
    public static void BuildAllPlatforms()
    {
        string[] scenes = { "Assets/doudizhubasic.unity" };
        string projectPath = Application.dataPath.Replace("/Assets", "");
        string buildFolder = Path.Combine(projectPath, "Builds");
        
        if (!Directory.Exists(buildFolder))
        {
            Directory.CreateDirectory(buildFolder);
        }
        
        // Windows 64位
        BuildPlayerOptions windowsBuild = new BuildPlayerOptions();
        windowsBuild.scenes = scenes;
        windowsBuild.locationPathName = Path.Combine(buildFolder, "Windows", "DouDiZhu.exe");
        windowsBuild.target = BuildTarget.StandaloneWindows64;
        windowsBuild.options = BuildOptions.None;
        
        // Mac
        BuildPlayerOptions macBuild = new BuildPlayerOptions();
        macBuild.scenes = scenes;
        macBuild.locationPathName = Path.Combine(buildFolder, "Mac", "DouDiZhu.app");
        macBuild.target = BuildTarget.StandaloneOSX;
        macBuild.options = BuildOptions.None;
        
        // Linux
        BuildPlayerOptions linuxBuild = new BuildPlayerOptions();
        linuxBuild.scenes = scenes;
        linuxBuild.locationPathName = Path.Combine(buildFolder, "Linux", "DouDiZhu");
        linuxBuild.target = BuildTarget.StandaloneLinux64;
        linuxBuild.options = BuildOptions.None;
        
        Debug.Log("开始构建所有平台版本...");
        
        // 构建Windows版本
        BuildReport windowsReport = BuildPipeline.BuildPlayer(windowsBuild);
        if (windowsReport.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Windows版本构建成功");
        }
        
        // 构建Mac版本
        BuildReport macReport = BuildPipeline.BuildPlayer(macBuild);
        if (macReport.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Mac版本构建成功");
        }
        
        // 构建Linux版本
        BuildReport linuxReport = BuildPipeline.BuildPlayer(linuxBuild);
        if (linuxReport.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Linux版本构建成功");
        }
        
        Debug.Log("所有平台构建完成！检查Builds文件夹。");
    }
}