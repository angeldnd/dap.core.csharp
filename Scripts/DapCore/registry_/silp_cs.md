# ASPECT_MIXIN() #
```
private Entity _Entity = null;
public Entity Entity {
    get { return _Entity; }
}

private string _Path = null;
public string Path {
    get { return _Path; }
}

public string RevPath {
    get {
        return string.Format("{0} ({1})", _Path, Revision);
    }
}

public bool Inited {
    get { return _Entity != null; }
}

public bool Init(Entity entity, string path) {
    if (_Entity != null) {
        Error("Already Inited: {0} -> {1}, {2}", _Entity, entity, path);
        return false;
    }
    if (entity == null) {
        Error("Invalid Entity: {0}, {1}", entity, path);
        return false;
    }
    if (!EntityConsts.IsValidAspectPath(path)) {
        Error("Invalid Path: {0}, {1}", entity, path);
        return false;
    }

    _Entity = entity;
    _Path = path;
    return true;
}

```

# ASPECT_LOG_MIXIN(virtualOrOverride) #
```C#
public ${virtualOrOverride} string GetLogPrefix() {
    if (_Entity != null) {
        return string.Format("{0}[{1}] {2} ", _Entity.GetLogPrefix(), GetType().Name, RevPath);
    } else {
        return string.Format("[] [{0}] {1} ", GetType().Name, RevPath);
    }
}
```
