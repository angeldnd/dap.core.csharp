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

        public const string TypeBoolTableProperty = "BoolTable";
        public const string TypeIntTableProperty = "IntTable";
        public const string TypeLongTableProperty = "LongTable";
        public const string TypeFloatTableProperty = "FloatTable";
        public const string TypeDoubleTableProperty = "DoubleTable";
        public const string TypeStringTableProperty = "StringTable";
        public const string TypeDataTableProperty = "DataTable";

        public const string TypeBoolDictProperty = "BoolDict";
        public const string TypeIntDictProperty = "IntDict";
        public const string TypeLongDictProperty = "LongDict";
        public const string TypeFloatDictProperty = "FloatDict";
        public const string TypeDoubleDictProperty = "DoubleDict";
        public const string TypeStringDictProperty = "StringDict";
        public const string TypeDataDictProperty = "DataDict";

        public const string KeyKey = "k";
        public const string KeyValue = "v";
    }

    public sealed class Properties : DictAspect<IContext, IProperty>, IDictProperties {
        public Properties(IContext owner, string key) : base(owner, key) {
        }

        public bool WaitProperty(string key, Action<IProperty, bool> callback, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, key, callback, waitSetup);
        }

        public bool WaitAndWatchProperty(string key, IVarWatcher watcher) {
            return Owner.Utils.WaitSetupAspect(this, key, (IProperty prop, bool isNew) => {
                if (!isNew) {
                    watcher.OnChanged(prop);
                }
            });
        }

        public bool WaitAndWatchProperty(string key, IBlockOwner owner, Action<IVar> block) {
            return WaitAndWatchProperty(key, new BlockVarWatcher(owner, block));
        }

        public bool WaitProperty<T>(string key, Action<IProperty<T>, bool> callback, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, key, callback, waitSetup);
        }

        public bool WaitAndWatchProperty<T>(string key, IValueWatcher<T> watcher) {
            return Owner.Utils.WaitSetupAspect(this, key, (IProperty<T> prop, bool isNew) => {
                prop.AddValueWatcher(watcher);
                if (!isNew) {
                    watcher.OnChanged(prop, prop.Value);
                }
            });
        }

        public bool WaitAndWatchProperty<T>(string key, IBlockOwner owner, Action<IVar<T>, T> block) {
            return WaitAndWatchProperty<T>(key, new BlockValueWatcher<T>(owner, block));
        }
    }
}
