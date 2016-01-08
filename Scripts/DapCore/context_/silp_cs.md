# CONTEXT_PROPERTIES_HELPER(type, cs_type) #
```C#
public ${type}Property Add${type}(string path, Pass pass, ${cs_type} val) {
    return Properties.Add${type}(path, pass, val);
}

public ${type}Property Add${type}(string path, ${cs_type} val) {
    return Properties.Add${type}(path, val);
}

public ${type}Property Remove${type}(string path, Pass pass) {
    return Properties.Remove${type}(path, pass);
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

public bool Set${type}(string path, Pass pass, ${cs_type} value) {
    return Properties.Set${type}(path, pass, value);
}

public bool Set${type}(string path, ${cs_type} value) {
    return Properties.Set${type}(path, value);
}

```

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

# DECLARE_SECURE_LIST(name, var_name, cs_type, list_name) #
```C#
private WeakList<${cs_type}> ${list_name} = null;

public int ${name}Count {
    get { return WeakListHelper.Count(${list_name}); }
}

public bool Add${name}(Pass pass, ${cs_type} ${var_name}) {
    if (!CheckAdminPass(pass)) return false;
    return WeakListHelper.Add(ref ${list_name}, ${var_name});
}

public bool Add${name}(${cs_type} ${var_name}) {
    return Add${name}(null, ${var_name});
}

public bool Remove${name}(Pass pass, ${cs_type} ${var_name}) {
    if (!CheckAdminPass(pass)) return false;
    return WeakListHelper.Remove(${list_name}, ${var_name});
}

public bool Remove${name}(${cs_type} ${var_name}) {
    return Remove${name}(null, ${var_name});
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
public ${type}Property Add${type}(string path, Pass pass, ${cs_type} val) {
    ${type}Property v = Add<${type}Property>(path, pass);
    if (v != null && !v.Setup(pass, val)) {
        Remove<${type}Property>(path);
        v = null;
    }
    return v;
}

public ${type}Property Add${type}(string path, ${cs_type} val) {
    return Add${type}(path, null, val);
}

public ${type}Property Remove${type}(string path, Pass pass) {
    return Remove<${type}Property>(path, pass);
}

public ${type}Property Remove${type}(string path) {
    return Remove<${type}Property>(path);
}

public bool Add${type}ValueChecker(string path, Pass pass, ValueChecker<${cs_type}> checker) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddValueChecker(pass, checker);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Add${type}ValueChecker(string path, ValueChecker<${cs_type}> checker) {
    return Add${type}ValueChecker(path, checker);
}

public bool Remove${type}ValueChecker(string path, Pass pass, ValueChecker<${cs_type}> checker) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.RemoveValueChecker(pass, checker);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Remove${type}ValueChecker(string path, ValueChecker<${cs_type}> checker) {
    return Remove${type}ValueChecker(path, checker);
}

public ${type}BlockValueChecker Add${type}BlockValueChecker(string path, Pass pass,
                                    BlockOwner owner, ${type}BlockValueChecker.CheckerBlock checker) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddBlockValueChecker(pass, owner, checker);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return null;
}

public ${type}BlockValueChecker Add${type}BlockValueChecker(string path,
                                    BlockOwner owner, ${type}BlockValueChecker.CheckerBlock block) {
    return Add${type}BlockValueChecker(path, owner, block);
}

public bool Add${type}ValueWatcher(string path, ValueWatcher<${cs_type}> watcher) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddValueWatcher(watcher);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Remove${type}ValueWatcher(string path, ValueWatcher<${cs_type}> watcher) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.RemoveValueWatcher(watcher);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public ${type}BlockValueWatcher Add${type}BlockValueWatcher(string path,
                                    BlockOwner owner, ${type}BlockValueWatcher.WatcherBlock watcher) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddBlockValueWatcher(owner, watcher);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return null;
}

public bool Is${type}(string path) {
    return Get<${type}Property>(path) != null;
}

public ${cs_type} Get${type}(string path) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.Value;
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return default(${cs_type});
}

public ${cs_type} Get${type}(string path, ${cs_type} defaultValue) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.Value;
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return defaultValue;
}


public bool Set${type}(string path, ${cs_type} val) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.SetValue(val);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Set${type}(string path, Pass pass, ${cs_type} val) {
    ${type}Property v = Get<${type}Property>(path);
    if (v != null) {
        return v.SetValue(pass, val);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

```

# PROPERTY_CLASS(type, cs_type) #
```C#
public sealed class ${type}BlockValueChecker : WeakBlock, IValueChecker<${cs_type}> {
    public delegate bool CheckerBlock(string path, ${cs_type} val, ${cs_type} newVal);

    private readonly CheckerBlock _Block;

    public ${type}BlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {
        _Block = block;
    }

    public bool IsValid(string path, ${cs_type} val, ${cs_type} newVal) {
        return _Block(path, val, newVal);
    }
}

public sealed class ${type}BlockValueWatcher : WeakBlock, IValueWatcher<${cs_type}> {
    public delegate void WatcherBlock(string path, ${cs_type} val, ${cs_type} newVal);

    private readonly WatcherBlock _Block;

    public ${type}BlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {
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

    protected override bool DoDecode(Pass pass, Data data) {
        return SetValue(pass, data.Get${type}(PropertiesConsts.KeyValue));
    }

    protected override bool NeedUpdate(${cs_type} newVal) {
        return NeedSetup || (Value != newVal);
    }

    public ${type}BlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,
                                                         ${type}BlockValueChecker.CheckerBlock _checker) {
        if (!CheckAdminPass(pass)) return null;

        ${type}BlockValueChecker checker = new ${type}BlockValueChecker(owner, _checker);
        if (AddValueChecker(pass, checker)) {
            return checker;
        }
        return null;
    }

    public ${type}BlockValueChecker AddBlockValueChecker(BlockOwner owner,
                                                         ${type}BlockValueChecker.CheckerBlock checker) {
        return AddBlockValueChecker(null, checker);
    }

    public ${type}BlockValueWatcher AddBlockValueWatcher(BlockOwner owner,
                                                         ${type}BlockValueWatcher.WatcherBlock _watcher) {
        ${type}BlockValueWatcher watcher = new ${type}BlockValueWatcher(owner, _watcher);
        if (AddValueWatcher(watcher)) {
            return watcher;
        }
        return null;
    }
}

```

# CONTEXT_DEPOSIT_WITHDRAW(name, type, vars, var) #
```C#
public ${type} Deposit${name}(string key, ${type} ${var}) {
    string varPath = ContextConsts.GetVarPath(${vars}, key);
    return Vars.DepositValue<${type}>(varPath, null, ${var});
}

public ${type} Withdraw${name}(string key) {
    string varPath = ContextConsts.GetVarPath(${vars}, key);
    return Vars.WithdrawValue<${type}>(varPath);
}

```

# EXTENSION_SETUP_PROPERTY(type, cs_type) #
```C#
public ${type}Property Setup${type}Property(string fragment,
        Pass pass, ${type}Property.GetterBlock getter,
        ${type}BlockValueChecker.CheckerBlock checker,
        ${type}BlockValueWatcher.WatcherBlock watcher) {
    return SetupProperty<${cs_type}>(PropertiesConsts.Type${type}Property,
        fragment, pass, getter,
        checker == null ? null : new ${type}BlockValueChecker(this, checker),
        watcher == null ? null : new ${type}BlockValueWatcher(this, watcher)
    ) as ${type}Property;
}

public ${type}Property Setup${type}Property(string fragment, Pass pass,
        ${type}Property.GetterBlock getter) {
    return Setup${type}Property(fragment, pass, getter, null, null);
}

public ${type}Property Setup${type}Property(string fragment, Pass pass,
        ${type}Property.GetterBlock getter,
        ${type}BlockValueWatcher.WatcherBlock watcher) {
    return Setup${type}Property(fragment, pass, getter, null, watcher);
}

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
