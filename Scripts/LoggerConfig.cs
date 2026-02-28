using Godot;

public partial class LoggerConfig : Node
{
    [Export] public LogLevel DefaultLogLevel = LogLevel.Info;
    [Export] public bool EnableTimestamp = true;
    [Export] public bool EnableClassName = true;
    [Export] public bool EnableColors = true;

    public override void _Ready()
    {
        // Configure the logger with the exported settings
        Logger.SetLogLevel(DefaultLogLevel);
        Logger.EnableTimestamp(EnableTimestamp);
        Logger.EnableClassName(EnableClassName);
        Logger.EnableColors(EnableColors);

        Logger.Info("Logger initialized successfully", "LoggerConfig");
    }

    // Helper method to change log level at runtime
    public void ChangeLogLevel(LogLevel newLevel)
    {
        Logger.SetLogLevel(newLevel);
    }

    // Helper method to toggle debug mode
    public void ToggleDebugMode()
    {
        if (DefaultLogLevel == LogLevel.Debug)
        {
            ChangeLogLevel(LogLevel.Info);
            DefaultLogLevel = LogLevel.Info;
        }
        else
        {
            ChangeLogLevel(LogLevel.Debug);
            DefaultLogLevel = LogLevel.Debug;
        }
    }
}