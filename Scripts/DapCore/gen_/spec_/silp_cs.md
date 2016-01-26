# _PROPERTY_SPEC(type, cs_type, checker_kind, checker_op, value_name, value_var) #
```C#
public class ${type}SpecValueChecker${checker_kind} : SpecValueChecker<${cs_type}> {
    public readonly ${cs_type} ${value_name}Value;
    public ${type}SpecValueChecker${checker_kind}(${cs_type} ${value_var}Value) {
        ${value_name}Value = ${value_var}Value;
    }

    protected override bool IsValid(${cs_type} val) {
        return val ${checker_op} ${value_name}Value;
    }

    public override bool DoEncode(Data spec) {
        return spec.Set${type}(SpecConsts.Kind${checker_kind}, ${value_name}Value);
    }
}

public class Sub${type}SpecValueChecker${checker_kind}<T> : SpecValueChecker<T> {
    public readonly string SubKey;
    public readonly ${cs_type} ${value_name}Value;
    public readonly Func<T, ${cs_type}> ValueGetter;
    public Sub${type}SpecValueChecker${checker_kind}(string subKey,
                        ${cs_type} ${value_var}Value, Func<T, ${cs_type}> valueGetter) {
        SubKey = subKey;
        ${value_name}Value = ${value_var}Value;
        ValueGetter = valueGetter;
    }

    protected override bool IsValid(T _val) {
        ${cs_type} val = ValueGetter(_val);
        return val ${checker_op} ${value_name}Value;
    }

    public override bool DoEncode(Data spec) {
        return spec.Set${type}(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.Kind${checker_kind}), ${value_name}Value);
    }
}

public class Data${type}SpecValueChecker${checker_kind} : Sub${type}SpecValueChecker${checker_kind}<Data> {
    public Data${type}SpecValueChecker${checker_kind}(string subKey, ${cs_type} ${value_var}Value) 
            : base(subKey, ${value_var}Value, (Data val) => val.Get${type}(subKey)) {
    }
}

```

# PROPERTY_SPEC(type, cs_type) #
```C#
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, Bigger, >, Min, min)
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, BiggerOrEqual, >=, Min, min)
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, Smaller, <, Max, max)
//SILP:_PROPERTY_SPEC(${type}, ${cs_type}, SmallerOrEqual, <=, Max, max)
```

# _PROPERTY_SPEC_IN(type, cs_type, checker_kind, each_return, final_return) #
```C#
public class ${type}SpecValueChecker${checker_kind} : SpecValueChecker<${cs_type}> {
    public readonly ${cs_type}[] Values;
    public ${type}SpecValueChecker${checker_kind}(${cs_type}[] values) {
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
        return spec.SetData(SpecConsts.Kind${checker_kind}, values);
    }
}

public class Sub${type}SpecValueChecker${checker_kind}<T> : SpecValueChecker<T> {
    public readonly string SubKey;
    public readonly ${cs_type}[] Values;
    public readonly Func<T, ${cs_type}> ValueGetter;
    public Sub${type}SpecValueChecker${checker_kind}(string subKey,
                                    ${cs_type}[] values, Func<T, ${cs_type}> valueGetter) {
        SubKey = subKey;
        Values = values;
        ValueGetter = valueGetter;
    }

    protected override bool IsValid(T _val) {
        ${cs_type} val = ValueGetter(_val);
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
        return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.Kind${checker_kind}), values);
    }
}

public class Data${type}SpecValueChecker${checker_kind} : Sub${type}SpecValueChecker${checker_kind}<Data> {
    public Data${type}SpecValueChecker${checker_kind}(string subKey, ${cs_type}[] values)
            : base(subKey, values, (Data val) => val.Get${type}(subKey)) {
    }
}

```

# PROPERTY_SPEC_IN(type, cs_type) #
```C#
//SILP:_PROPERTY_SPEC_IN(${type}, ${cs_type}, In, true, false) #
//SILP:_PROPERTY_SPEC_IN(${type}, ${cs_type}, NotIn, false, true) #
```

# _REGISTER_SPEC_HELPER(type, cs_type, checker_kind) #
```C#
Spec.RegisterSpecValueChecker(PropertiesConsts.Type${type}Property, SpecConsts.Kind${checker_kind},
        (IProperty _prop, Data spec, string specKey) => {
    if (spec == null) return false;
    ${type}Property prop = _prop as ${type}Property;
    if (prop == null) return false;
    return prop.AddValueChecker(new ${type}SpecValueChecker${checker_kind}(spec.Get${type}(specKey)));
});

```

# REGISTER_SPEC_HELPER(type, cs_type) #
```C#
//SILP:_REGISTER_SPEC_HELPER(${type}, ${cs_type}, Bigger)
//SILP:_REGISTER_SPEC_HELPER(${type}, ${cs_type}, BiggerOrEqual)
//SILP:_REGISTER_SPEC_HELPER(${type}, ${cs_type}, Smaller)
//SILP:_REGISTER_SPEC_HELPER(${type}, ${cs_type}, SmallerOrEqual)
```

# _REGISTER_SPEC_IN_HELPER(type, cs_type, checker_kind) #
```C#
Spec.RegisterSpecValueChecker(PropertiesConsts.Type${type}Property, SpecConsts.Kind${checker_kind},
        (IProperty _prop, Data spec, string specKey) => {
    if (spec == null) return false;
    ${type}Property prop = _prop as ${type}Property;
    if (prop == null) return false;
    Data _values = spec.GetData(specKey, null);
    if (_values == null) return false;
    List<${cs_type}> values = new List<${cs_type}>();
    for (int i = 0; i < _values.Count; i++) {
        string index = i.ToString();
        if (_values.Is${type}(index)) {
            values.Add(_values.Get${type}(index));
        }
    }
    return prop.AddValueChecker(new ${type}SpecValueChecker${checker_kind}(values.ToArray()));
});

```
# REGISTER_SPEC_IN_HELPER(type, cs_type) #
```C#
//SILP:_REGISTER_SPEC_IN_HELPER(${type}, ${cs_type}, In)
//SILP:_REGISTER_SPEC_IN_HELPER(${type}, ${cs_type}, NotIn)
```

# _REGISTER_SPEC_DATA_HELPER(checker_kind) #
```C#
Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.Kind${checker_kind},
        (IProperty _prop, Data spec, string specKey) => {
    if (spec == null) return false;
    DataProperty prop = _prop as DataProperty;
    if (prop == null) return false;
    string subKey = SpecConsts.GetSubKey(specKey);
    if (subKey == null) return false;
    DataType valueType = spec.GetValueType(specKey);
    switch (valueType) {
        case DataType.Int:
            return prop.AddValueChecker(
                    new DataIntSpecValueChecker${checker_kind}(subKey, spec.GetInt(specKey)));
        case DataType.Long:
            return prop.AddValueChecker(
                    new DataLongSpecValueChecker${checker_kind}(subKey, spec.GetLong(specKey)));
        case DataType.Float:
            return prop.AddValueChecker(
                    new DataFloatSpecValueChecker${checker_kind}(subKey, spec.GetFloat(specKey)));
        case DataType.Double:
            return prop.AddValueChecker(
                    new DataDoubleSpecValueChecker${checker_kind}(subKey, spec.GetDouble(specKey)));
    }
    return false;
});

```

# _REGISTER_SPEC_DATA_IN_CASE(type, cs_type, checker_kind) #
```C#
        case DataType.${type}:
            List<${cs_type}> ${cs_type}Values = new List<${cs_type}>();
            for (int i = 0; i < _values.Count; i++) {
                string index = i.ToString();
                if (_values.Is${type}(index)) {
                    ${cs_type}Values.Add(_values.Get${type}(index));
                }
            }
            return prop.AddValueChecker(new Data${type}SpecValueChecker${checker_kind}(subKey, ${cs_type}Values.ToArray()));
```

# _REGISTER_SPEC_DATA_IN_HELPER(checker_kind) #
```C#
Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.Kind${checker_kind},
        (IProperty _prop, Data spec, string specKey) => {
    if (spec == null) return false;
    DataProperty prop = _prop as DataProperty;
    if (prop == null) return false;
    string subKey = SpecConsts.GetSubKey(specKey);
    if (subKey == null) return false;
    Data _values = spec.GetData(specKey, null);
    if (_values == null) return false;
    DataType valueType = _values.GetValueType(0.ToString());
    switch (valueType) {
        //SILP:_REGISTER_SPEC_DATA_IN_CASE(Int, int, ${checker_kind})
        //SILP:_REGISTER_SPEC_DATA_IN_CASE(Long, long, ${checker_kind})
        //SILP:_REGISTER_SPEC_DATA_IN_CASE(String, string, ${checker_kind})
    }
    return false;
});

```

# REGISTER_SPEC_DATA_HELPER() #
```C#
//SILP:_REGISTER_SPEC_DATA_HELPER(Bigger)
//SILP:_REGISTER_SPEC_DATA_HELPER(BiggerOrEqual)
//SILP:_REGISTER_SPEC_DATA_HELPER(Smaller)
//SILP:_REGISTER_SPEC_DATA_HELPER(SmallerOrEqual)
//SILP:_REGISTER_SPEC_DATA_IN_HELPER(In)
//SILP:_REGISTER_SPEC_DATA_IN_HELPER(NotIn)
```

