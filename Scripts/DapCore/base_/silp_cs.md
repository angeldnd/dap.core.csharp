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
protected ${class}(TO owner, string key) {
    _Owner = owner;
    _Key = key;
}

protected ${class}(TO owner) {
    _Owner = owner;
    _Key = string.Format("{0}", Guid.NewGuid().GetHashCode());
}

private readonly TO _Owner;
public TO Owner {
    get { return _Owner; }
}
public IOwner GetOwner() {
    return _Owner;
}

private readonly string _Key;
public string Key {
    get { return _Key; }
}

public override bool DebugMode {
    get { return _Owner == null ? false : _Owner.DebugMode; }
}

public virtual void OnAdded() {}
public virtual void OnRemoved() {}
```

# ASPECT_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, string key) : base(owner, key) {
    _Context = owner == null ? null : owner.GetContext();
    _Path = Env.GetAspectPath(this);
    _Uri = Env.GetAspectUri(this);
}
```

# IN_DICT_MIXIN(class) #
```
protected ${class}(TO owner, string key) : base(owner, key) {
}

public IDict OwnerAsDict {
    get { return Owner; }
}
```

# IN_DICT_ASPECT_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, string key) : base(owner, key) {
    _Context = owner == null ? null : owner.GetContext();
    _Path = Env.GetAspectPath(this);
    _Uri = Env.GetAspectUri(this);
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

public override string RevInfo {
    get {
        return string.Format("[{0}] ({1})", _Index, Revision);
    }
}
```

# IN_TABLE_ASPECT_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, int index) : base(owner, index) {
    _Context = owner == null ? null : owner.GetContext();
    _Path = Env.GetAspectPath(this);
    _Uri = Env.GetAspectUri(this);
}
```

# IN_BOTH_ASPECT_MIXIN_CONSTRUCTOR(class) #
```
//SILP:IN_DICT_ASPECT_MIXIN_CONSTRUCTOR(${class})

//SILP:IN_TABLE_ASPECT_MIXIN_CONSTRUCTOR(${class})
```

# IN_BOTH_MIXIN(class) #
```
protected ${class}(TO owner, string key) : base(owner, key) {
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

public override string RevInfo {
    get {
        if (_Index >= 0) {
            return string.Format("[{0}] ({1})", _Index, Revision);
        } else {
            return string.Format("({0})", Revision);
        }
    }
}
```

# ASPECT_MIXIN() #
```
private readonly IContext _Context;
public IContext GetContext() {
    return _Context;
}

public IContext Context {
    get { return _Context; }
}

private readonly string _Path;
public string Path {
    get { return _Path; }
}

private readonly string _Uri;
public override sealed string Uri {
    get {
        return _Uri;
    }
}

private bool _Debugging = false;
public bool Debugging {
    get { return _Debugging; }
    set { _Debugging = value; }
}

public override sealed bool DebugMode {
    get { return _Debugging || _Context.DebugMode; }
}

public override void OnAdded() {
    Env.Instance.Hooks._OnAspectAdded(this);
}

public override void OnRemoved() {
    Env.Instance.Hooks._OnAspectRemoved(this);
}
```

# CONTEXT_MIXIN() #
```
    _Path = Env.GetContextPath(this);

    _Properties = new Properties(this, ContextConsts.KeyProperties);
    _Channels = new Channels(this, ContextConsts.KeyChannels);
    _Handlers = new Handlers(this, ContextConsts.KeyHandlers);
    _Vars = new Vars(this, ContextConsts.KeyVars);
    _Manners = new Manners(this, ContextConsts.KeyManners);
    _Bus = new Bus(this, ContextConsts.KeyBus);
}

private readonly string _Path;
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

private readonly Bus _Bus;
public Bus Bus {
    get { return _Bus; }
}

public IContext GetContext() {
    return this;
}

public override sealed string Uri {
    get { return _Path; }
}

private bool _Debugging = false;
public bool Debugging {
    get { return _Debugging; }
    set { _Debugging = value; }
}

public override sealed bool DebugMode {
    get { return _Debugging; }
}

public override void OnAdded() {
    Env.Instance.Hooks._OnContextAdded(this);
}

public override void OnRemoved() {
    Env.Instance.Hooks._OnContextRemoved(this);
}
```
