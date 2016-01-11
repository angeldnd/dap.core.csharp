# CONTEXT_MIXIN(owner, class) #
```
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

public ${class}(${owner} owner, string path, Pass pass) : base(owner, path, pass) {
    Pass sectionPass = pass.Open;

    _Properties = new Properties(this, ContextConsts.SectionProperties, sectionPass);
    _Channels = new Channels(this, ContextConsts.SectionChannels, sectionPass);
    _Handlers = new Handlers(this, ContextConsts.SectionHandlers, sectionPass);
    _Vars = new Vars(this, ContextConsts.SectionVars, sectionPass);
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

private WeakList<IEntityWatcher> _Watchers = null;

public int WatcherCount {
    get { return WeakListHelper.Count(_Watchers); }
}

public bool AddWatcher(IEntityWatcher watcher) {
    return WeakListHelper.Add(ref _Watchers, watcher);
}

public bool RemoveWatcher(IEntityWatcher watcher) {
    return WeakListHelper.Remove(_Watchers, watcher);
}
```
