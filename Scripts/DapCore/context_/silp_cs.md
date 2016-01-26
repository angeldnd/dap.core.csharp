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
