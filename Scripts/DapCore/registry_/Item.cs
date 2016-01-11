﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public interface IItem : IContext, IElement {
    }

    public static class ItemConsts {
        public const string TypeItem = "Item";
    }

    public class Item : Element<Registry>, IItem {
        public override string Type {
            get { return ItemConsts.TypeItem; }
        }

        public virtual void OnAspectAdded(IAspect aspect) {
            WeakListHelper.Notify(_Watchers, (IEntityWatcher watcher) => {
                watcher.OnAspectAdded(aspect);
            });
        }

        public virtual void OnAspectRemoved(IAspect aspect) {
            WeakListHelper.Notify(_Watchers, (IEntityWatcher watcher) => {
                watcher.OnAspectRemoved(aspect);
            });
        }

        //SILP: CONTEXT_MIXIN(Registry, Item)
        private readonly Properties _Properties;                                               //__SILP__
        public Properties Properties {                                                         //__SILP__
            get { return _Properties; }                                                        //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        private readonly Channels _Channels;                                                   //__SILP__
        public Channels Channels {                                                             //__SILP__
            get { return _Channels; }                                                          //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        private readonly Handlers _Handlers;                                                   //__SILP__
        public Handlers Handlers {                                                             //__SILP__
            get { return _Handlers; }                                                          //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        private readonly Vars _Vars;                                                           //__SILP__
        public Vars Vars {                                                                     //__SILP__
            get { return _Vars; }                                                              //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public Item(Registry owner, string path, Pass pass) : base(owner, path, pass) {        //__SILP__
            Pass sectionPass = pass.Open;                                                      //__SILP__
                                                                                               //__SILP__
            _Properties = new Properties(this, ContextConsts.SectionProperties, sectionPass);  //__SILP__
            _Channels = new Channels(this, ContextConsts.SectionChannels, sectionPass);        //__SILP__
            _Handlers = new Handlers(this, ContextConsts.SectionHandlers, sectionPass);        //__SILP__
            _Vars = new Vars(this, ContextConsts.SectionVars, sectionPass);                    //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        private bool _DebugMode = false;                                                       //__SILP__
        public override bool DebugMode {                                                       //__SILP__
            get { return _DebugMode; }                                                         //__SILP__
        }                                                                                      //__SILP__
        public void SetDebugMode(bool debugMode) {                                             //__SILP__
            _DebugMode= debugMode;                                                             //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        private string[] _DebugPatterns = {""};                                                //__SILP__
        public override string[] DebugPatterns {                                               //__SILP__
            get { return _DebugPatterns; }                                                     //__SILP__
        }                                                                                      //__SILP__
        public void SetDebugPatterns(string[] patterns) {                                      //__SILP__
            _DebugPatterns = patterns;                                                         //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        private WeakList<IEntityWatcher> _Watchers = null;                                     //__SILP__
                                                                                               //__SILP__
        public int WatcherCount {                                                              //__SILP__
            get { return WeakListHelper.Count(_Watchers); }                                    //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public bool AddWatcher(IEntityWatcher watcher) {                                       //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);                                 //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public bool RemoveWatcher(IEntityWatcher watcher) {                                    //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);                                  //__SILP__
        }                                                                                      //__SILP__
    }
}
