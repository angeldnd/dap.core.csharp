using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class SpecConsts {
        public const string KeySpec = "spec";

        public const string KindBigger = ">";
        public const string KindBiggerOrEqual = ">=";
        public const string KindSmaller = "<";
        public const string KindSmallerOrEqual = "<=";

        //Note that since the == and != check is not reliable for
        //float and double, not adding these kinds for them.
        public const string KindIn = "~";
        public const string KindNotIn = "!~";

        public const string Separator = " ";
        public const string SubPrefix = ".";

        public static string GetSubSpecKey(string subKey, string SpecKind) {
            return string.Format("{0}{1}{2}{3}", SubPrefix, subKey, Separator, SpecKind);
        }

        public static string GetSubKey(string specKey) {
            if (specKey.StartsWith(SubPrefix)) {
                int index = specKey.IndexOf(Separator);
                if (index > 0) {
                    return specKey.Substring(0, index).Substring(SubPrefix.Length - 1);
                }
            }
            return null;
        }

        public static string GetSpecKind(string specKey) {
            if (specKey.StartsWith(SubPrefix)) {
                int index = specKey.IndexOf(Separator);
                if (index > 0) {
                    return specKey.Substring(index + Separator.Length);
                }
            }
            return specKey;
        }
    }

    /*
     * Here the factory will add checker to property directly, since the checker
     * need type of the value, which is not available in Property, so we need to do
     * casting in the factory method, so don't want to cast again just for adding
     * the checker later.
     */
    public delegate bool SpecValueCheckerFactory(IProperty prop, Data spec, string specKey);

    public static class Spec {
        private static Vars _SpecValueCheckerFactories = new Vars(Env.Instance, "SpecValueCheckerFactories");

        static Spec() {
            BuiltInSpecFactory.RegistrySpecValueCheckers();
        }

        public static bool RegisterSpecValueChecker(string propertyType, string specKind, SpecValueCheckerFactory factory) {
            string factoryKey = string.Format("{0}{1}{2}", propertyType, SpecConsts.Separator, specKind);
            return _SpecValueCheckerFactories.AddVar(factoryKey, factory) != null;
        }

        public static bool FactorySpecValueChecker(IProperty prop, Data spec, string specKey) {
            string specKind = SpecConsts.GetSpecKind(specKey);
            string factoryKey = string.Format("{0}{1}{2}", prop.DapType, SpecConsts.Separator, specKind);
            SpecValueCheckerFactory factory = _SpecValueCheckerFactories.GetValue<SpecValueCheckerFactory>(factoryKey);
            if (factory != null) {
                return factory(prop, spec, specKey);
            } else {
                Log.Error("Unknown SpecValueChecker Type: {0}, Spec: {1}", factoryKey, spec);
            }
            return false;
        }
    }
}
