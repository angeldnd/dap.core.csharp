using System;

namespace angeldnd.dap {
    public interface IAspect : IElement {
        IEntity GetEntity();
    }

    public interface IAspect<TE> : IAspect where TE : IEntity {
        TE Entity { get; }
    }

    public interface IAspect<TE, TO> : IAspect<TE>, IElement<TO>
                                            where TE : IEntity<TE>
                                            where TO : ISection<TE> {
    }

    public abstract class Aspect<TE, TO> : Element<TO>, IAspect<TE, TO>
                                                    where TE : IEntity<TE>
                                                    where TO : ISection<TE> {
        public TE Entity {
            get { return Owner.Entity; }
        }

        public Entity GetEntity() {
            return Owner.Entity;
        }

        public Aspect(TO owner, string path, Pass pass) : base(owner, path, pass) {
            Entity = owner.GetEntity();
        }
    }
}
