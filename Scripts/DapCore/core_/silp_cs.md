# DECLARE_LIST(name, var_name, cs_type, list_name) #
```C#
private WeakList<${cs_type}> ${list_name} = null;

public int ${name}Count {
    get { return WeakListHelper.Count(${list_name}); }
}

public bool Add${name}(${cs_type} ${var_name}) {
    return WeakListHelper.Add(ref ${list_name}, ${var_name});
}

public bool Remove${name}(${cs_type} ${var_name}) {
    return WeakListHelper.Remove(${list_name}, ${var_name});
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
    if (Sealed && !key.StartsWith(VarPrefix)) {
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

# ASPECT_MIXIN(class) #
```
public readonly Entity Entity;

public readonly string Path;

public string RevPath {
    get {
        return string.Format("{0} ({1})", Path, Revision);
    }
}

public ${class}(Entity entity, string path, Pass pass) : base(pass) {
    Entity = entity;
    Path = path;
}

public override string GetLogPrefix() {
    return string.Format("{0}[{1}] {2} ", Entity.GetLogPrefix(), GetType().Name, RevPath);
}

public override bool DebugMode {
    get { return Entity.DebugMode; }
}

public override string[] DebugPatterns {
    get { return Entity.DebugPatterns; }
}

public virtual void OnAdded() {}
public virtual void OnRemoved() {}
```

# BLOCK_OWNER() #
```
private List<WeakBlock> _Blocks = null;

public void AddBlock(WeakBlock block) {
    if (_Blocks == null) {
        _Blocks = new List<WeakBlock>();
    }
    if (!_Blocks.Contains(block)) {
        _Blocks.Add(block);
    }
}

public void RemoveBlock(WeakBlock block) {
    if (_Blocks == null) {
        return;
    }
    int index = _Blocks.IndexOf(block);
    if (index >= 0) {
        _Blocks.RemoveAt(index);
    }
}
```
