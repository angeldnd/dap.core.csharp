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
