# CONTEXT_PROPERTIES_HELPER(type, cs_type) #
```C#
public ${type}Property Add${type}(string path, Object pass, ${cs_type} val) {
    return Properties.Add${type}(path, pass, val);
}

public ${type}Property Add${type}(string path, ${cs_type} val) {
    return Properties.Add${type}(path, val);
}

public ${type}Property Remove${type}(string path) {
    return Properties.Remove${type}(path);
}

public bool Is${type}(string path) {
    return Properties.Is${type}(path);
}

public ${cs_type} Get${type}(string path) {
    return Properties.Get${type}(path);
}

public ${cs_type} Get${type}(string path, ${cs_type} defaultValue) {
    return Properties.Get${type}(path, defaultValue);
}

public bool Set${type}(string path, ${cs_type} value) {
    return Properties.Set${type}(path, value);
}

public bool Set${type}(string path, Object pass, ${cs_type} value) {
    return Properties.Set${type}(path, pass, value);
}

```

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

# ADD_REMOVE_HELPER(name, a_path, a_var, a_type, l_name, l_var, l_type) #
```C#
public bool Add${name}(string ${a_path}, ${l_type} ${l_var}) {
    ${a_type} ${a_var} = Get<${a_type}>(${a_path});
    if (${a_var} != null) {
        return ${a_var}.Add${l_name}(${l_var});
    } else {
        Error("${a_type} Not Found: {0}", ${a_path});
    }
    return false;
}

public bool Remove${name}(string ${a_path}, ${l_type} ${l_var}) {
    ${a_type} ${a_var} = Get<${a_type}>(${a_path});
    if (${a_var} != null) {
        return ${a_var}.Remove${l_name}(${l_var});
    } else {
        Error("${a_type} Not Found: {0}", ${a_path});
    }
    return false;
}

```

# PROPERTIES_HELPER(type, cs_type) #
```C#
public ${type}Property Add${type}(string path, Object pass, ${cs_type} val) {
    ${type}Property v = Add<${type}Property>(path);
    if (v != null && !v.Setup(pass, val)) {
        Remove<${type}Property>(path);
        v = null;
    }
    return v;
}

public ${type}Property Add${type}(string path, ${cs_type} val) {
    return Add${type}(path, null, val);
}

public ${type}Property Remove${type}(string path) {
    return Remove<${type}Property>(path);
}

public bool Add${type}ValueChecker(string path, ValueChecker<${cs_type}> checker) {
     ${type}Property p = Get<${type}Property>(path);
     if (p != null) {
        return p.AddValueChecker(checker);
     }
     return false;
}

public bool Remove${type}ValueChecker(string path, ValueChecker<${cs_type}> checker) {
     ${type}Property p = Get<${type}Property>(path);
     if (p != null) {
        return p.RemoveValueChecker(checker);
     }
     return false;
}

public bool Add${type}BlockValueChecker(string path, ${type}BlockValueChecker.CheckerBlock block) {
    ${type}BlockValueChecker checker = new ${type}BlockValueChecker(block);
    return Add${type}ValueChecker(path, checker);
}

public bool Add${type}ValueWatcher(string path, ValueWatcher<${cs_type}> watcher) {
     ${type}Property p = Get<${type}Property>(path);
     if (p != null) {
        return p.AddValueWatcher(watcher);
     }
     return false;
}

public bool Remove${type}ValueWatcher(string path, ValueWatcher<${cs_type}> watcher) {
     ${type}Property p = Get<${type}Property>(path);
     if (p != null) {
        return p.RemoveValueWatcher(watcher);
     }
     return false;
}

public bool Add${type}BlockValueWatcher(string path, ${type}BlockValueWatcher.WatcherBlock block) {
    ${type}BlockValueWatcher watcher = new ${type}BlockValueWatcher(block);
    return Add${type}ValueWatcher(path, watcher);
}
 
public bool Is${type}(string path) {
    return Get<${type}Property>(path) != null;
}

public ${cs_type} Get${type}(string path) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.Value;
    }
    return default(${cs_type});
}

public ${cs_type} Get${type}(string path, ${cs_type} defaultValue) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.Value;
    }
    return defaultValue;
}


public bool Set${type}(string path, ${cs_type} val) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.SetValue(val);
    }
    return false;
}

public bool Set${type}(string path, Object pass, ${cs_type} val) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.SetValue(pass, val);
    }
    return false;
}

```

# PROPERTY_CLASS(type, cs_type) #
```C#
public sealed class ${type}BlockValueChecker : ValueChecker<${cs_type}> {
    public delegate bool CheckerBlock(string path, ${cs_type} val, ${cs_type} newVal);

    private readonly CheckerBlock _Block;

    public ${type}BlockValueChecker(CheckerBlock block) {
        _Block = block;
    }

    public bool IsValid(string path, ${cs_type} val, ${cs_type} newVal) {
        return _Block(path, val, newVal);
    }
}

public sealed class ${type}BlockValueWatcher : ValueWatcher<${cs_type}> {
    public delegate void WatcherBlock(string path, ${cs_type} val, ${cs_type} newVal);

    private readonly WatcherBlock _Block;

    public ${type}BlockValueWatcher(WatcherBlock block) {
        _Block = block;
    }

    public void OnChanged(string path, ${cs_type} lastVal, ${cs_type} val) {
        _Block(path, lastVal, val);
    }
}

public class ${type}Property : Property<${cs_type}> {
    public override string Type {
        get { return PropertiesConsts.Type${type}Property; }
    }
        
    protected override bool DoEncode(Data data) {
        return data.Set${type}(PropertiesConsts.KeyValue, Value);
    }

    protected override bool DoDecode(Data data) {
        SetValue(data.Get${type}(PropertiesConsts.KeyValue));
        return true;
    }

    private bool _CheckingValue = false;
    private bool _UpdatingValue = false;
    
    public override bool SetValue(${cs_type} newVal) {
        if (_CheckingValue) return false;
        if (_UpdatingValue) return false;

        if (Value != newVal) {
            if (_Checkers != null) {
                _CheckingValue = true;
                for (int i = 0; i < _Checkers.Count; i++) {
                    if (!_Checkers[i].IsValid(Path, Value, newVal)) {
                        _CheckingValue = false;
                        return false;
                    }
                }
                _CheckingValue = false;
            }
            _UpdatingValue = true;
            ${cs_type} lastVal = Value;
            if (!base.SetValue(newVal)) {
                _UpdatingValue = false;
                return false;
            }
            if (_Watchers != null) {
                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnChanged(Path, lastVal, Value);
                }
            }
            _UpdatingValue = false;
            return true;
        }
        return false;
    }

    public bool AddBlockValueChecker(${type}BlockValueChecker.CheckerBlock block) {
        ${type}BlockValueChecker checker = new ${type}BlockValueChecker(block);
        return AddValueChecker(checker);
    }

    public bool AddBlockValueWatcher(${type}BlockValueWatcher.WatcherBlock block) {
        ${type}BlockValueWatcher watcher = new ${type}BlockValueWatcher(block);
        return AddValueWatcher(watcher);
    }
}

```
