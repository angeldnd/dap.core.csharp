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
    _Key = string.Format("{0}", Guid.NewGuid().ToString());
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

private bool _IsOrphan = true;
public bool IsOrphan {
    get { return _IsOrphan; } 
}

public void _OnAdded(IOwner owner) {
    if (_Owner != owner) {
        throw new DapException("{0}: _OnAdded: Wrong Owner: {1} -> {2}",
                                LogPrefix, _Owner, owner);
    }
    if (!_IsOrphan) {
        throw new DapException("{0}: _OnAdded: IsOrphan == false", LogPrefix);
    }
    _IsOrphan = false;
    OnAdded();
}

public void _OnRemoved(IOwner owner) {
    if (_Owner != owner) {
        throw new DapException("{0}: _OnRemoved: Wrong Owner: {1} -> {2}",
                                LogPrefix, _Owner, owner);
    }
    if (_IsOrphan) {
        throw new DapException("{0}: _OnRemoved: IsOrphan == true", LogPrefix);
    }
    _IsOrphan = true;
    OnRemoved();
}

protected virtual void OnAdded() {}
protected virtual void OnRemoved() {}

protected override void AddSummaryFields(Data summary) {
    base.AddSummaryFields(summary);
    summary.S(ElementConsts.SummaryKey, _Key);
}
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

public bool _SetIndex(IOwner owner, int index) {
    if (Owner != owner) return false;

    _Index = index;
    return true;
}

protected override void AddSummaryFields(Data summary) {
    base.AddSummaryFields(summary);
    summary.I(ElementConsts.SummaryIndex, _Index);
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
    get { return _Debugging || base.DebugMode; }
}

protected override sealed void OnAdded() {
    if (_Context != null) {
        Env.Instance.Hooks._OnAspectAdded(this);
    }
    OnAspectAdded();
}

protected override sealed void OnRemoved() {
    if (_Context != null) {
        //TODO: Add AspectWatcher to Context
    }
    OnAspectRemoved();
}

protected virtual void OnAspectAdded() {}
protected virtual void OnAspectRemoved() {}

```

# ASPECT_SUMMARY_MIXIN() #
```
protected override void AddSummaryFields(Data summary) {
    base.AddSummaryFields(summary);
    summary.S(ContextConsts.SummaryPath, _Path)
           .B(ContextConsts.SummaryDebugging, _Debugging);
}
```

# DICT_ASPECT_MIXIN() #
```
protected override void AddSummaryFields(Data summary) {
    base.AddSummaryFields(summary);
    summary.S(ContextConsts.SummaryPath, _Path)
           .B(ContextConsts.SummaryDebugging, _Debugging)
           .I(ContextConsts.SummaryWatcherCount, DictWatcherCount)
           .I(ContextConsts.Summary2ndWatcherCount, GenericDictWatcherCount);
}
```

# TABLE_ASPECT_MIXIN() #
```
protected override void AddSummaryFields(Data summary) {
    base.AddSummaryFields(summary);
    summary.S(ContextConsts.SummaryPath, _Path)
           .B(ContextConsts.SummaryDebugging, _Debugging)
           .I(ContextConsts.SummaryWatcherCount, TableWatcherCount)
           .I(ContextConsts.Summary2ndWatcherCount, GenericTableWatcherCount);
}
```

# CONTEXT_MIXIN() #
```
    _Path = Env.GetContextPath(this);

    _Properties = AddTopAspect<Properties>(ContextConsts.KeyProperties);
    _Channels = AddTopAspect<Channels>(ContextConsts.KeyChannels);
    _Handlers = AddTopAspect<Handlers>(ContextConsts.KeyHandlers);
    _Bus = AddTopAspect<Bus>(ContextConsts.KeyBus);
    _Vars = AddTopAspect<Vars>(ContextConsts.KeyVars);
    _Utils = AddTopAspect<Utils>(ContextConsts.KeyUtils);
    _Manners = AddTopAspect<Manners>(ContextConsts.KeyManners);
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

private readonly Bus _Bus;
public Bus Bus {
    get { return _Bus; }
}

private readonly Vars _Vars;
public Vars Vars {
    get { return _Vars; }
}

private readonly Utils _Utils;
public Utils Utils {
    get { return _Utils; }
}

private readonly Manners _Manners;
public Manners Manners {
    get { return _Manners; }
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

private bool _Removed = false;
public bool Removed {
    get { return _Removed; }
}

protected override sealed void OnAdded() {
    Env.Instance.Hooks._OnContextAdded(this);
    OnContextAdded();
}

protected override sealed void OnRemoved() {
    if (_Removed) {
        Error("Already Removed");
        return;
    }
    _Removed = true;
    Env.Instance.Hooks._OnContextRemoved(this);
    OnContextRemoved();
}

protected override void AddSummaryFields(Data summary) {
    base.AddSummaryFields(summary);
    summary.S(ContextConsts.SummaryPath, _Path)
           .B(ContextConsts.SummaryDebugging, _Debugging);
}

private Dictionary<string, IAspect> _TopAspectsDict = new Dictionary<string, IAspect>();
private List<IAspect> _TopAspectsList = new List<IAspect>();

protected TA AddTopAspect<TA>(string key) where TA : class, IAspect {
    IAspect oldAspect = null;
    if (_TopAspectsDict.TryGetValue(key, out oldAspect)) {
        Critical("Top Aspect Key Conflicted: <{0}>: {1} -> {2}",
                    typeof(TA).FullName, key, oldAspect);
        return null;
    }
    TA topAspect = Factory.Create<TA>(this, key);
    if (topAspect != null) {
        _TopAspectsDict[topAspect.Key] = topAspect;
        _TopAspectsList.Add(topAspect);
    }
    return topAspect;
}

public TA GetAspect<TA>(string aspectPath, bool isDebug = false) where TA : class, IAspect {
    if (aspectPath == null) return null;
    List<string> segments = PathConsts.Split(aspectPath);
    if (segments.Count < 1) {
        ErrorOrDebug(isDebug, "Invalid aspectPath: {0}", aspectPath);
        return null;
    }
    IAspect topAspect;
    if (!_TopAspectsDict.TryGetValue(segments[0], out topAspect)) {
        ErrorOrDebug(isDebug, "Not Found: {0}", aspectPath);
        return null;
    }
    if (segments.Count == 1) {
        return topAspect.As<TA>(isDebug);
    } else {
        IOwner asOwner = topAspect.As<IOwner>(isDebug);
        if (asOwner != null) {
            return TreeHelper.GetDescendant<TA>(asOwner, segments, 1, isDebug);
        }
    }
    return null;
}

public IAspect GetAspect(string aspectPath, bool isDebug = false) {
    return GetAspect<IAspect>(aspectPath, isDebug);
}

public void ForEachTopAspects(Action<IAspect> callback) {
    var en = _TopAspectsList.GetEnumerator();
    while (en.MoveNext()) {
        callback(en.Current);
    }
}

public void ForEachAspects(Action<IAspect> callback) {
    ForEachTopAspects((IAspect aspect) => {
        AspectExtension.ForEachAspects(aspect, callback);
    });
}

public virtual void AddDetailFields(Data summary) {
    Data aspects = DataCache.Take("_summary.aspects");
    ForEachAspects((IAspect aspect) => {
        aspects.A(aspect.Path, aspect.Summary);
    });
    summary.A(ContextConsts.SummaryAspects, aspects);
}

protected virtual void OnContextAdded() {}
protected virtual void OnContextRemoved() {}

```

# MANNER_MIXIN() #
```C#
}

public Properties Properties {
    get { return Context.Properties; }
}

public Channels Channels {
    get { return Context.Channels; }
}

public Handlers Handlers {
    get { return Context.Handlers; }
}

public Bus Bus {
    get { return Context.Bus; }
}

public Vars Vars {
    get { return Context.Vars; }
}

public Manners Manners {
    get { return Owner; }
}
```

