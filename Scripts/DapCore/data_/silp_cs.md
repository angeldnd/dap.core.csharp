# DATA_TYPE(type, cs_type) #
```
public abstract bool Is${type}(string key);
public abstract bool TryGet${type}(string key, out ${cs_type} val, bool isDebug = false);
public abstract ${cs_type} Get${type}(string key);
public abstract ${cs_type} Get${type}(string key, ${cs_type} defaultValue);
public abstract bool Set${type}(string key, ${cs_type} val);
public abstract void ForEach${type}(Action<int, ${cs_type}> callback);
public abstract bool UntilTrue${type}(Func<int, ${cs_type}, bool> callback);
public abstract bool UntilFalse${type}(Func<int, ${cs_type}, bool> callback);

```

# DATA_QUICK_SETTER(name, type, cs_type) #
```
public Data ${name}(string key, ${cs_type} val) {
    Set${type}(key, val);
    return this;
}

```

# REAL_DATA_TYPE(type, cs_type) #
```
public override bool Is${type}(string key) {
    DataType type = GetValueType(key);
    return type == DataType.${type};
}

public override bool TryGet${type}(string key, out ${cs_type} val, bool isDebug = false) {
    if (_${type}Values == null) {
        Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);
    } else if (!Is${type}(key)) {
        Log.ErrorOrDebug(isDebug, "Value Is Not ${type}: {0} -> {1} -> {2}",
            key, GetValueType(key), GetValue(key));
    } else {
        ${cs_type} _val;
        if (_${type}Values.TryGetValue(key, out _val)) {
            val = _val;
            return true;
        } else {
            Log.Error("Value Not Found: {0}", key);
        }
    }
    val = default(${cs_type});
    return false;
}

public override ${cs_type} Get${type}(string key) {
    ${cs_type} result;
    TryGet${type}(key, out result);
    return result;
}

public override ${cs_type} Get${type}(string key, ${cs_type} defaultValue) {
    ${cs_type} result;
    if (TryGet${type}(key, out result, true)) {
        return result;
    }
    return defaultValue;
}

public override bool Set${type}(string key, ${cs_type} val) {
    bool isTempKey = IsTempKey(key);
    if (Sealed && !isTempKey) {
        Log.Error("Already Sealed: {0} -> {1}", key, val);
        return false;
    }
    if (isTempKey || !_ValueTypes.ContainsKey(key)) {
        _ValueTypes[key] = DataType.${type};
        if (_${type}Values == null) {
            _${type}Values = new Dictionary<string, ${cs_type}>();
        }
        _${type}Values[key] = val;
        return true;
    }
    Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);
    return false;
}

public override void ForEach${type}(Action<int, ${cs_type}> callback) {
    for (int i = 0; i < Count; i++) {
        string key = i.ToString();
        callback(i, Get${type}(key));
    }
}

public override bool UntilTrue${type}(Func<int, ${cs_type}, bool> callback) {
    for (int i = 0; i < Count; i++) {
        string key = i.ToString();
        if (callback(i, Get${type}(key))) {
            return true;
        }
    }
    return false;
}

public override bool UntilFalse${type}(Func<int, ${cs_type}, bool> callback) {
    for (int i = 0; i < Count; i++) {
        string key = i.ToString();
        if (!callback(i, Get${type}(key))) {
            return false;
        }
    }
    return true;
}


```
