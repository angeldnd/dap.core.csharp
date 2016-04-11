# PROPERTY_CLASS(type, cs_type) #
```C#
[DapType(PropertiesConsts.Type${type}Property)]
[DapOrder(-10)]
public sealed class ${type}Property : BaseProperty<${cs_type}> {
    public ${type}Property(IDictProperties owner, string key) : base(owner, key) {
    }

    public ${type}Property(ITableProperties owner, int index) : base(owner, index) {
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
