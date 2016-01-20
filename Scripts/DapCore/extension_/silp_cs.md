# CONTEXT_PROPERTIES_HELPER(type, cs_type) #
```C#
public static ${type}Property Add${type}(this IContext context, string path, Pass propertyPass, ${cs_type} val) {
    return context.Properties.Add${type}(path, propertyPass, val);
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

public static bool Set${type}(this IContext context, string path, Pass propertyPass, ${cs_type} value) {
    return context.Properties.Set${type}(path, propertyPass, value);
}

public static bool Set${type}(this IContext context, string path, ${cs_type} value) {
    return context.Properties.Set${type}(path, value);
}

```

# TREE_PROPERTIES_HELPER(type, cs_type) #
```C#
public static ${type}Property Add${type}(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, ${cs_type} val) {
    ${type}Property v = properties.Add<${type}Property>(path, propertyPass);
    if (v != null && !v.Setup(propertyPass, val)) {
        properties.Remove<${type}Property>(path);
        v = null;
    }
    return v;
}

public static ${type}Property Add${type}(this ITreeProperties<IProperty> properties, string path, ${cs_type} val) {
    return properties.Add${type}(path, null, val);
}

public static ${type}Property Remove${type}(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {
    return properties.Remove<${type}Property>(path, propertyPass);
}

public static ${type}Property Remove${type}(this ITreeProperties<IProperty> properties, string path) {
    return properties.Remove<${type}Property>(path);
}

public static bool Add${type}ValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<${cs_type}> checker) {
    ${type}Property p = properties.Get<${type}Property>(path);
    if (p != null) {
        return p.AddValueChecker(propertyPass, checker);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return false;
}

public static bool Add${type}ValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<${cs_type}> checker) {
    return Add${type}ValueChecker(properties, path, null, checker);
}

public static bool Remove${type}ValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<${cs_type}> checker) {
    ${type}Property p = properties.Get<${type}Property>(path);
    if (p != null) {
        return p.RemoveValueChecker(propertyPass, checker);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return false;
}

public static bool Remove${type}ValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<${cs_type}> checker) {
    return Remove${type}ValueChecker(properties, path, null, checker);
}

public static BlockValueChecker<${cs_type}> Add${type}BlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,
                                    IBlockOwner owner, Func<IVar<${cs_type}>, ${cs_type}, bool> checker) {
    ${type}Property p = properties.Get<${type}Property>(path);
    if (p != null) {
        return p.AddBlockValueChecker(propertyPass, owner, checker);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return null;
}

public static BlockValueChecker<${cs_type}> Add${type}BlockValueChecker(this ITreeProperties<IProperty> properties, string path,
                                    IBlockOwner owner, Func<IVar<${cs_type}>, ${cs_type}, bool> checker) {
    return Add${type}BlockValueChecker(properties, path, null, owner, checker);
}

public static bool Add${type}ValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<${cs_type}> watcher) {
    ${type}Property p = properties.Get<${type}Property>(path);
    if (p != null) {
        return p.AddValueWatcher(watcher);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return false;
}

public static bool Remove${type}ValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<${cs_type}> watcher) {
    ${type}Property p = properties.Get<${type}Property>(path);
    if (p != null) {
        return p.RemoveValueWatcher(watcher);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return false;
}

public static BlockValueWatcher<${cs_type}> Add${type}BlockValueWatcher(this ITreeProperties<IProperty> properties, string path,
                                    IBlockOwner owner, Action<IVar<${cs_type}>, ${cs_type}> watcher) {
    ${type}Property p = properties.Get<${type}Property>(path);
    if (p != null) {
        return p.AddBlockValueWatcher(owner, watcher);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return null;
}

public static bool Is${type}(this ITreeProperties<IProperty> properties, string path) {
    return properties.Is<${type}Property>(path);
}

public static ${cs_type} Get${type}(this ITreeProperties<IProperty> properties, string path) {
    ${type}Property v = properties.Get<${type}Property>(path);
    if (v != null) {
        return v.Value;
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return default(${cs_type});
}

public static ${cs_type} Get${type}(this ITreeProperties<IProperty> properties, string path, ${cs_type} defaultValue) {
    ${type}Property v = properties.Get<${type}Property>(path);
    if (v != null) {
        return v.Value;
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return defaultValue;
}


public static bool Set${type}(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, ${cs_type} val) {
    ${type}Property v = properties.Get<${type}Property>(path);
    if (v != null) {
        return v.SetValue(propertyPass, val);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return false;
}

public static bool Set${type}(this ITreeProperties<IProperty> properties, string path, ${cs_type} val) {
    ${type}Property v = properties.Get<${type}Property>(path);
    if (v != null) {
        return v.SetValue(val);
    } else {
        properties.Error("Property Not Exist: {0}", path);
    }
    return false;
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

# EXTRA_SETUP_PROPERTY(type, cs_type) #
```C#
public static ${type}Property Setup${type}Property(this Extra ext,
        string fragment, Pass propertyPass, Func<${cs_type}> getter,
        Func<IVar<${cs_type}>, ${cs_type}, bool> checker,
        Action<IVar<${cs_type}>, ${cs_type}> watcher) {
    return ext.SetupProperty<${type}Property, ${cs_type}>(PropertiesConsts.Type${type}Property,
        fragment, propertyPass, getter,
        checker == null ? null : new BlockValueChecker<${cs_type}>(ext, checker),
        watcher == null ? null : new BlockValueWatcher<${cs_type}>(ext, watcher)
    );
}

public static ${type}Property Setup${type}Property(this Extra ext,
        string fragment, Pass propertyPass,
        Func<${cs_type}> getter,
        Action<IVar<${cs_type}>, ${cs_type}> watcher) {
    return Setup${type}Property(ext, fragment, propertyPass, getter, null, watcher);
}

public static ${type}Property Setup${type}Property(this Extra ext,
        string fragment, Pass propertyPass, Func<${cs_type}> getter) {
    return Setup${type}Property(ext, fragment, propertyPass, getter, null, null);
}

```

