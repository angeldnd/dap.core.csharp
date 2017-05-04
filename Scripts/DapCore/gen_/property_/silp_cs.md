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

[DapType(PropertiesConsts.Type${type}TableProperty)]
[DapOrder(DapOrders.Property)]
public sealed class ${type}TableProperty : TableProperty<${type}Property> {
    public ${type}TableProperty(IDictProperties owner, string key) : base(owner, key) {
    }

    public ${type}TableProperty(ITableProperties owner, int index) : base(owner, index) {
    }
}

[DapType(PropertiesConsts.Type${type}DictProperty)]
[DapOrder(DapOrders.Property)]
public sealed class ${type}DictProperty : DictProperty<${type}Property> {
    public ${type}DictProperty(IDictProperties owner, string key) : base(owner, key) {
    }

    public ${type}DictProperty(ITableProperties owner, int index) : base(owner, index) {
    }
}
```
