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
                prop.AddVarWatcher(watcher);
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

        public Data EncodeValues(HashSet<string> excludes = null) {
            Data data = new RealData();
            data.S(ObjectConsts.KeyDapType, Context.DapType);

            ForEach<IProperty>((IProperty prop) => {
                if (excludes == null || !excludes.Contains(prop.Key)) {
                    Data propValue = prop.EncodeValue();
                    propValue.CopyValueTo(PropertiesConsts.KeyValue, data, prop.Key);
                }
            });
            return data;
        }

        public bool DecodeValues(Data data, bool strict = false) {
            bool ok = true;
            foreach (var key in data.Keys) {
                if (key == ObjectConsts.KeyDapType) continue;

                IProperty prop = Get(key);
                if (prop == null) {
                    Error("Invalid Entry Key: {0} -> {1}",
                                    key, data.GetValue(key));
                    ok = false;
                    if (strict) return ok;
                }
                Data valueData = new RealData();
                if (data.CopyValueTo(key, valueData, PropertiesConsts.KeyValue)) {
                    if (!prop.DecodeValue(valueData)) {
                        prop.Error("Invalid Entry Value: DecodeValue: {0} ->\n{1}",
                                    key,
                                    Convertor.DataConvertor.Convert(valueData, "\t"));
                        ok = false;
                        if (strict) return ok;
                    }
                }
            }
            return ok;
        }
    }
}
