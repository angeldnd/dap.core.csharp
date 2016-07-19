using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Hooks : TableAspect<IContext, Hook> {
        private enum HookEventKind {
            Invalid, OnContextAdded, OnContextRemoved, OnAspectAdded, OnAspectRemoved
        }

        private class HookEvent {
            public readonly HookEventKind Kind = HookEventKind.Invalid;
            public readonly IContext Context = null;
            public readonly IAspect Aspect = null;

            public HookEvent(HookEventKind kind, IContext context) {
                Kind = kind;
                Context = context;
            }

            public HookEvent(HookEventKind kind, IAspect aspect) {
                Kind = kind;
                Aspect = aspect;
            }
        }

        private List<HookEvent> _PendingEvents = new List<HookEvent>();

        private Hook _DebugHook = null;
        public Hook DebugHook {
            get { return _DebugHook; }
        }

        public Hooks(IContext owner, string key) : base(owner, key) {
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

        /*
         * Treat DebugHook differently since it's better to setup it up before
         * any other operation, though certain operations like Env.GetByUri()
         * might fail if some aspects are created in Context's constructors.
         * Use a spectial trick to trigger the hooks in next tick.
         * Since DebugHook is quite simple, it's fine to be called without delay.
         */

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context) {
            _DebugHook._OnContextAdded(context);
            _PendingEvents.Add(new HookEvent(HookEventKind.OnContextAdded, context));
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            _DebugHook._OnContextRemoved(context);
            _PendingEvents.Add(new HookEvent(HookEventKind.OnContextRemoved, context));
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            _DebugHook._OnAspectAdded(aspect);
            _PendingEvents.Add(new HookEvent(HookEventKind.OnAspectAdded, aspect));
        }

        // Should only be called from IAspect.OnRemoved();
        public void _OnAspectRemoved(IAspect aspect) {
            _DebugHook._OnAspectRemoved(aspect);
            _PendingEvents.Add(new HookEvent(HookEventKind.OnAspectRemoved, aspect));
        }

        public override void OnAdded() {
            //Do Nothing.
        }

        public override void OnRemoved() {
            //Do Nothing.
        }

        // Should only be called from Env.Tick()
        public void _HandlePendingEvents() {
            for (int i = 0; i < _PendingEvents.Count; i++) {
                HookEvent e = _PendingEvents[i];
                switch (e.Kind) {
                    case HookEventKind.OnContextAdded:
                        ForEach((Hook hook) => {
                            if (hook == _DebugHook) return;
                            hook._OnContextAdded(e.Context);
                        });
                        break;
                    case HookEventKind.OnContextRemoved:
                        ForEach((Hook hook) => {
                            if (hook == _DebugHook) return;
                            hook._OnContextRemoved(e.Context);
                        });
                        break;
                    case HookEventKind.OnAspectAdded:
                        ForEach((Hook hook) => {
                            if (hook == _DebugHook) return;
                            hook._OnAspectAdded(e.Aspect);
                        });
                        break;
                    case HookEventKind.OnAspectRemoved:
                        ForEach((Hook hook) => {
                            if (hook == _DebugHook) return;
                            hook._OnAspectRemoved(e.Aspect);
                        });
                        break;
                }
            }
            _PendingEvents.Clear();
        }
    }
}
