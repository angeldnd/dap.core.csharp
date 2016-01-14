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

# ELEMENT_MIXIN(class, owner) #
```
private readonly ${owner} _Owner;
public ${owner} Owner {
    get { return _Owner; }
}
public IOwner GetOwner() {
    return _Owner;
}

protected ${class}(${owner} owner, Pass pass) : base(pass) {
    _Owner = owner;
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

# IN_TREE_MIXIN(class, owner) #
```
private readonly string _Path;
public string Path {
    get { return _Path; }
}

protected ${class}(${owner} owner, string path, Pass pass) : base(owner, pass) {
    _Path = path;
}

public string RevPath {
    get {
        return string.Format("{0} ({1})", Path, Revision);
    }
}

public override string LogPrefix {
    get {
        return string.Format("{0}{1} ({2}) ",
                base.LogPrefix, Path, Revision);
    }
}
```

# ENTITY_MIXIN() #
```
//SILP: DECLARE_LIST(EntityWatcher, watcher, IEntityWatcher, _EntityWatchers)

public void OnAspectAdded(IAspect aspect) {
    WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {
        watcher.OnAspectAdded(this, aspect);
    });
}

public void OnAspectRemoved(IAspect aspect) {
    WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {
        watcher.OnAspectRemoved(this, aspect);
    });
}
```

# CONTEXT_MIXIN() #
```
    Pass sectionPass = Pass == null ? null : Pass.Open;

    _Properties = new Properties(this, sectionPass);
    _Channels = new Channels(this, sectionPass);
    _Handlers = new Handlers(this, sectionPass);
    _Vars = new Vars(this, sectionPass);
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

private bool _DebugMode = false;
public override bool DebugMode {
    get { return _DebugMode; }
}
public void SetDebugMode(bool debugMode) {
    _DebugMode= debugMode;
}

private string[] _DebugPatterns = {""};
public override string[] DebugPatterns {
    get { return _DebugPatterns; }
}
public void SetDebugPatterns(string[] patterns) {
    _DebugPatterns = patterns;
}
```
