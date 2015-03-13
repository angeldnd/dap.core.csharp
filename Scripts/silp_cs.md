# DATA_TYPE(type, cs_type) #
```
public bool Is${type}(string key) {
    DataType type = GetType(key);
    return type == DataType.${type};
}

public ${cs_type} Get${type}(string key) {
    if (Is${type}(key)) {
        ${cs_type} result;
        if (_${type}Values.TryGetValue(key, out result)) {
            return result;
        }
    }
    return default(${cs_type}); 
}

public ${cs_type} Get${type}(string key, ${cs_type} defaultValue) {
    if (Is${type}(key)) {
        ${cs_type} result;
        if (_${type}Values.TryGetValue(key, out result)) {
            return result;
        }
    }
    return defaultValue;
}

public bool Set${type}(string key, ${cs_type} value) {
    if (!_ValueTypes.ContainsKey(key)) {
        _ValueTypes[key] = DataType.${type};
        _${type}Values[key] = value;
        return true;
    }
    return false;
}

``` 

# ASPECT_MIXIN() #
```
private Entity _Entity = null;
public Entity Entity {
    get { return _Entity; }
}

private string _Path = null;
public string Path {
    get { return _Path; }
}

private bool _Inited = false;
public bool Init(Entity entity, string path) {
    if (_Inited) return false;
    if (entity == null || string.IsNullOrEmpty(path)) return false;

    _Entity = entity;
    _Path = path;
    _Inited = true;
    return true;
}

protected virtual bool DoEncode(Data data) {
    return true;
}

protected virtual bool DoDecode(Data data) {
    return true;
}

```

# DAPOBJECT_MIXIN() #
```
public string Type {
    get { return null; }
}

public Data Encode() {
    if (!string.IsNullOrEmpty(Type)) {
        Data data = new Data();
        if (data.SetString(DapObjectConsts.KeyType, Type)) {
            if (DoEncode(data)) {
                return data;
            }
        }
    }
    if (LogDebug) Log.Debug("Not Encodable!");
    return null;
}

public bool Decode(Data data) {
    string type = data.GetString(DapObjectConsts.KeyType);
    if (type == Type) {
        return DoDecode(data);
    }
    return false;
}

```

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

# MONO_LOG_MIXIN() #
```
//Keep them public so default editor inspector can be shown
//TODO: Create custom editor so they can be private again
public bool _DebugMode = false;
public string[] _DebugPatterns = {""};

private DebugLogger _DebugLogger = DebugLogger.Instance;

public virtual bool LogDebug {
    get { return _DebugMode; }
}

public virtual string[] DebugPatterns {
    get { return _DebugPatterns; }
}

public virtual string GetLogPrefix() {
    return string.Format("[{0}] [{1}] ", GetType().Name, name);
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
        _DebugLogger.LogWithPatterns("INFO", DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Info(GetLogPrefix() + string.Format(format, values));
    }
}

public void Debug(string format, params object[] values) {
    if (LogDebug) {
        _DebugLogger.LogWithPatterns("DEBUG", DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Debug(GetLogPrefix() + string.Format(format, values));
    }
}

```

# ASPECT_LOG_MIXIN() #
```C#
private DebugLogger _DebugLogger = DebugLogger.Instance;

public bool LogDebug {
    get { return Entity != null && Entity.LogDebug; }
}

public virtual string GetLogPrefix() {
    if (_Entity != null) {
        return string.Format("{0}[{1}] ", _Entity.GetLogPrefix(), GetType().Name);
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
