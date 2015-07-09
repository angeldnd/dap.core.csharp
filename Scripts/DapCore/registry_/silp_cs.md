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

private bool _Inited = false;
public bool Inited {
    get { return _Inited; }
}

public bool Init(Entity entity, string path) {
    if (_Inited) return false;
    if (entity == null || string.IsNullOrEmpty(path)) return false;

    _Entity = entity;
    _Path = path;
    _Inited = true;
    return true;
}

```


