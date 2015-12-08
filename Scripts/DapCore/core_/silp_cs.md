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
    if (_Sealed && !key.StartsWith(VarPrefix)) {
        return false;
    }
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

public virtual bool Init(Entity entity, string path) {
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

public virtual string[] DebugPatterns {
    get { return null; }
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

# ACCESSOR_LOG_MIXIN(source, target, entity) #
```C#
public override bool DebugMode {
    get { return ${target} != null && ${target}.DebugMode; }
}

public override string[] DebugPatterns {
    get { return ${target} != null ? ${target}.DebugPatterns : null; }
}

```

# SECURABLE_ASPECT_MIXIN() #
```C#
private Pass _Pass = null;
protected Pass Pass {
    get { return _Pass; }
}

public bool AdminSecured {
    get {
        return _Pass != null;
    }
}

public bool WriteSecured {
    get {
        if (_Pass == null) return false;
        if (_Pass.Writable) return false;
        return true;
    }
}

public virtual bool Init(Entity entity, string path, Pass pass) {
    if (!base.Init(entity, path)) {
        return false;
    }
    _Pass = pass;
    return true;
}

public override sealed bool Init(Entity entity, string path) {
    return Init(entity, path, null);
}

public bool CheckAdminPass(Pass pass) {
    if (_Pass == null) return true;
    if (_Pass.CheckAdminPass(this, pass)) return true;

    Error("Invalid Admin Pass: _Pass = {0}, pass = {1}", _Pass, pass);
    return false;
}

public bool CheckWritePass(Pass pass) {
    if (_Pass == null) return true;
    if (_Pass.CheckWritePass(this, pass)) return true;

    Error("Invalid Write Pass: _Pass = {0}, pass = {1}", _Pass, pass);
    return false;
}

```

