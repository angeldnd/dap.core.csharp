using System;

namespace angeldnd.dap {
    public delegate Entity EntityFactory();
    public delegate Aspect AspectFactory(Entity entity, string path);

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
            result.RegisterAspect<TickItem>(TickItemConsts.TypeTickItem);
            result.RegisterAspect<Properties>(PropertiesConsts.TypeProperties);
            result.RegisterAspect<BoolProperty>(PropertiesConsts.TypeBoolProperty);
            result.RegisterAspect<IntProperty>(PropertiesConsts.TypeIntProperty);
            result.RegisterAspect<LongProperty>(PropertiesConsts.TypeLongProperty);
            result.RegisterAspect<FloatProperty>(PropertiesConsts.TypeFloatProperty);
            result.RegisterAspect<DoubleProperty>(PropertiesConsts.TypeDoubleProperty);
            result.RegisterAspect<StringProperty>(PropertiesConsts.TypeStringProperty);
            result.RegisterAspect<DataProperty>(PropertiesConsts.TypeDataProperty);

            return result;
        }

        public readonly Vars EntityFactories;
        public readonly Vars AspectFactories;

        public Factory() {
            EntityFactories = Add<Vars>("entity_factories");
            AspectFactories = Add<Vars>("aspect_factories");
        }

        public bool RegisterEntity(string type, EntityFactory factory) {
            return EntityFactories.AddVar(type, factory) != null;
        }

        public bool RegisterEntity<T>(string type) where T : Entity {
            return RegisterEntity(type, () => {
                return Activator.CreateInstance(typeof(T)) as T;
            });
        }

        public bool RegisterAspect(string type, AspectFactory factory) {
            return AspectFactories.AddVar(type, factory) != null;
        }

        public bool RegisterAspect<T>(string type) where T : class, Aspect {
            return RegisterAspect(type, (Entity entity, string path) => {
                T aspect = Activator.CreateInstance(typeof(T)) as T;
                if (aspect.Init(entity, path)) {
                    return aspect;
                }
                return null;
            });
        }


        public Entity FactoryEntity(string type) {
            EntityFactory factory = EntityFactories.GetValue<EntityFactory>(type);
            if (factory != null) {
                return factory();
            }
            return null;
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            AspectFactory factory = AspectFactories.GetValue<AspectFactory>(type);
            if (factory != null) {
                return factory(entity, path);
            }
            return null;
        }
    }
}
