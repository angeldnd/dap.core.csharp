using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Hooks : TableAspect<IContext, Hook> {
        private Hook _DebugHook = null;
        public Hook DebugHook {
            get { return _DebugHook; }
        }

        public Hooks(IContext owner, string key) : base(owner, key) {
        }

        public void Setup() {
            if (_DebugHook != null) {
                Error("Already Setup");
                return;
            }
            _DebugHook = Add();
            _DebugHook.Setup(
                "DebugHook",
                (IContext context) => {
                    context.Debugging = true;
                },
                (IAspect aspect) => {
                    aspect.Debugging = true;
                }
            );
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context) {
            ForEach((Hook hook) => {
                hook._OnContextAdded(context);
            });
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            ForEach((Hook hook) => {
                hook._OnContextRemoved(context);
            });
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            ForEach((Hook hook) => {
                hook._OnAspectAdded(aspect);
            });
        }

        // Should only be called from IAspect.OnRemoved();
        public void _OnAspectRemoved(IAspect aspect) {
            ForEach((Hook hook) => {
                hook._OnAspectRemoved(aspect);
            });
        }
    }
}
