# GROUP_PROPERTY_MIXIN(class) #
```
public ${class}(IProperties owner, string path, Pass pass) : base(owner, path, pass) {
}

public ${class}(IProperties owner, int index, Pass pass) : base(owner, index, pass) {
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

public bool Decode(Pass pass, Data data) {
    if (!CheckWritePass(pass)) return false;

    string type = data.GetString(ObjectConsts.KeyType);
    if (type == Type) {
        return DoDecode(pass, data);
    } else {
        Error("Type Mismatched: {0}, {1}", Type, type);
    }
    return false;
}

public bool Decode(Data data) {
    return Decode(null, data);
}

private void FireOnChanged() {
    WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {
        watcher.OnChanged(this);
    });
}

public BlockVarWatcher AddBlockVarWatcher(IBlockOwner owner,
                                                    Action<IVar> _watcher) {
    BlockVarWatcher watcher = new BlockVarWatcher(owner, _watcher);
    if (AddVarWatcher(watcher)) {
        return watcher;
    }
    return null;
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
        //_Properties.ResetAllVarWatchers(_PropertiesPass);
        return true;
    }
    return false;
}

public bool RemoveVarWatcher(IVarWatcher watcher) {
    if (WeakListHelper.Remove(_VarWatchers, watcher)) {
        //_Properties.ResetAllVarWatchers(_PropertiesPass);
        return true;
    }
    return false;
}
```

