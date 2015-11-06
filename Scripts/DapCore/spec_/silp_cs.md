# _PROPERTY_SPEC(type, cs_type, checker_type, checker_op, value_name, value_var) #
```C#
public class ${type}SpecValueChecker${checker_type} : SpecValueChecker<${cs_type}> {
    public readonly ${cs_type} ${value_name}Value;
    public ${type}SpecValueChecker${checker_type}(${cs_type} ${value_var}Value) {
        ${value_name}Value = ${value_var}Value;
    }

    protected override bool IsValid(${cs_type} val) {
        return val ${checker_op} ${value_name}Value;
    }

    public override bool DoEncode(Data spec) {
        return spec.Set${type}(PropertiesSpecConsts.Key${checker_type}, ${value_name}Value);
    }
}

```

# PROPERTY_SPEC(type, cs_type) #
```C#
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, Bigger, >, Min, min)
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, BiggerOrEqual, >=, Min, min)
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, Smaller, <, Max, max)
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, SmallerOrEqual, <=, Max, max)
//SILP: PROPERTY_SPEC_IN(${type}, ${cs_type})
```

# _PROPERTY_SPEC_IN(type, cs_type, checker_type, each_return, final_return) #
```C#
public class ${type}SpecValueChecker${checker_type} : SpecValueChecker<${cs_type}> {
    public readonly ${cs_type}[] Values;
    public ${type}SpecValueChecker${checker_type}(${cs_type}[] values) {
        Values = values;
    }

    protected override bool IsValid(${cs_type} val) {
        foreach (${cs_type} v in Values) {
            if (v == val) return ${each_return};
        }
        return ${final_return};
    }

    public override bool DoEncode(Data spec) {
        Data values = new Data();
        for (int i = 0; i < Values.Length; i++) {
            ${cs_type} v = Values[i];
            values.Set${type}(i.ToString(), v);
        }
        return spec.SetData(PropertiesSpecConsts.Key${checker_type}, values);
    }
}

```

# PROPERTY_SPEC_IN(type, cs_type) #
```C#
//SILP:_PROPERTY_SPEC_IN(${type}, ${cs_type}, In, true, false) #
//SILP:_PROPERTY_SPEC_IN(${type}, ${cs_type}, NotIn, false, true) #
```

# _PROPERTIES_SPEC_HELPER(type, cs_type, checker_type) #
```
_SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.Type${type}Property, PropertiesSpecConsts.Key${checker_type})] =
        (Property _prop, Pass pass, Data spec, string key) => {
    if (spec == null) return false;
    ${type}Property prop = _prop as ${type}Property;
    if (prop == null) return false;
    return prop.AddValueChecker(new ${type}SpecValueChecker${checker_type}(spec.Get${type}(key)));
};

```

# PROPERTIES_SPEC_HELPER(type, cs_type) #
```
//SILP:_PROPERTIES_SPEC_HELPER(${type}, ${cs_type}, Bigger)
//SILP:_PROPERTIES_SPEC_HELPER(${type}, ${cs_type}, BiggerOrEqual)
//SILP:_PROPERTIES_SPEC_HELPER(${type}, ${cs_type}, Smaller)
//SILP:_PROPERTIES_SPEC_HELPER(${type}, ${cs_type}, SmallerOrEqual)
```

# _PROPERTIES_SPEC_IN_HELPER(type, cs_type, checker_type) #
```
RegistrySpecValueCheckerFactories(PropertiesConsts.Type${type}Property, PropertiesSpecConsts.Key${checker_type},
        (Property _prop, Pass pass, Data spec, string key) => {
    if (spec == null) return false;
    ${type}Property prop = _prop as ${type}Property;
    if (prop == null) return false;
    Data _values = spec.GetData(key, null);
    if (_values == null) return false;
    List<${cs_type}> values = new List<${cs_type}>();
    for (int i = 0; i < _values.Count; i++) {
        string index = i.ToString();
        if (_values.Is${type}(index)) {
            values.Add(_values.Get${type}(index));
        }
    }
    return prop.AddValueChecker(new ${type}SpecValueChecker${checker_type}(values.ToArray()));
});

```
# PROPERTIES_SPEC_IN_HELPER(type, cs_type) #
```
//SILP:_PROPERTIES_SPEC_IN_HELPER(${type}, ${cs_type}, In)
//SILP:_PROPERTIES_SPEC_IN_HELPER(${type}, ${cs_type}, NotIn)
```

