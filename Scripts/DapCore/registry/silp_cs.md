# REGISTER_ASPECT_FACTORY(type, cs_type) #
```
result.RegisterAspect(${type}, (Entity entity, string path) => {
    ${cs_type} aspect = new ${cs_type}();
    if (aspect.Init(entity, path)) {
        return aspect;
    }
    return null;
});

```

# REGISTER_ENTITY_FACTORY(type, cs_type) #
```
result.RegisterEntity(${type}, () => {
    return new ${cs_type}();
});
```

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
    OnInit();
    return true;
}

```


