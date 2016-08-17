# PROPERTY_CLASS(type, cs_type) #
```C#
[DapVarType(PropertiesConsts.Type${type}Property, typeof(${cs_type}))]
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
