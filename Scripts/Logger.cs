using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

public enum LogLevel
{
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
    Critical = 4
}

public static class Logger
{
    private static LogLevel _currentLogLevel = LogLevel.Debug;
    private static bool _enableTimestamp = true;
    private static bool _enableClassName = true;
    private static bool _enableColors = true;

    // Configuration methods
    public static void SetLogLevel(LogLevel level)
    {
        _currentLogLevel = level;
        Info($"Log level set to: {level}");
    }

    public static void EnableTimestamp(bool enable)
    {
        _enableTimestamp = enable;
    }

    public static void EnableClassName(bool enable)
    {
        _enableClassName = enable;
    }

    public static void EnableColors(bool enable)
    {
        _enableColors = enable;
    }

    // Main logging methods with automatic class detection
    public static void Debug(string message, [CallerFilePath] string callerPath = "")
    {
        Log(LogLevel.Debug, message, GetClassNameFromPath(callerPath));
    }

    public static void Info(string message, [CallerFilePath] string callerPath = "")
    {
        Log(LogLevel.Info, message, GetClassNameFromPath(callerPath));
    }

    public static void Warning(string message, [CallerFilePath] string callerPath = "")
    {
        Log(LogLevel.Warning, message, GetClassNameFromPath(callerPath));
    }

    public static void Error(string message, [CallerFilePath] string callerPath = "")
    {
        Log(LogLevel.Error, message, GetClassNameFromPath(callerPath));
    }

    public static void Critical(string message, [CallerFilePath] string callerPath = "")
    {
        Log(LogLevel.Critical, message, GetClassNameFromPath(callerPath));
    }

    // Exception logging with automatic class detection
    public static void Exception(Exception ex, string additionalMessage = "", [CallerFilePath] string callerPath = "")
    {
        string message = string.IsNullOrEmpty(additionalMessage)
            ? $"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}"
            : $"{additionalMessage} - Exception: {ex.Message}\nStack Trace: {ex.StackTrace}";

        Log(LogLevel.Critical, message, GetClassNameFromPath(callerPath));
    }

    // Core logging method
    private static void Log(LogLevel level, string message, string className)
    {
        if (level < _currentLogLevel)
            return;

        string formattedMessage = FormatMessage(level, message, className);

        switch (level)
        {
            case LogLevel.Debug:
                GD.Print(formattedMessage);
                break;
            case LogLevel.Info:
                GD.Print(formattedMessage);
                break;
            case LogLevel.Warning:
                GD.PrintErr(formattedMessage);
                break;
            case LogLevel.Error:
                GD.PrintErr(formattedMessage);
                break;
            case LogLevel.Critical:
                GD.PrintErr(formattedMessage);
                break;
        }
    }

    private static string FormatMessage(LogLevel level, string message, string className)
    {
        string timestamp = _enableTimestamp ? $"[{DateTime.Now:HH:mm:ss.fff}] " : "";
        string logLevelStr = GetLogLevelString(level);
        string classStr = _enableClassName && !string.IsNullOrEmpty(className) ? $"[{className}] " : "";

        return $"{timestamp}{logLevelStr}{classStr}{message}";
    }

    private static string GetLogLevelString(LogLevel level)
    {
        return $"[{level.ToString().ToUpper()}] ";
    }

    // Conditional logging with automatic class detection
    public static void DebugIf(bool condition, string message, [CallerFilePath] string callerPath = "")
    {
        if (condition) Debug(message, callerPath);
    }

    public static void InfoIf(bool condition, string message, [CallerFilePath] string callerPath = "")
    {
        if (condition) Info(message, callerPath);
    }

    public static void WarningIf(bool condition, string message, [CallerFilePath] string callerPath = "")
    {
        if (condition) Warning(message, callerPath);
    }

    public static void ErrorIf(bool condition, string message, [CallerFilePath] string callerPath = "")
    {
        if (condition) Error(message, callerPath);
    }

    // Utility methods for common scenarios with automatic class detection
    public static void LogMethodEntry(string methodName, [CallerFilePath] string callerPath = "")
    {
        Debug($"Entering method: {methodName}", callerPath);
    }

    public static void LogMethodExit(string methodName, [CallerFilePath] string callerPath = "")
    {
        Debug($"Exiting method: {methodName}", callerPath);
    }

    public static void LogVariable(string variableName, object value, [CallerFilePath] string callerPath = "")
    {
        Debug($"{variableName} = {value}", callerPath);
    }

    // Helper method to extract class name from file path
    private static string GetClassNameFromPath(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return string.Empty;

        string fileName = Path.GetFileNameWithoutExtension(filePath);
        return fileName;
    }
}