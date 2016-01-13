using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IInTableElement : IElement {
        /*
         * Constructor(TO owner, Pass pass)
         */
    }

    public interface IInTableElement<TO> : IElement<TO>, IInTableElement
                                            where TO : ITable {
    }

    public interface ITable : IOwner {
    }

    public interface ITable<T> : ITable
                                    where T : class, IInTableElement {

    }

    public abstract partial class Table<T> : Object, ITable<T>
                                                where T : class, IInTableElement {
        protected Table(Pass pass) : base(pass) {
        }
    }
}
