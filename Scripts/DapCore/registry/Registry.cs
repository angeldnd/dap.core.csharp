using System;
using System.Collections.Generic;

namespace ADD.Dap {
    public struct RegistryConsts {
        public const char Separator = '/';
    }

    public class Registry : Context {
        public override char Separator {
            get { return RegistryConsts.Separator; }
        }

        public static Registry Global = new Registry();

        public readonly Factory Factory;

        public Registry(Factory factory) {
            Factory = factory;
        }

        public Registry() {
            Factory = Factory.NewBuiltinFactory();
        }

        public Item AddItem(string path, string type) {
            if (!Has(path)) {
                Aspect aspect = Factory.FactoryAspect(this, path, type);
                if (aspect != null && aspect is Item) {
                    SetAspect(aspect);
                    return aspect as Item;
                }
            }
            return null;
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            return Factory.FactoryAspect(entity, path, type);
        }
    }
}
