# WEAK_DATA_TYPE(type, cs_type) #
```
public override bool Is${type}(string key) {
    return _Real.Is${type}(key);
}

public override bool TryGet${type}(string key, out ${cs_type} val, bool isDebug = false) {
    return _Real.TryGet${type}(key, out val, isDebug);
}

public override ${cs_type} Get${type}(string key) {
    return _Real.Get${type}(key);
}

public override ${cs_type} Get${type}(string key, ${cs_type} defaultValue) {
    return _Real.Get${type}(key, defaultValue);
}

public override bool Set${type}(string key, ${cs_type} val) {
    return _Real.Set${type}(key, val);
}

public override void ForEach${type}(Action<int, ${cs_type}> callback) {
    _Real.ForEach${type}(callback);
}

public override bool UntilTrue${type}(Func<int, ${cs_type}, bool> callback) {
    return _Real.UntilTrue${type}(callback);
}

public override bool UntilFalse${type}(Func<int, ${cs_type}, bool> callback) {
    return _Real.UntilFalse${type}(callback);
}

```
