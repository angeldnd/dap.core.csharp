# SINGLETON_MIXIN() #
```C#
protected static T _Instance;

public static T GetInstance(bool logError) {
    if (_Instance == null) {
        _Instance = Helper.GetSingleton<T>(logError);
    }
    return _Instance;
}

public static T Instance {
    get {
        return GetInstance(true);
    }
}

public static bool HasInstance() {
    return GetInstance(false) != null;
}
```

# ASPECT_LOG_MIXIN() #
```C#
private DebugLogger _DebugLogger = DebugLogger.Instance;

public abstract Entity Entity { get; }

public bool LogDebug {
    get { return Entity != null && Entity.DebugMode; }
}

public virtual string GetLogPrefix() {
    if (Entity != null) {
        return string.Format("{0}[{1}] ", Entity.GetLogPrefix(), GetType().Name);
    } else {
        return string.Format("[] [] [{0}] ", GetType().Name);
    }
}

public void Critical(string format, params object[] values) {
    if (LogDebug) {
        _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Critical(GetLogPrefix() + string.Format(format, values));
    }
}

public void Error(string format, params object[] values) {
    if (LogDebug) {
        _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Error(GetLogPrefix() + string.Format(format, values));
    }
}

public void Info(string format, params object[] values) {
    if (LogDebug) {
        _DebugLogger.LogWithPatterns("INFO", Entity.DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Info(GetLogPrefix() + string.Format(format, values));
    }
}

public void Debug(string format, params object[] values) {
    if (LogDebug) {
        _DebugLogger.LogWithPatterns("DEBUG", Entity.DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Debug(GetLogPrefix() + string.Format(format, values));
    }
}
```
