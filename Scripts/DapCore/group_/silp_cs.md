# GROUP_PROPERTY_MIXIN(class) #
```
public ${class}(IDictProperties owner, string key) : base(owner, key) {
}

public ${class}(ITableProperties owner, int index) : base(owner, index) {
}

//IProperty
public Data Encode() {
    if (!string.IsNullOrEmpty(Type)) {
        Data data = new Data();
        if (data.SetString(ObjectConsts.KeyType, Type)) {
            if (DoEncode(data)) {
                return data;
            }
        }
    }
    if (LogDebug) Debug("Not Encodable!");
    return null;
}

public bool Decode(Data data) {
    string type = data.GetString(ObjectConsts.KeyType);
    if (type == Type) {
        return DoDecode(data);
    } else {
        Error("Type Mismatched: {0}, {1}", Type, type);
    }
    return false;
}

private void FireOnChanged() {
    WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {
        watcher.OnChanged(this);
    });
}

//IVar
public object GetValue() {
    return null;
}

private WeakList<IVarWatcher> _VarWatchers = null;

public int VarWatcherCount {
    get { return WeakListHelper.Count(_VarWatchers); }
}

public bool AddVarWatcher(IVarWatcher watcher) {
    if (WeakListHelper.Add(ref _VarWatchers, watcher)){
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

