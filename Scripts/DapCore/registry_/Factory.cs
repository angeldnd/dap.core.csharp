using System;

namespace angeldnd.dap {
    public delegate Entity EntityFactory();
    public delegate Aspect AspectFactory(Entity entity, string path);

    /*
     * Here the factory will add checker to property directly, since the checker
     * need type of the value, which is not available in Property, so we need to do
     * casting in the factory method, so don't want to cast again just for adding
     * the checker later.
     */
    public delegate bool SpecValueCheckerFactory(Property prop, Pass pass, Data spec, string specKey);

    public class Factory: Entity {
        public override string Type {
            get { return null; }
        }

        public static Factory NewBuiltinFactory() {
            var result = new Factory();
            //Entities
            result.RegisterEntity<Context>(ContextConsts.TypeContext);

            //Aspects
            result.RegisterAspect<Item>(ItemConsts.TypeItem);
            result.RegisterAspect<Properties>(PropertiesConsts.TypeProperties);
            result.RegisterAspect<BoolProperty>(PropertiesConsts.TypeBoolProperty);
            result.RegisterAspect<IntProperty>(PropertiesConsts.TypeIntProperty);
            result.RegisterAspect<LongProperty>(PropertiesConsts.TypeLongProperty);
            result.RegisterAspect<FloatProperty>(PropertiesConsts.TypeFloatProperty);
            result.RegisterAspect<DoubleProperty>(PropertiesConsts.TypeDoubleProperty);
            result.RegisterAspect<StringProperty>(PropertiesConsts.TypeStringProperty);
            result.RegisterAspect<DataProperty>(PropertiesConsts.TypeDataProperty);

            result.RegisterAspect<Tickable>(TickableConsts.TypeTickable);

            result.RegisterAspect<Bundle>(BundleConsts.TypeBundle);
            result.RegisterAspect<DataEntry>(BundleConsts.TypeDataEntry);
            result.RegisterAspect<TextEntry>(BundleConsts.TypeTextEntry);
            result.RegisterAspect<BinEntry>(BundleConsts.TypeBinEntry);

            return result;
        }

        public readonly Vars EntityFactories;
        public readonly Vars AspectFactories;
        public readonly Vars SpecValueCheckerFactories;

        public Factory() {
            EntityFactories = Add<Vars>("entity_factories");
            AspectFactories = Add<Vars>("aspect_factories");
            SpecValueCheckerFactories = Add<Vars>("spec_value_checker_factories");
        }

        public bool RegisterEntity(string type, EntityFactory factory) {
            return EntityFactories.AddVar(type, factory) != null;
        }

        public bool RegisterEntity<T>(string type) where T : Entity {
            return RegisterEntity(type, () => {
                return Activator.CreateInstance(typeof(T)) as T;
            });
        }

        public Entity FactoryEntity(string type) {
            EntityFactory factory = EntityFactories.GetValue<EntityFactory>(type);
            if (factory != null) {
                return factory();
            } else {
                Error("Unknown Entity Type: {0}", type);
            }
            return null;
        }

        public bool RegisterAspect(string type, AspectFactory factory) {
            return AspectFactories.AddVar(type, factory) != null;
        }

        public bool RegisterAspect<T>(string type) where T : class, Aspect {
            return RegisterAspect(type, (Entity entity, string path) => {
                T aspect = Activator.CreateInstance(typeof(T)) as T;
                return aspect;
            });
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            AspectFactory factory = AspectFactories.GetValue<AspectFactory>(type);
            if (factory != null) {
                return factory(entity, path);
            } else {
                entity.Error("Unknown Aspect Type: {0}, {1}", path, type);
            }
            return null;
        }

        public bool RegisterSpecValueChecker(string propertyType, string specKind, SpecValueCheckerFactory factory) {
            string factoryKey = string.Format("{0}{1}{2}", propertyType, SpecConsts.Separator, specKind);
            return SpecValueCheckerFactories.AddVar(factoryKey, factory) != null;
        }

        public bool FactorySpecValueChecker(Property prop, Pass pass, Data spec, string specKey) {
            string specKind = SpecConsts.GetSpecKind(specKey);
            string factoryKey = string.Format("{0}{1}{2}", prop.Type, SpecConsts.Separator, specKind);
            SpecValueCheckerFactory factory = SpecValueCheckerFactories.GetValue<SpecValueCheckerFactory>(factoryKey);
            if (factory != null) {
                return factory(prop, pass, spec, specKey);
            } else {
                Error("Unknown SpecValueChecker Type: {0}, Spec: {1}", factoryKey, spec);
            }
            return false;
        }
    }
}
