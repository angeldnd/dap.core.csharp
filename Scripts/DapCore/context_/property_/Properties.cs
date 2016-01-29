using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IProperties : IVars {
    }

    public interface IDictProperties : IProperties, IDict {
        /* DictPropertiesExtension
        IProperty AddProperty(string key, Data data);
        */
    }

    public interface ITableProperties : IProperties, ITable {
    }

    public static class PropertiesConsts {
        public const string TypeBoolProperty = "Bool";
        public const string TypeIntProperty = "Int";
        public const string TypeLongProperty = "Long";
        public const string TypeFloatProperty = "Float";
        public const string TypeDoubleProperty = "Double";
        public const string TypeStringProperty = "String";
        public const string TypeDataProperty = "Data";

        public const string KeyValue = "v";
    }

    public sealed class Properties : DictAspect<IContext, IProperty>, IDictProperties {
        public Properties(IContext owner, string key) : base(owner, key) {
        }
    }
}
