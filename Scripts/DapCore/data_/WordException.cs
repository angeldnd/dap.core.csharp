using System;

namespace angeldnd.dap {
    public class WordException : CaretException {
        public readonly Word Word;

        public override Caret Caret {
            get { return Word == null ? Caret.InvalidCaret : Word.Caret; }
        }

        public override string Hint {
            get { return Word == null ? "" : Word.Value; }
        }

        public WordException(Word word, string format, params object[] values)
                    : base(Log.GetMsg(format, values)) {
            Word = word;
        }

        public WordException(Word word, Exception innerException,
                                string format, params object[] values)
                    : base(Log.GetMsg(format, values), innerException) {
            Word = word;
        }
    }
}

