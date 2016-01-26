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

# ELEMENT_MIXIN(class) #
```
protected ${class}(TO owner) {
    _Owner = owner;
}

private readonly TO _Owner;
public TO Owner {
    get { return _Owner; }
}
public IOwner GetOwner() {
    return _Owner;
}

public override string RevInfo {
    get {
        if (Key != null) {
            return string.Format("[{0}] ({1}) ", Key, Revision);
        } else {
            return base.RevInfo;
        }
    }
}

public override string LogPrefix {
    get {
        return string.Format("{0}{1}",
                Owner.LogPrefix, base.LogPrefix);
    }
}

public override bool DebugMode {
    get { return Owner.DebugMode; }
}

public override string[] DebugPatterns {
    get { return Owner.DebugPatterns; }
}

public virtual string Key {
    get { return null; }
}

public virtual void OnRemoved() {}
```

# ELEMENT_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner) : base(owner) {
}
```

# IN_DICT_MIXIN_KEY() #
```
private readonly string _Key;
public override string Key {
    get { return _Key; }
}
```

# IN_DICT_MIXIN(class) #
```
protected ${class}(TO owner, string key) : base(owner) {
    _Key = key;
}

public IDict OwnerAsDict {
    get { return Owner; }
}

//SILP:IN_DICT_MIXIN_KEY()
```

# IN_DICT_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, string key) : base(owner, key) {
}
```

# IN_TABLE_MIXIN_INDEX() #
```
private int _Index = -1;
public int Index {
    get { return _Index; }
}

public bool SetIndex(IOwner owner, int index) {
    if (Owner != owner) return false;

    _Index = index;
    return true;
}
```

# IN_TABLE_MIXIN(class) #
```
protected ${class}(TO owner, int index) : base(owner) {
    _Index = index;
}

public ITable OwnerAsTable {
    get { return Owner; }
}

//SILP:IN_TABLE_MIXIN_INDEX()

public override string Key {
    get {
        return _Index.ToString();
    }
}
```

# IN_TABLE_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, int index) : base(owner, index) {
}
```

# IN_BOTH_MIXIN_CONSTRUCTOR(class) #
```
//SILP:IN_DICT_MIXIN_CONSTRUCTOR(${class})

//SILP:IN_TABLE_MIXIN_CONSTRUCTOR(${class})
```

# IN_BOTH_MIXIN(class) #
```
protected ${class}(TO owner, string key) : base(owner) {
    if (owner is IDict) {
        _Key = key;
    }
}

protected ${class}(TO owner, int index) : base(owner) {
    if (owner is ITable) {
        _Index = index;
    }
}

public IDict OwnerAsDict {
    get { return Owner as IDict; }
}

public ITable OwnerAsTable {
    get { return Owner as ITable; }
}

//SILP: IN_TABLE_MIXIN_INDEX()

private readonly string _Key;
public override string Key {
    get {
        if (_Key != null) {
            return _Key;
        } else {
            return _Index.ToString();
        }
    }
}
```

# ASPECT_MIXIN() #
```
public IContext GetContext() {
    return Owner.GetContext();
}

public IContext Context {
    get { return Owner.GetContext(); }
}
```

# CONTEXT_MIXIN() #
```
    _Path = ContextConsts.GetContextPath(ContextExtension.GetKeys(this));

    _Properties = new Properties(this);
    _Channels = new Channels(this);
    _Handlers = new Handlers(this);
    _Vars = new Vars(this);
    _Manners = new Manners(this);
}

private string _Path;
public string Path {
    get { return _Path; }
}

private readonly Properties _Properties;
public Properties Properties {
    get { return _Properties; }
}

private readonly Channels _Channels;
public Channels Channels {
    get { return _Channels; }
}

private readonly Handlers _Handlers;
public Handlers Handlers {
    get { return _Handlers; }
}

private readonly Vars _Vars;
public Vars Vars {
    get { return _Vars; }
}

private readonly Manners _Manners;
public Manners Manners {
    get { return _Manners; }
}

public IContext GetContext() {
    return this;
}

private bool _DebugMode = false;
public override bool DebugMode {
    get { return _DebugMode; }
}
public void SetDebugMode(bool debugMode) {
    _DebugMode= debugMode;
}

private string[] _DebugPatterns = null;
public override string[] DebugPatterns {
    get { return _DebugPatterns; }
}
public void SetDebugPatterns(string[] patterns) {
    _DebugPatterns = patterns;
}
```
