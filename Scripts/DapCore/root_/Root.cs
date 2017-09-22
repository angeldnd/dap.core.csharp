using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public static class RootConsts {
        public const string KeyHooks = "Hooks";

        [DapParam(typeof(Data))]
        public const string SummaryPatterns = "patterns";
    }

    public class Root : DictContext<Root, IContext> {
        public static Root GetRoot(IContext context) {
            IContext current = context;
            while (context != null) {
                if (context is Root) {
                    return context as Root;
                }
                context = context.OwnerAsDictContext;
            }
            return null;
        }

        public static string GetContextPath(IContext context) {
            return TreeHelper.GetPath<IContext>(context.Root, context);
        }

        public static string GetAspectPath(IAspect aspect) {
            return TreeHelper.GetPath<IAspect>(aspect.Context, aspect);
        }

        public static string GetAspectUri(IAspect aspect) {
            IContext context = aspect.Context;
            return UriConsts.Encode(context == null ? "" : context.Path, aspect.Path);
        }

        public readonly RootHooks Hooks;

        public Root() : base(null, null) {
            //Can NOT create any aspects other than Hooks here.
            //Also can't use normal add here..
            Hooks = new RootHooks(this, RootConsts.KeyHooks);
        }

        public override string LogPrefix {
            get {
                return string.Format("[Root] {0} ", RevInfo);
            }
        }

        /*
         * Note that this call is not working in constructors (since elements are added
         * to owner after)
         */
        public bool TryGetByUri(string uri, out IContext context, out IAspect aspect) {
            context = null;
            aspect = null;
            if (uri == null) return false;

            string contextPath, aspectPath;
            if (!UriConsts.Decode(uri, out contextPath, out aspectPath)) {
                return false;
            }

            if (string.IsNullOrEmpty(contextPath)) {
                context = this;
            } else {
                context = ContextExtension.GetContext(this, contextPath, true);
            }
            if (context == null) return false;

            if (string.IsNullOrEmpty(aspectPath)) {
                return true;
            } else {
                aspect = context.GetAspect(aspectPath, true);
                return aspect != null;
            }
        }

        public T GetByUri<T>(string uri, bool isDebug = false) where T : class, IContextElement {
            T result = null;
            IContext context;
            IAspect aspect;
            if (TryGetByUri(uri, out context, out aspect)) {
                if (aspect != null) {
                    result = aspect.As<T>(true);
                    if (result == null) {
                        ErrorOrDebug(isDebug, "GetByUri<{0}>({1}): Aspect Type MisMatched: {2}",
                                        typeof(T).FullName, uri, aspect);
                    }
                } else {
                    result = context.As<T>(true);
                    if (result == null) {
                        ErrorOrDebug(isDebug, "GetByUri<{0}>({1}): Context Type MisMatched: {2}",
                                        typeof(T).FullName, uri, context);
                    }
                }
            } else {
                ErrorOrDebug(isDebug, "GetByUri<{0}>({1}): Not Found", typeof(T).FullName, uri);
            }
            return result;
        }
    }
}
