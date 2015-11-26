# DECLARE_LIST(name, var_name, cs_type, list_name) #
```C#
protected List<${cs_type}> ${list_name} = null;

public int ${name}Count {
    get {
        if (${list_name} == null) {
            return 0;
        }
        return ${list_name}.Count;
    }
}

public virtual bool Add${name}(${cs_type} ${var_name}) {
    if (${list_name} == null) {
        ${list_name} = new List<${cs_type}>();
        ResetAllVarWatchers();
    }
    if (!${list_name}.Contains(${var_name})) {
        ${list_name}.Add(${var_name});
        return true;
    }
    return false;
}

public virtual bool Remove${name}(${cs_type} ${var_name}) {
    if (${list_name} != null && ${list_name}.Contains(${var_name})) {
        ${list_name}.Remove(${var_name});
        if (${list_name}.Count == 0) {
            ${list_name} = null;
            ResetAllVarWatchers();
        }
        return true;
    }
    return false;
}

```
