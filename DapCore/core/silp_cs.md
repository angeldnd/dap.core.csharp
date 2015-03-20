# VARS_HELPER(type, cs_type) #
```C#
public ${type}Var Add${type}(string path, ${cs_type} val) {
    ${type}Var v = Add<${type}Var>(path);
    if (v != null) {
        v.SetValue(val);
    }
    return v;
}

public ${type}Var Remove${type}(string path) {
    return Remove<${type}Var>(path);
}


public bool Is${type}(string path) {
    return Get<${type}Var>(path) != null;
}

public ${cs_type} Get${type}(string path) {
    ${type}Var v = Get<${type}Var>(path);
    if (v != null) {
        return v.Value;
    }
    return default(${cs_type});
}

public ${cs_type} Get${type}(string path, ${cs_type} defaultValue) {
    ${type}Var v = Get<${type}Var>(path);
    if (v != null) {
        return v.Value;
    }
    return defaultValue;
}

public bool SetValue(string path, ${cs_type} val) {
    ${type}Var v = Get<${type}Var>(path);
    if (v != null) {
        return v.SetValue(val);
    }
    return false;
}

```

# VAR_CLASS(type, cs_type) #
```C#
public class ${type}Var : Var<${cs_type}> {
    public override string Type {
        get { return VarsConsts.Type${type}Var; }
    }
        
    protected override bool DoEncode(Data data) {
        return data.Set${type}(VarsConsts.KeyValue, Value);
    }

    protected override bool DoDecode(Data data) {
        SetValue(data.Get${type}(VarsConsts.KeyValue));
        return true;
    }
}

```
# DATA_TYPE(type, cs_type) #
```
public bool Is${type}(string key) {
    DataType type = GetType(key);
    return type == DataType.${type};
}

public ${cs_type} Get${type}(string key) {
    if (_${type}Values != null && Is${type}(key)) {
        ${cs_type} result;
        if (_${type}Values.TryGetValue(key, out result)) {
            return result;
        }
    }
    return default(${cs_type}); 
}

public ${cs_type} Get${type}(string key, ${cs_type} defaultValue) {
    if (_${type}Values != null && Is${type}(key)) {
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
        if (_${type}Values == null) {
            _${type}Values = new Dictionary<string, ${cs_type}>();
        }
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
public bool Inited {
    get { return _Inited; }
}

public bool Init(Entity entity, string path) {
    if (_Inited) return false;
    if (entity == null || string.IsNullOrEmpty(path)) return false;

    _Entity = entity;
    _Path = path;
    _Inited = true;
    OnInit();
    return true;
}

```

# ASPECT_ONINIT_MIXIN() #
```
protected virtual void OnInit() {}
```

# ASPECT_ENCODE_DECODE_MIXIN() #
```
protected virtual bool DoEncode(Data data) {
    return true;
}

protected virtual bool DoDecode(Data data) {
    return true;
}

```

# DAPOBJECT_MIXIN() #
```
public virtual string Type {
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
    if (LogDebug) Debug("Not Encodable!");
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

# ENTITY_LOG_MIXIN() #
```
private DebugLogger _DebugLogger = DebugLogger.Instance;

private bool _DebugMode = false;
public bool DebugMode {
    get { return _DebugMode; }
    set {
        _DebugMode = true;
    }
}

private string[] _DebugPatterns = {""};
public virtual string[] DebugPatterns {
    get { return _DebugPatterns; }
    set {
        _DebugPatterns = value;
    }
}

public virtual bool LogDebug {
    get { return _DebugMode || Log.LogDebug; }
}

public virtual string GetLogPrefix() {
    return string.Format("[{0}] [{1}] ", GetType().Name, Name);
}

public void Critical(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Critical(GetLogPrefix() + string.Format(format, values));
    }
}

public void Error(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Error(GetLogPrefix() + string.Format(format, values));
    }
}

public void Info(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.INFO, DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Info(GetLogPrefix() + string.Format(format, values));
    }
}

public void Debug(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Debug(GetLogPrefix() + string.Format(format, values));
    }
}

```

# ASPECT_LOG_MIXIN() #
```C#
private DebugLogger _DebugLogger = DebugLogger.Instance;

public bool DebugMode {
    get { return Entity != null && Entity.DebugMode; }
}

public bool LogDebug {
    get { return (Entity != null && Entity.LogDebug) || Log.LogDebug; }
}

public virtual string GetLogPrefix() {
    if (_Entity != null) {
        return string.Format("{0}[{1}] ", _Entity.GetLogPrefix(), GetType().Name);
    } else {
        return string.Format("[] [] [{0}] ", GetType().Name);
    }
}

public void Critical(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Critical(GetLogPrefix() + string.Format(format, values));
    }
}

public void Error(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Error(GetLogPrefix() + string.Format(format, values));
    }
}

public void Info(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Info(GetLogPrefix() + string.Format(format, values));
    }
}

public void Debug(string format, params object[] values) {
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,
                GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
    } else {
        Log.Debug(GetLogPrefix() + string.Format(format, values));
    }
}

```

# ENTITY_ASPECT_LOG_MIXIN() #
```
public override bool LogDebug {
    get { return base.LogDebug || (_Entity != null && _Entity.LogDebug); }
}

public override string GetLogPrefix() {
    if (Entity != null) {
        return string.Format("{0}[{1}] [{2}]", Entity.GetLogPrefix(), GetType().Name, Name);
    } else {
        return string.Format("[] [] [{0}] [{1}] ", GetType().Name, Name);
    }
}

public override string[] DebugPatterns {
    get {
        string[] basePatterns = base.DebugPatterns;
        string[] entityPatterns = null;
        if (_Entity != null) {
            entityPatterns = _Entity.DebugPatterns;
        }
        if (basePatterns == null || basePatterns.Length == 0) {
            return entityPatterns;
        } else if (entityPatterns == null || entityPatterns.Length == 0) {
            return basePatterns;
        }
        string[] result = new string[basePatterns.Length + entityPatterns.Length];
        basePatterns.CopyTo(result, 0);
        entityPatterns.CopyTo(result, basePatterns.Length);
        return result; 
    }
}

```

