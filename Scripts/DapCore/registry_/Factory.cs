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
            //SILP: REGISTER_ENTITY_FACTORY(ContextConsts.TypeContext, Context);
            result.RegisterEntity(ContextConsts.TypeContext, () => {  //__SILP__
                return new Context();                                 //__SILP__
            });                                                       //__SILP__

            //Aspects
            //SILP: REGISTER_ASPECT_FACTORY(ItemConsts.TypeItem, Item)
            result.RegisterAspect(ItemConsts.TypeItem, (Entity entity, string path) => { //__SILP__
                Item aspect = new Item();                                                //__SILP__
                if (aspect.Init(entity, path)) {                                         //__SILP__
                    return aspect;                                                       //__SILP__
                }                                                                        //__SILP__
                return null;                                                             //__SILP__
            });                                                                          //__SILP__
                                                                                         //__SILP__

            //Properties
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeProperties, Properties);
            result.RegisterAspect(PropertiesConsts.TypeProperties, (Entity entity, string path) => { //__SILP__
                Properties aspect = new Properties();                                                //__SILP__
                if (aspect.Init(entity, path)) {                                                     //__SILP__
                    return aspect;                                                                   //__SILP__
                }                                                                                    //__SILP__
                return null;                                                                         //__SILP__
            });                                                                                      //__SILP__
                                                                                                     //__SILP__

            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeBoolProperty, BoolProperty);
            result.RegisterAspect(PropertiesConsts.TypeBoolProperty, (Entity entity, string path) => { //__SILP__
                BoolProperty aspect = new BoolProperty();                                              //__SILP__
                if (aspect.Init(entity, path)) {                                                       //__SILP__
                    return aspect;                                                                     //__SILP__
                }                                                                                      //__SILP__
                return null;                                                                           //__SILP__
            });                                                                                        //__SILP__
                                                                                                       //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeIntProperty, IntProperty);
            result.RegisterAspect(PropertiesConsts.TypeIntProperty, (Entity entity, string path) => { //__SILP__
                IntProperty aspect = new IntProperty();                                               //__SILP__
                if (aspect.Init(entity, path)) {                                                      //__SILP__
                    return aspect;                                                                    //__SILP__
                }                                                                                     //__SILP__
                return null;                                                                          //__SILP__
            });                                                                                       //__SILP__
                                                                                                      //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeLongProperty, LongProperty);
            result.RegisterAspect(PropertiesConsts.TypeLongProperty, (Entity entity, string path) => { //__SILP__
                LongProperty aspect = new LongProperty();                                              //__SILP__
                if (aspect.Init(entity, path)) {                                                       //__SILP__
                    return aspect;                                                                     //__SILP__
                }                                                                                      //__SILP__
                return null;                                                                           //__SILP__
            });                                                                                        //__SILP__
                                                                                                       //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeFloatProperty, FloatProperty);
            result.RegisterAspect(PropertiesConsts.TypeFloatProperty, (Entity entity, string path) => { //__SILP__
                FloatProperty aspect = new FloatProperty();                                             //__SILP__
                if (aspect.Init(entity, path)) {                                                        //__SILP__
                    return aspect;                                                                      //__SILP__
                }                                                                                       //__SILP__
                return null;                                                                            //__SILP__
            });                                                                                         //__SILP__
                                                                                                        //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeDoubleProperty, DoubleProperty);
            result.RegisterAspect(PropertiesConsts.TypeDoubleProperty, (Entity entity, string path) => { //__SILP__
                DoubleProperty aspect = new DoubleProperty();                                            //__SILP__
                if (aspect.Init(entity, path)) {                                                         //__SILP__
                    return aspect;                                                                       //__SILP__
                }                                                                                        //__SILP__
                return null;                                                                             //__SILP__
            });                                                                                          //__SILP__
                                                                                                         //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeStringProperty, StringProperty);
            result.RegisterAspect(PropertiesConsts.TypeStringProperty, (Entity entity, string path) => { //__SILP__
                StringProperty aspect = new StringProperty();                                            //__SILP__
                if (aspect.Init(entity, path)) {                                                         //__SILP__
                    return aspect;                                                                       //__SILP__
                }                                                                                        //__SILP__
                return null;                                                                             //__SILP__
            });                                                                                          //__SILP__
                                                                                                         //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(PropertiesConsts.TypeDataProperty, DataProperty);
            result.RegisterAspect(PropertiesConsts.TypeDataProperty, (Entity entity, string path) => { //__SILP__
                DataProperty aspect = new DataProperty();                                              //__SILP__
                if (aspect.Init(entity, path)) {                                                       //__SILP__
                    return aspect;                                                                     //__SILP__
                }                                                                                      //__SILP__
                return null;                                                                           //__SILP__
            });                                                                                        //__SILP__
                                                                                                       //__SILP__
            return result;
        }

        public readonly Vars<EntityFactory> EntityFactories;
        public readonly Vars<AspectFactory> AspectFactories;

        public Factory() {
            EntityFactories = Add<Vars<EntityFactory>>("entity_factories");
            AspectFactories = Add<Vars<AspectFactory>>("aspect_factories");
        }

        public bool RegisterEntity(string type, EntityFactory factory) {
            return EntityFactories.AddVar(type, factory) != null;
        }

        public bool RegisterAspect(string type, AspectFactory factory) {
            return AspectFactories.AddVar(type, factory) != null;
        }

        public Entity FactoryEntity(string type) {
            EntityFactory factory = EntityFactories.GetValue<EntityFactory>(type);
            if (factory != null) {
                return factory();
            }
            return null;
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            AspectFactory factory = AspectFactories.GetValue(type);
            if (factory != null) {
                return factory(entity, path);
            }
            return null;
        }
    }
}
