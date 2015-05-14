using System;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class AutoBootstrap: System.Attribute {
        public AutoBootstrap() {
        }
    }

    public abstract class Bootstrapper {
        /*
         * For automatic running bootstrappers, need to add AutoBootstrap attribute
         * and implement Bootstrap method as below:
         *

         [AutoBootstrap]
         public class TestBootstrapper : Bootstrapper {
            public static bool Bootstrap();
         }

         */
    }
}
