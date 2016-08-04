# PROPERTY_CLASS(type, cs_type) #
```C#
[DapType(PropertiesConsts.Type${type}Property)]
[DapOrder(DapOrders.Property)]
public sealed class ${type}Property : Property<${cs_type}> {
    public ${type}Property(IDictProperties owner, string key) : base(owner, key) {
    }

    public ${type}Property(ITableProperties owner, int index) : base(owner, index) {
    }

    protected override Encoder<${cs_type}> GetEncoder() {
        return Encoder.${type}Encoder;
    }

    protected override bool NeedUpdate(${cs_type} newVal) {
        return base.NeedSetup() || (Value != newVal);
    }
}
```
