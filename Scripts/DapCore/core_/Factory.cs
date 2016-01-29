using System;

namespace angeldnd.dap {
    public delegate IObject DapFactory(params object[] values);

    public static class Factory {
        private static readonly Vars _Factories = new Vars(Env.Instance, "Factories");

        static Factory() {
            Register<Registry>(RegistryConsts.TypeRegistry);
            Register<Items>(ItemsConsts.TypeItems);
            Register<Item>(ItemConsts.TypeItem);

            Register<BoolProperty>(PropertiesConsts.TypeBoolProperty);
            Register<IntProperty>(PropertiesConsts.TypeIntProperty);
            Register<LongProperty>(PropertiesConsts.TypeLongProperty);
            Register<FloatProperty>(PropertiesConsts.TypeFloatProperty);
            Register<DoubleProperty>(PropertiesConsts.TypeDoubleProperty);
            Register<StringProperty>(PropertiesConsts.TypeStringProperty);
            Register<DataProperty>(PropertiesConsts.TypeDataProperty);
        }

        public static bool Register(string type, DapFactory factory) {
            return _Factories.AddVar(type, factory) != null;
        }

        public static bool Register<T>(string type) where T : class, IObject {
            return Register(type, (object[] values) => {
                return Activator.CreateInstance(typeof(T), values) as T;
            });
        }

        public static T New<T>(string type, params object[] values) where T : class, IObject {
            DapFactory factory = _Factories.GetValue<DapFactory>(type);
            if (factory != null) {
                try {
                    object obj = factory(values);
                    if (obj is T) {
                        return (T)obj;
                    } else {
                        Log.Error("Factory.New: <{0}> Type Mismatched: {1} -> {2}",
                                type, typeof(T).FullName, obj.GetType().FullName);
                    }
                } catch (Exception e) {
                    Log.Error("Factory.New: <{0}> {1} -> {2}", type, typeof(T).FullName, e);
                }
            } else {
                Log.Error("Factory.New: {0} Unknown Type", type);
            }
            return null;
        }
    }
}
