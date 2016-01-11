# CONTEXT_PROPERTIES_HELPER(type, cs_type) #
```C#
public static ${type}Property Add${type}(this IContext context, string path, Pass pass, ${cs_type} val) {
    return context.Properties.Add${type}(path, pass, val);
}

public static ${type}Property Add${type}(this IContext context, string path, ${cs_type} val) {
    return context.Properties.Add${type}(path, val);
}

public static ${type}Property Remove${type}(this IContext context, string path, Pass pass) {
    return context.Properties.Remove${type}(path, pass);
}

public static ${type}Property Remove${type}(this IContext context, string path) {
    return context.Properties.Remove${type}(path);
}

public static bool Is${type}(this IContext context, string path) {
    return context.Properties.Is${type}(path);
}

public static ${cs_type} Get${type}(this IContext context, string path) {
    return context.Properties.Get${type}(path);
}

public static ${cs_type} Get${type}(this IContext context, string path, ${cs_type} defaultValue) {
    return context.Properties.Get${type}(path, defaultValue);
}

public static bool Set${type}(this IContext context, string path, Pass pass, ${cs_type} value) {
    return context.Properties.Set${type}(path, pass, value);
}

public static bool Set${type}(this IContext context, string path, ${cs_type} value) {
    return context.Properties.Set${type}(path, value);
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

public bool Add${type}ValueChecker(string path, Pass pass, IValueChecker<${cs_type}> checker) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddValueChecker(pass, checker);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Add${type}ValueChecker(string path, IValueChecker<${cs_type}> checker) {
    return Add${type}ValueChecker(path, checker);
}

public bool Remove${type}ValueChecker(string path, Pass pass, IValueChecker<${cs_type}> checker) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.RemoveValueChecker(pass, checker);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Remove${type}ValueChecker(string path, IValueChecker<${cs_type}> checker) {
    return Remove${type}ValueChecker(path, checker);
}

public BlockValueChecker<${cs_type}> Add${type}BlockValueChecker(string path, Pass pass,
                                    IBlockOwner owner, Func<IVar<${cs_type}>, ${cs_type}, bool> checker) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddBlockValueChecker(pass, owner, checker);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return null;
}

public BlockValueChecker<${cs_type}> Add${type}BlockValueChecker(string path,
                                    IBlockOwner owner, Func<IVar<${cs_type}>, ${cs_type}, bool> checker) {
    return Add${type}BlockValueChecker(path, owner, checker);
}

public bool Add${type}ValueWatcher(string path, IValueWatcher<${cs_type}> watcher) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.AddValueWatcher(watcher);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public bool Remove${type}ValueWatcher(string path, IValueWatcher<${cs_type}> watcher) {
    ${type}Property p = Get<${type}Property>(path);
    if (p != null) {
        return p.RemoveValueWatcher(watcher);
    } else {
        Error("Property Not Exist: {0}", path);
    }
    return false;
}

public BlockValueWatcher<${cs_type}> Add${type}BlockValueWatcher(string path,
                                    IBlockOwner owner, Action<IVar<${cs_type}>, ${cs_type}> watcher) {
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
public class ${type}Property : Property<${cs_type}> {
    public override string Type {
        get { return PropertiesConsts.Type${type}Property; }
    }

    public ${type}Property(Properties owner, string path, Pass pass) : base(owner, path, pass) {
    }

    protected override bool DoEncode(Data data) {
        return data.Set${type}(PropertiesConsts.KeyValue, Value);
    }

    protected override bool DoDecode(Pass pass, Data data) {
        return SetValue(pass, data.Get${type}(PropertiesConsts.KeyValue));
    }

    protected override bool NeedUpdate(${cs_type} newVal) {
        return base.NeedUpdate(newVal) || (Value != newVal);
    }
}

```

# CONTEXT_DEPOSIT_WITHDRAW(name, type, vars, var) #
```C#
public static ${type} Deposit${name}(this IContext context, string key, ${type} ${var}) {
    string varPath = ContextConsts.GetVarPath(${vars}, key);
    return context.Vars.DepositValue<${type}>(varPath, null, ${var});
}

public static ${type} Withdraw${name}(this IContext context, string key) {
    string varPath = ContextConsts.GetVarPath(${vars}, key);
    return context.Vars.WithdrawValue<${type}>(varPath);
}

```

# EXTENSION_SETUP_PROPERTY(type, cs_type) #
```C#
public ${type}Property Setup${type}Property(string fragment,
        Pass pass, Func<${cs_type}> getter,
        Func<IVar<${cs_type}>, ${cs_type}, bool> checker,
        Action<IVar<${cs_type}>, ${cs_type}> watcher) {
    return SetupProperty<${type}Property, ${cs_type}>(PropertiesConsts.Type${type}Property,
        fragment, pass, getter,
        checker == null ? null : new BlockValueChecker<${cs_type}>(this, checker),
        watcher == null ? null : new BlockValueWatcher<${cs_type}>(this, watcher)
    ) as ${type}Property;
}

public ${type}Property Setup${type}Property(string fragment, Pass pass,
        Func<${cs_type}> getter) {
    return Setup${type}Property(fragment, pass, getter, null, null);
}

public ${type}Property Setup${type}Property(string fragment, Pass pass,
        Func<${cs_type}> getter,
        Action<IVar<${cs_type}>, ${cs_type}> watcher) {
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
