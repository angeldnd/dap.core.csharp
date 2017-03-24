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
    Log.Error("Key Exist: {0} {1} -> {2}", key, GetValue(key), val);
    return false;
}

public void ForEach${type}(Action<int, ${cs_type}> callback) {
    for (int i = 0; i < Count; i++) {
        string key = i.ToString();
        callback(i, Get${type}(key));
    }
}

public bool UntilTrue${type}(Func<int, ${cs_type}, bool> callback) {
    for (int i = 0; i < Count; i++) {
        string key = i.ToString();
        if (callback(i, Get${type}(key))) {
            return true;
        }
    }
    return false;
}

public bool UntilFalse${type}(Func<int, ${cs_type}, bool> callback) {
    for (int i = 0; i < Count; i++) {
        string key = i.ToString();
        if (!callback(i, Get${type}(key))) {
            return false;
        }
    }
    return true;
}


``` 

# DATA_QUICK_SETTER(name, type, cs_type) #
```
public Data ${name}(string key, ${cs_type} val) {
    Set${type}(key, val);
    return this;
}

```
