using System;

namespace angeldnd.dap {
    public delegate IObject DapFactory(params object[] values);

    /*
     * Here the factory will add checker to property directly, since the checker
     * need type of the value, which is not available in Property, so we need to do
     * casting in the factory method, so don't want to cast again just for adding
     * the checker later.
     */
    public delegate bool SpecValueCheckerFactory(IProperty prop, Pass pass, Data spec, string specKey);

    public static class Factory {
        private static Vars _Factories;

        //private static Vars _SpecValueCheckerFactories;

        static Factory() {
            _Factories = new Context(null).Vars;

            //Aspects
            Register<Item>(ItemConsts.TypeItem);
            Register<Properties>(PropertiesConsts.TypeProperties);
            Register<BoolProperty>(PropertiesConsts.TypeBoolProperty);
            Register<IntProperty>(PropertiesConsts.TypeIntProperty);
            Register<LongProperty>(PropertiesConsts.TypeLongProperty);
            Register<FloatProperty>(PropertiesConsts.TypeFloatProperty);
            Register<DoubleProperty>(PropertiesConsts.TypeDoubleProperty);
            Register<StringProperty>(PropertiesConsts.TypeStringProperty);
            Register<DataProperty>(PropertiesConsts.TypeDataProperty);

            /*
            _SpecValueCheckerFactories = new Context().Vars;
            SpecHelper.RegistrySpecValueCheckers();
            */
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

        /*
        public static bool RegisterSpecValueChecker(string propertyType, string specKind, SpecValueCheckerFactory factory) {
            string factoryKey = string.Format("{0}{1}{2}", propertyType, SpecConsts.Separator, specKind);
            return _SpecValueCheckerFactories.AddVar(factoryKey, factory) != null;
        }

        public static bool FactorySpecValueChecker(IProperty prop, Pass pass, Data spec, string specKey) {
            string specKind = SpecConsts.GetSpecKind(specKey);
            string factoryKey = string.Format("{0}{1}{2}", prop.Type, SpecConsts.Separator, specKind);
            SpecValueCheckerFactory factory = _SpecValueCheckerFactories.GetValue<SpecValueCheckerFactory>(factoryKey);
            if (factory != null) {
                return factory(prop, pass, spec, specKey);
            } else {
                Log.Error("Unknown SpecValueChecker Type: {0}, Spec: {1}", factoryKey, spec);
            }
            return false;
        }
        */
    }
}
