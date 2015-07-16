# DECLARE_LIST(name, var_name, cs_type, list_name) #
```C#
protected List<${cs_type}> ${list_name} = null;

public bool Add${name}(${cs_type} ${var_name}) {
    if (${list_name} == null) ${list_name} = new List<${cs_type}>();
    if (!${list_name}.Contains(${var_name})) {
        ${list_name}.Add(${var_name});
        return true;
    }
    return false;
}

public bool Remove${name}(${cs_type} ${var_name}) {
    if (${list_name} != null && ${list_name}.Contains(${var_name})) {
        ${list_name}.Remove(${var_name});
        return true;
    }
    return false;
}

``` 

# DATA_TYPE(type, cs_type) #
```
public bool Is${type}(string key) {
    DataType type = GetValueType(key);
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

public bool Set${type}(string key, ${cs_type} val) {
    if (!_ValueTypes.ContainsKey(key)) {
        _ValueTypes[key] = DataType.${type};
        if (_${type}Values == null) {
            _${type}Values = new Dictionary<string, ${cs_type}>();
        }
        _${type}Values[key] = val;
        return true;
    }
    return false;
}

``` 

# DATA_QUICK_SETTER(name, type, cs_type) #
```
public Data ${name}(string key, ${cs_type} val) {
    Set${type}(key, val);
    return this;
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

public string RevPath {
    get {
        return string.Format("{0} ({1})", _Path, Revision);
    }
}

public bool Inited {
    get { return _Entity != null; }
}

public bool Init(Entity entity, string path) {
    if (_Entity != null) {
        Error("Already Inited: {0} -> {1}, {2}", _Entity, entity, path);
        return false;
    }
    if (entity == null) {
        Error("Invalid Entity: {0}, {1}", entity, path);
        return false;
    }
    if (string.IsNullOrEmpty(path)) {
        Error("Invalid Path: {0}, {1}", entity, path);
        return false;
    }

    _Entity = entity;
    _Path = path;
    return true;
}

```

# ASPECT_EVENTS_MIXIN() #
```
public virtual void OnAdded() {}
public virtual void OnRemoved() {}
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

private int _Revision = 0;
public int Revision {
    get { return _Revision; }
}

protected virtual void AdvanceRevision() {
    _Revision += 1;
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
    return string.Format("[{0}] ({1}) ", GetType().Name, Revision);
}

public void Critical(string format, params object[] values) {
    Log.Source = this;
    if (DebugMode) {
        _DebugLogger.Critical(
            _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Critical(GetLogPrefix() + string.Format(format, values));
    }
}

public void Error(string format, params object[] values) {
    Log.Source = this;
    if (DebugMode) {
        _DebugLogger.Error(
            _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Error(GetLogPrefix() + string.Format(format, values));
    }
}

public void Info(string format, params object[] values) {
    Log.Source = this;
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.INFO, DebugPatterns,
                _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Info(GetLogPrefix() + string.Format(format, values));
    }
}

public void Debug(string format, params object[] values) {
    Log.Source = this;
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, DebugPatterns,
                _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Debug(GetLogPrefix() + string.Format(format, values));
    }
}

```

# ASPECT_LOG_MIXIN(virtualOrOverride) #
```C#
public ${virtualOrOverride} string GetLogPrefix() {
    if (_Entity != null) {
        return string.Format("{0}[{1}] {2} ", _Entity.GetLogPrefix(), GetType().Name, RevPath);
    } else {
        return string.Format("[] [{0}] {1} ", GetType().Name, RevPath);
    }
}
```

# ACCESSOR_LOG_MIXIN(source, target) #
```C#
private DebugLogger _DebugLogger = DebugLogger.Instance;

public bool DebugMode {
    get { return ${target} != null && ${target}.DebugMode; }
}

public bool LogDebug {
    get { return (${target} != null && ${target}.LogDebug) || Log.LogDebug; }
}

public void Critical(string format, params object[] values) {
    Log.Source = ${source};
    if (DebugMode) {
        _DebugLogger.Critical(
                _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Critical(GetLogPrefix() + string.Format(format, values));
    }
}

public void Error(string format, params object[] values) {
    Log.Source = ${source};
    if (DebugMode) {
        _DebugLogger.Error(
                _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Error(GetLogPrefix() + string.Format(format, values));
    }
}

public void Info(string format, params object[] values) {
    Log.Source = ${source};
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,
                _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Info(GetLogPrefix() + string.Format(format, values));
    }
}

public void Debug(string format, params object[] values) {
    Log.Source = ${source};
    if (DebugMode) {
        _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,
                _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));
    } else {
        Log.Debug(GetLogPrefix() + string.Format(format, values));
    }
}

```

# SECURABLE_ASPECT_MIXIN() #
```C#
private static readonly Pass OPEN_PASS = new Pass();

private Pass _Pass = null;
protected Pass Pass {
    get { return _Pass; }
}

public bool Secured {
    get {
        if (_Pass == null) return false;
        if (OPEN_PASS == _Pass) return false;
        return true;
    }
}

public bool SetPass(Pass pass) {
    /*
        * The OPEN_PASS trick is to set the pass, so it can't
        * be set in the future, but it's "open", any pass can
        * pass the check.
        */
    if (_Pass == null) {
        if (pass == null) {
            _Pass = OPEN_PASS;
        } else {
            _Pass = pass;
        }
        return true;
    } else if (_Pass == pass) {
        return true;
    } else if (OPEN_PASS == _Pass && pass == null) {
        return true;
    } else if (_Pass.Equals(pass)) {
        return true;
    }
    Error("SetPass Failed: {0} -> {1}", _Pass, pass);
    return false;
}

public bool CheckPass(Pass pass) {
    if (_Pass == null) return true;
    if (_Pass == pass) return true;
    if (OPEN_PASS == _Pass) return true;
    if (_Pass.Equals(pass)) return true;

    Error("Invalid Pass: _Pass = {0}, pass = {1}", _Pass, pass);
    return false;
}
```

