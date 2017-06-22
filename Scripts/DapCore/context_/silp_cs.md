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

# ADD_REMOVE_HELPER(name, a_key, a_var, a_type, l_name, l_var, l_type) #
```C#
public bool Add${name}(string ${a_key}, ${l_type} ${l_var}) {
    ${a_type} ${a_var} = Get(${a_key});
    if (${a_var} != null) {
        return ${a_var}.Add${l_name}(${l_var});
    }
    return false;
}

public bool Remove${name}(string ${a_key}, ${l_type} ${l_var}) {
    ${a_type} ${a_var} = Get(${a_key});
    if (${a_var} != null) {
        return ${a_var}.Remove${l_name}(${l_var});
    }
    return false;
}
```

# WEAK_LIST_FOREACH_BEGIN(name, var_name, cs_type, list_name) #
```C#
if (${list_name} != null) {
    if (Log.Profiler != null) Log.Profiler.BeginSample("${name}");
    bool needGc = false;
    foreach (var r in ${list_name}.RetainLock()) {
        ${cs_type} ${var_name} = ${list_name}.GetTarget(r);
        if (${var_name} == null) {
            needGc = true;
        } else {
            if (Log.Profiler != null) Log.Profiler.BeginSample(${var_name}.ToString());
```

# WEAK_LIST_FOREACH_END(name, var_name, cs_type, list_name) #
```C#
            if (Log.Profiler != null) Log.Profiler.EndSample();
        }
    }
    ${list_name}.ReleaseLock(needGc);
    if (Log.Profiler != null) Log.Profiler.EndSample();
}
```
