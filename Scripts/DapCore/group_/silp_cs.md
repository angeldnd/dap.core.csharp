# GROUP_PROPERTY_MIXIN(class) #
```
public ${class}(IDictProperties owner, string key) : base(owner, key) {
}

public ${class}(ITableProperties owner, int index) : base(owner, index) {
}

//IProperty
public Data Encode() {
    if (!string.IsNullOrEmpty(DapType)) {
        Data data = EncodeValue(true);
        if (data.SetString(ObjectConsts.KeyDapType, DapType)) {
            return data;
        }
    }
    if (LogDebug) Debug("Not Encodable!");
    return null;
}

private Data EncodeValue(bool fullMode) {
    Data data = new Data();
    if (data.SetData(PropertiesConsts.KeyValue, DoEncodeValue(fullMode))) {
        return data;
    }
    return null;
}

public Data EncodeValue() {
    return EncodeValue(false);
}

private bool DecodeValue(bool fullMode, Data data) {
    if (data == null) return false;
    Data v = data.GetData(PropertiesConsts.KeyValue);
    if (v == null) return false;

    return DoDecodeValue(fullMode, v);
}

public bool DecodeValue(Data data) {
    return DecodeValue(false, data);
}

public bool Decode(Data data) {
    if (data == null) return false;
    string dapType = data.GetString(ObjectConsts.KeyDapType);
    if (dapType == DapType) {
        return DecodeValue(true, data);
    } else {
        Error("Dap Type Mismatched: {0}, {1}", DapType, dapType);
    }
    return false;
}

public void FireOnChanged() {
    WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {
        watcher.OnChanged(this);
    });
}

//IVar
public object GetValue() {
    Error("Not Supported");
    return null;
}

public bool SetValue(object newValue) {
    Error("Not Supported");
    return false;
}

private WeakList<IVarWatcher> _VarWatchers = null;

public int VarWatcherCount {
    get { return WeakListHelper.Count(_VarWatchers); }
}

public bool AddVarWatcher(IVarWatcher watcher) {
    if (WeakListHelper.Add(ref _VarWatchers, watcher)){
        CheckWatcherWrapper();
        return true;
    }
    return false;
}

public bool RemoveVarWatcher(IVarWatcher watcher) {
    if (WeakListHelper.Remove(_VarWatchers, watcher)) {
        return true;
    }
    return false;
}

public BlockVarWatcher AddVarWatcher(IBlockOwner owner,
                                     Action<IVar> _watcher) {
    BlockVarWatcher watcher = new BlockVarWatcher(owner, _watcher);
    if (AddVarWatcher(watcher)) {
        return watcher;
    }
    return null;
}

public int ValueCheckerCount {
    get { return 0; }
}

public void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker {
}

public int ValueWatcherCount {
    get { return 0; }
}
```

# GROUP_ENCODE_MIXIN(element_type) #
```
private bool DoEncode(Data data) {
    Data values = new Data();
    if (!data.SetData(PropertiesConsts.KeyValue, values)) return false;

    return UntilFalse((${element_type} element) => {
        Data subData = element.Encode();
        return subData != null && values.SetData(element.Key, subData);
    });
}

public Data EncodeValue() {
    Data data = new Data();
    Data values = new Data();
    if (!data.SetData(PropertiesConsts.KeyValue, values)) return false;

    return UntilFalse((${element_type} element) => {
        Data subData = element.EncodeValue();
        return subData != null && values.SetData(element.Key, subData);
    });
    return data;
}
```
