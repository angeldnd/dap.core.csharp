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
protected ${class}(TO owner, Pass pass) : base(pass) {
    _Owner = owner;
}

private readonly TO _Owner;
public TO Owner {
    get { return _Owner; }
}
public IOwner GetOwner() {
    return _Owner;
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

public virtual void OnAdded() {}
public virtual void OnRemoved() {}
```

# ELEMENT_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, Pass pass) : base(owner, pass) {
}
```

# IN_TREE_MIXIN_PATH() #
```
private readonly string _Path;
public string Path {
    get { return _Path; }
}
```

# IN_TREE_MIXIN(class) #
```
protected ${class}(TO owner, string path, Pass pass) : base(owner, pass) {
    _Path = path;
}

//SILP:IN_TREE_MIXIN_PATH()

public override string RevInfo {
    get {
        return string.Format("[{0}] ({1}) ", _Path, Revision);
    }
}
```

# IN_TREE_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, string path, Pass pass) : base(owner, path, pass) {
}
```

# IN_TABLE_MIXIN_INDEX() #
```
private int _Index = -1;
public int Index {
    get { return _Index; }
}

public bool SetIndex(Pass pass, int index) {
    if (!CheckAdminPass(pass)) return false;

    _Index = index;
    return true;
}
```

# IN_TABLE_MIXIN(class) #
```
protected ${class}(TO owner, int index, Pass pass) : base(owner, pass) {
    _Index = index;
}

//SILP:IN_TABLE_MIXIN_INDEX()

public override string RevInfo {
    get {
        return string.Format("[{0}] ({1}) ", _Index, Revision);
    }
}
```

# IN_TABLE_MIXIN_CONSTRUCTOR(class) #
```
protected ${class}(TO owner, int index, Pass pass) : base(owner, index, pass) {
}
```

# IN_BOTH_MIXIN_CONSTRUCTOR(class) #
```
//SILP:IN_TREE_MIXIN_CONSTRUCTOR(${class})

//SILP:IN_TABLE_MIXIN_CONSTRUCTOR(${class})
```

# IN_BOTH_MIXIN(class) #
```
protected ${class}(TO owner, string path, Pass pass) : base(owner, pass) {
    _Path = path;
}

protected ${class}(TO owner, int index, Pass pass) : base(owner, pass) {
    _Index = index;
}

//SILP: IN_TREE_MIXIN_PATH()

//SILP: IN_TABLE_MIXIN_INDEX()

public override string RevInfo {
    get {
        if (_Path != null) {
            return string.Format("[{0}] ({1}) ", _Path, Revision);
        } else {
            return string.Format("[{0}] ({1}) ", _Index, Revision);
        }
    }
}
```

# ENTITY_MIXIN() #
```
public IEntity GetEntity() {
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

//SILP: DECLARE_LIST(EntityWatcher, watcher, IEntityWatcher, _EntityWatchers)

public virtual void OnAspectAdded(IAspect aspect) {
    WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {
        watcher.OnAspectAdded(this, aspect);
    });
}

public virtual void OnAspectRemoved(IAspect aspect) {
    WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {
        watcher.OnAspectRemoved(this, aspect);
    });
}
```

# IN_ENTITY_ELEMENT_MIXIN() #
```
public IEntity GetEntity() {
    return Owner.GetEntity();
}

public IEntity Entity {
    get { return Owner.GetEntity(); }
}
```

# ASPECT_MIXIN() #
```
//SILP:IN_ENTITY_ELEMENT_MIXIN()
```

# SECTION_MIXIN(T) #
```
//SILP:IN_ENTITY_ELEMENT_MIXIN()

protected override void OnElementAdded(${T} element) {
    WeakListHelper.Notify(_Watchers, (ISectionWatcher watcher) => {
        watcher.OnAspectAdded(element);
    });
    Entity.OnAspectAdded(element);
}

protected override void OnElementRemoved(${T} element) {
    WeakListHelper.Notify(_Watchers, (ISectionWatcher watcher) => {
        watcher.OnAspectRemoved(element);
    });
    Entity.OnAspectRemoved(element);
}

//SILP: DECLARE_LIST(Watcher, watcher, ISectionWatcher, _Watchers)
```

# CONTEXT_MIXIN() #
```
    Pass sectionPass = Pass.ToOpen(Pass);

    _Properties = new Properties(this, sectionPass);
    _Channels = new Channels(this, sectionPass);
    _Handlers = new Handlers(this, sectionPass);
    _Vars = new Vars(this, sectionPass);
    _Others = new Others(this, sectionPass);
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

private readonly Others _Others;
public Others Others {
    get { return _Others; }
}
```

# IN_BOTH_CONTEXT_MIXIN(class) #
```
private ${class}(TO owner, string path, Pass pass) : base(owner, path, pass) {
    Pass sectionPass = Pass.ToOpen(Pass);

    _Properties = new Properties(this, sectionPass);
    _Channels = new Channels(this, sectionPass);
    _Handlers = new Handlers(this, sectionPass);
    _Vars = new Vars(this, sectionPass);
    _Others = new Others(this, sectionPass);
}

private ${class}(TO owner, int index, Pass pass) : base(owner, index, pass) {
//SILP: CONTEXT_MIXIN()
```
