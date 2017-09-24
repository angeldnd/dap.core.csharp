using System;

namespace angeldnd.dap {
    public sealed class Word {
        public readonly Caret Caret;
        public readonly string Value;

        public Word(string source, int line, int column, string val) {
            Caret = new Caret(source, line, column);
            Value = val;
        }

        public Word(Caret caret, string val) {
            Caret = caret;
            Value = val;
        }

        public override string ToString() {
            return string.Format("[Word: {0} {1}]", Caret, Value);
        }
    }
}
