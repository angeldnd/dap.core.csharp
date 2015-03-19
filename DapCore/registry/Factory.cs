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

            //Vars
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeVars, Vars);
            result.RegisterAspect(VarsConsts.TypeVars, (Entity entity, string path) => { //__SILP__
                Vars aspect = new Vars();                                                //__SILP__
                if (aspect.Init(entity, path)) {                                         //__SILP__
                    return aspect;                                                       //__SILP__
                }                                                                        //__SILP__
                return null;                                                             //__SILP__
            });                                                                          //__SILP__
                                                                                         //__SILP__

            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeBoolVar, BoolVar);
            result.RegisterAspect(VarsConsts.TypeBoolVar, (Entity entity, string path) => { //__SILP__
                BoolVar aspect = new BoolVar();                                             //__SILP__
                if (aspect.Init(entity, path)) {                                            //__SILP__
                    return aspect;                                                          //__SILP__
                }                                                                           //__SILP__
                return null;                                                                //__SILP__
            });                                                                             //__SILP__
                                                                                            //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeIntVar, IntVar);
            result.RegisterAspect(VarsConsts.TypeIntVar, (Entity entity, string path) => { //__SILP__
                IntVar aspect = new IntVar();                                              //__SILP__
                if (aspect.Init(entity, path)) {                                           //__SILP__
                    return aspect;                                                         //__SILP__
                }                                                                          //__SILP__
                return null;                                                               //__SILP__
            });                                                                            //__SILP__
                                                                                           //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeLongVar, LongVar);
            result.RegisterAspect(VarsConsts.TypeLongVar, (Entity entity, string path) => { //__SILP__
                LongVar aspect = new LongVar();                                             //__SILP__
                if (aspect.Init(entity, path)) {                                            //__SILP__
                    return aspect;                                                          //__SILP__
                }                                                                           //__SILP__
                return null;                                                                //__SILP__
            });                                                                             //__SILP__
                                                                                            //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeFloatVar, FloatVar);
            result.RegisterAspect(VarsConsts.TypeFloatVar, (Entity entity, string path) => { //__SILP__
                FloatVar aspect = new FloatVar();                                            //__SILP__
                if (aspect.Init(entity, path)) {                                             //__SILP__
                    return aspect;                                                           //__SILP__
                }                                                                            //__SILP__
                return null;                                                                 //__SILP__
            });                                                                              //__SILP__
                                                                                             //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeDoubleVar, DoubleVar);
            result.RegisterAspect(VarsConsts.TypeDoubleVar, (Entity entity, string path) => { //__SILP__
                DoubleVar aspect = new DoubleVar();                                           //__SILP__
                if (aspect.Init(entity, path)) {                                              //__SILP__
                    return aspect;                                                            //__SILP__
                }                                                                             //__SILP__
                return null;                                                                  //__SILP__
            });                                                                               //__SILP__
                                                                                              //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeStringVar, StringVar);
            result.RegisterAspect(VarsConsts.TypeStringVar, (Entity entity, string path) => { //__SILP__
                StringVar aspect = new StringVar();                                           //__SILP__
                if (aspect.Init(entity, path)) {                                              //__SILP__
                    return aspect;                                                            //__SILP__
                }                                                                             //__SILP__
                return null;                                                                  //__SILP__
            });                                                                               //__SILP__
                                                                                              //__SILP__
            //SILP: REGISTER_ASPECT_FACTORY(VarsConsts.TypeDataVar, DataVar);
            result.RegisterAspect(VarsConsts.TypeDataVar, (Entity entity, string path) => { //__SILP__
                DataVar aspect = new DataVar();                                             //__SILP__
                if (aspect.Init(entity, path)) {                                            //__SILP__
                    return aspect;                                                          //__SILP__
                }                                                                           //__SILP__
                return null;                                                                //__SILP__
            });                                                                             //__SILP__
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

        public readonly Vars EntityFactories;
        public readonly Vars AspectFactories;

        public Factory() {
            EntityFactories = Add<Vars>("entity_factories");
            AspectFactories = Add<Vars>("aspect_factories");
        }

        public bool RegisterEntity(string type, EntityFactory factory) {
            return EntityFactories.AddAnyVar<EntityFactory>(type, factory) != null;
        }

        public bool RegisterAspect(string type, AspectFactory factory) {
            return AspectFactories.AddAnyVar(type, factory) != null;
        }

        public Entity FactoryEntity(string type) {
            EntityFactory factory = EntityFactories.GetAnyValue<EntityFactory>(type);
            if (factory != null) {
                return factory();
            }
            return null;
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            AspectFactory factory = AspectFactories.GetAnyValue<AspectFactory>(type);
            if (factory != null) {
                return factory(entity, path);
            }
            return null;
        }
    }
}
