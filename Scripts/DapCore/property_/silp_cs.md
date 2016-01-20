# PROPERTY_CLASS(type, cs_type) #
```C#
public class ${type}Property : Property<${cs_type}> {
    public override string Type {
        get { return PropertiesConsts.Type${type}Property; }
    }

    public ${type}Property(Properties owner, string path, Pass pass) : base(owner, path, pass) {
    }

    protected override bool DoEncode(Data data) {
        return data.Set${type}(PropertiesConsts.KeyValue, Value);
    }

    protected override bool DoDecode(Pass pass, Data data) {
        return SetValue(pass, data.Get${type}(PropertiesConsts.KeyValue));
    }

    protected override bool NeedUpdate(${cs_type} newVal) {
        return base.NeedUpdate(newVal) || (Value != newVal);
    }
}
```
