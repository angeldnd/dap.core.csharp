# DICT_PROPERTIES_HELPER(type, cs_type) #
```C#
public static ${type}Property Add${type}(this IDictProperties properties, string key, ${cs_type} val) {
    ${type}Property v = properties.Add<${type}Property>(key);
    if (v != null && !v.Setup(val)) {
        properties.Remove<${type}Property>(key);
        v = null;
    }
    return v;
}

public static ${type}Property Remove${type}(this IDictProperties properties, string key) {
    return properties.Remove<${type}Property>(key);
}

public static bool Add${type}ValueChecker(this IDictProperties properties, string key, IValueChecker<${cs_type}> checker) {
    ${type}Property p = properties.Get<${type}Property>(key);
    if (p != null) {
        return p.AddValueChecker(checker);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return false;
}

public static bool Remove${type}ValueChecker(this IDictProperties properties, string key, IValueChecker<${cs_type}> checker) {
    ${type}Property p = properties.Get<${type}Property>(key);
    if (p != null) {
        return p.RemoveValueChecker(checker);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return false;
}

public static BlockValueChecker<${cs_type}> Add${type}ValueChecker(this IDictProperties properties, string key,
                                    IBlockOwner owner, Func<IVar<${cs_type}>, ${cs_type}, bool> block) {
    ${type}Property p = properties.Get<${type}Property>(key);
    if (p != null) {
        return p.AddValueChecker(owner, block);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return null;
}

public static bool Add${type}ValueWatcher(this IDictProperties properties, string key, IValueWatcher<${cs_type}> watcher) {
    ${type}Property p = properties.Get<${type}Property>(key);
    if (p != null) {
        return p.AddValueWatcher(watcher);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return false;
}

public static bool Remove${type}ValueWatcher(this IDictProperties properties, string key, IValueWatcher<${cs_type}> watcher) {
    ${type}Property p = properties.Get<${type}Property>(key);
    if (p != null) {
        return p.RemoveValueWatcher(watcher);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return false;
}

public static BlockValueWatcher<${cs_type}> Add${type}ValueWatcher(this IDictProperties properties, string key,
                                    IBlockOwner owner, Action<IVar<${cs_type}>, ${cs_type}> block) {
    ${type}Property p = properties.Get<${type}Property>(key);
    if (p != null) {
        return p.AddValueWatcher(owner, block);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return null;
}

public static bool Is${type}(this IDictProperties properties, string key) {
    return properties.Is<${type}Property>(key);
}

public static ${cs_type} Get${type}(this IDictProperties properties, string key) {
    ${type}Property v = properties.Get<${type}Property>(key);
    if (v != null) {
        return v.Value;
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return default(${cs_type});
}

public static ${cs_type} Get${type}(this IDictProperties properties, string key, ${cs_type} defaultValue) {
    ${type}Property v = properties.Get<${type}Property>(key, true);
    if (v != null) {
        return v.Value;
    } else {
        properties.Debug("Property Not Exist: {0}", key);
    }
    return defaultValue;
}

public static bool Set${type}(this IDictProperties properties, string key, ${cs_type} val) {
    ${type}Property v = properties.Get<${type}Property>(key);
    if (v != null) {
        return v.SetValue(val);
    } else {
        properties.Error("Property Not Exist: {0}", key);
    }
    return false;
}
```

# VARS_DEPOSIT_WITHDRAW(name, type, vars, var) #
```C#
public static ${type} Deposit${name}(this Vars vars, string key, ${type} ${var}) {
    string varKey = ContextConsts.GetAspectKey(${vars}, key);
    return vars.DepositValue<${type}>(varKey, null, ${var});
}

public static ${type} Withdraw${name}(this Vars vars, string key) {
    string varKey = ContextConsts.GetAspectKey(${vars}, key);
    return vars.WithdrawValue<${type}>(varKey);
}

```

# EXTRA_SETUP_PROPERTY(type, cs_type) #
```C#
public static ${type}Property Setup${type}Property(this Extra ext,
        string fragment, Func<${cs_type}> getter,
        Func<IVar<${cs_type}>, ${cs_type}, bool> checker,
        Action<IVar<${cs_type}>, ${cs_type}> watcher) {
    return ext.SetupProperty<${type}Property, ${cs_type}>(PropertiesConsts.Type${type}Property,
        fragment, getter,
        checker == null ? null : new BlockValueChecker<${cs_type}>(ext, checker),
        watcher == null ? null : new BlockValueWatcher<${cs_type}>(ext, watcher)
    );
}

public static ${type}Property Setup${type}Property(this Extra ext,
        string fragment,
        Func<${cs_type}> getter,
        Action<IVar<${cs_type}>, ${cs_type}> watcher) {
    return Setup${type}Property(ext, fragment, getter, null, watcher);
}

public static ${type}Property Setup${type}Property(this Extra ext,
        string fragment, Func<${cs_type}> getter) {
    return Setup${type}Property(ext, fragment, getter, null, null);
}

```

