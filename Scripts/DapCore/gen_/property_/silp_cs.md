# PROPERTY_CLASS(type, cs_type) #
```C#
public sealed class ${type}Property : Property<${cs_type}> {
    public override string Type {
        get { return PropertiesConsts.Type${type}Property; }
    }

    public ${type}Property(Properties owner, string key) : base(owner, key) {
    }

    public ${type}Property(Properties owner, int index) : base(owner, index) {
    }

    protected override bool DoEncode(Data data) {
        return data.Set${type}(PropertiesConsts.KeyValue, Value);
    }

    protected override bool DoDecode(Data data) {
        return SetValue(data.Get${type}(PropertiesConsts.KeyValue));
    }

    protected override bool NeedUpdate(${cs_type} newVal) {
        return base.NeedUpdate(newVal) || (Value != newVal);
    }
}
```
