using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class DataConvertorConsts {
        public const string DataBegin = "{";
        public const string DataEnd = "}";
        public const string KeyBegin = ":";
        public const string ValueBegin = "=";

        public const string Space = " ";

        public readonly static char[] WordChars = new char[]{':', '=', '{', '}'};

        public static bool IsWordChar(char ch) {
            foreach (char word in WordChars) {
                if (word == ch) return true;
            }
            return false;
        }

        public static bool IsWordChar(String str) {
            foreach (char word in WordChars) {
                if (word.ToString() == str) return true;
            }
            return false;
        }
    }

    public class PartialData {
        public enum ExpectKind {Type = 0, Key, Value};

        public DataType ValueType = DataType.Invalid;
        public string Key = null;

        public void Clear() {
            ValueType = DataType.Invalid;
            Key = null;
        }

        public ExpectKind GetExpectKind() {
            if (ValueType == DataType.Invalid) {
                return ExpectKind.Type;
            } else if (Key == null) {
                return ExpectKind.Key;
            } else {
                return ExpectKind.Value;
            }
        }
    }

    public class DataConvertor : Convertor<Data> {
        public bool TryParse(string source, string content, out Data val, bool isDebug = false) {
            try {
                val = Parse(source, content);
                return true;
            } catch (Exception e) {
                Log.ErrorOrDebug(isDebug, "Parse Failed: <{0}> {1}\n\n{2}\n\n{3}",
                        typeof(Data).FullName,
                        CaretException.GetMessage(source, e),
                        e.StackTrace, WordSplitter.AppendLineNumber(content));
            }
            val = null;
            return false;
        }

        public string Convert(Data val, string indent) {
            if (val == null) return Convertor.Null;
            if (indent == "") indent = null;

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            AppendData(builder, indent, 0, val);
            return builder.ToString();
        }

        public override string Convert(Data val) {
            return Convert(val, "\t");
        }

        public override Data Parse(string str) {
            return Parse(string.Empty, str);
        }

        public Data Parse(string source, string content) {
            if (string.IsNullOrEmpty(content) || content == Convertor.Null) return null;

            Stack<Data> dataStack = new Stack<Data>();
            Data lastData = null;
            PartialData partialData = new PartialData();
            partialData.ValueType = DataType.Data;
            partialData.Key = "";
            Word lastWord = new Word(source, 0, 0, DataConvertorConsts.ValueBegin);

            WordSplitter.Split(source, content, DataConvertorConsts.WordChars, (Word word) => {
                ProcessWord(dataStack, ref lastData, partialData, lastWord, word);
                lastWord = word;
            });

            if (dataStack.Count == 0 && lastData != null) {
                return lastData;
            } else {
                throw new WordException(lastWord, "Data Stack Error: {0}, {1}", dataStack.Count, lastData);
            }
        }

        private void ProcessWord(Stack<Data> dataStack, ref Data lastData,
                                    PartialData partialData,
                                    Word lastWord, Word word) {
            switch (partialData.GetExpectKind()) {
                case PartialData.ExpectKind.Type:
                    ProcessType(dataStack, ref lastData, partialData, lastWord, word);
                    break;
                case PartialData.ExpectKind.Key:
                    ProcessKey(dataStack, ref lastData, partialData, lastWord, word);
                    break;
                case PartialData.ExpectKind.Value:
                    ProcessValue(dataStack, ref lastData, partialData, lastWord, word);
                    break;
            }
        }

        private void PopDataFromStack(Stack<Data> dataStack, ref Data lastData, Word word) {
            if (dataStack.Count == 0) {
                throw new WordException(word, "Empty DataStack");
            }
            lastData = dataStack.Pop();
        }

        private void ProcessType(Stack<Data> dataStack, ref Data lastData,
                                    PartialData partialData,
                                    Word lastWord, Word word) {
            if (DataConvertorConsts.IsWordChar(word.Value)) {
                if (word.Value == DataConvertorConsts.DataEnd) {
                    PopDataFromStack(dataStack, ref lastData, word);
                } else {
                    throw new WordException(word, "Expecting DataType");
                }
            } else {
                partialData.ValueType = Convertor.DataTypeConvertor.Parse(word.Value);
                if (partialData.ValueType == DataType.Invalid) {
                    throw new WordException(word, "Invalid DataType");
                }
            }
        }

        private void ProcessKey(Stack<Data> dataStack, ref Data lastData,
                                    PartialData partialData,
                                    Word lastWord, Word word) {
            if (DataConvertorConsts.IsWordChar(word.Value)) {
                if (lastWord.Value == DataConvertorConsts.KeyBegin
                        || word.Value != DataConvertorConsts.KeyBegin) {
                    throw new WordException(word, "Syntax Error");
                }
            } else if (lastWord.Value == DataConvertorConsts.KeyBegin) {
                partialData.Key = word.Value;
            } else {
                throw new WordException(word, "Syntax Error");
            }
        }

        private void ProcessValue(Stack<Data> dataStack, ref Data lastData,
                                    PartialData partialData,
                                    Word lastWord, Word word) {
            Data data = dataStack.Count > 0 ? dataStack.Peek() : null;
            if (DataConvertorConsts.IsWordChar(word.Value)) {
                if (word.Value == DataConvertorConsts.DataBegin) {
                    Data subData = new RealData();
                    dataStack.Push(subData);
                    if (data != null) {
                        data.SetData(partialData.Key, subData);
                    }
                    partialData.Clear();
                } else if (word.Value == DataConvertorConsts.DataEnd) {
                    if (partialData.ValueType == DataType.String) {
                        SetSimpleDataValue(data, partialData, new Word(word.Caret, ""));
                        PopDataFromStack(dataStack, ref lastData, word);
                    } else {
                        throw new WordException(word, "Missing Value");
                    }
                } else if (lastWord.Value == DataConvertorConsts.ValueBegin
                        || word.Value != DataConvertorConsts.ValueBegin) {
                    throw new WordException(word, "Syntax Error");
                }
            } else if (lastWord.Value == DataConvertorConsts.ValueBegin) {
                SetSimpleDataValue(data, partialData, word);
            } else {
                throw new WordException(word, "Syntax Error");
            }
        }

        private void SetSimpleDataValue(Data data, PartialData partialData, Word word) {
            if (data == null) {
                throw new WordException(word, "Syntax Error");
            }
            bool ok = false;
            switch (partialData.ValueType) {
                case DataType.Bool:
                    ok = data.SetBool(partialData.Key, Convertor.BoolConvertor.Parse(word.Value));
                    break;
                case DataType.Int:
                    ok = data.SetInt(partialData.Key, Convertor.IntConvertor.Parse(word.Value));
                    break;
                case DataType.Long:
                    ok = data.SetLong(partialData.Key, Convertor.LongConvertor.Parse(word.Value));
                    break;
                case DataType.Float:
                    ok = data.SetFloat(partialData.Key, Convertor.FloatConvertor.Parse(word.Value));
                    break;
                case DataType.Double:
                    ok = data.SetDouble(partialData.Key, Convertor.DoubleConvertor.Parse(word.Value));
                    break;
                case DataType.String:
                    ok = data.SetString(partialData.Key, Convertor.StringConvertor.Parse(word.Value));
                    break;
                case DataType.Data:
                    if (word.Value == Convertor.Null) {
                        ok = data.SetData(partialData.Key, null);
                    }
                    break;
            }
            if (ok) {
                partialData.Clear();
            } else {
                throw new WordException(word, "Syntax Error");
            }
        }

        private void AppendIndents(StringBuilder builder, string indent, int indentLevel) {
            if (indentLevel <= 0 || indent == null) return;

            for (int i = 0; i < indentLevel; i++) {
                builder.Append(indent);
            }
        }

        private void AppendData(StringBuilder builder, string indent, int indentLevel, Data data) {
            if (data == null) {
                builder.Append(Convertor.Null);
                return;
            }

            builder.Append(DataConvertorConsts.DataBegin);
            if (indent != null && data.Count > 0) {
                builder.AppendLine();
            }

            AppendDataValues(builder, indent, indentLevel, data);

            if (indent != null && data.Count > 0) {
                AppendIndents(builder, indent, indentLevel);
            }
            builder.Append(DataConvertorConsts.DataEnd);
        }

        protected void AppendValue(StringBuilder builder, string indent, int indentLevel, Data data, string key, string end) {
            DataType valueType = data.GetValueType(key);

            AppendIndents(builder, indent, indentLevel);
            AppendTypeAndKey(builder, valueType, key, indent != null);

            switch (valueType) {
                case DataType.Bool:
                    builder.Append(Convertor.BoolConvertor.Convert(data.GetBool(key)));
                    break;
                case DataType.Int:
                    builder.Append(Convertor.IntConvertor.Convert(data.GetInt(key)));
                    break;
                case DataType.Long:
                    builder.Append(Convertor.LongConvertor.Convert(data.GetLong(key)));
                    break;
                case DataType.Float:
                    builder.Append(Convertor.FloatConvertor.Convert(data.GetFloat(key)));
                    break;
                case DataType.Double:
                    builder.Append(Convertor.DoubleConvertor.Convert(data.GetDouble(key)));
                    break;
                case DataType.String:
                    AppendString(builder, data.GetString(key));
                    break;
                case DataType.Data:
                    AppendData(builder, indent, indentLevel, data.GetData(key));
                    break;
            }
            if (end != null) {
                builder.Append(end);
            }
            if (indent != null) {
                builder.AppendLine();
            } else {
                builder.Append(DataConvertorConsts.Space);
            }
        }

        protected virtual void AppendDataValues(StringBuilder builder, string indent, int indentLevel, Data data) {
            foreach (string key in data.Keys) {
                AppendValue(builder, indent, indentLevel + 1, data, key, null);
            }
        }

        protected virtual void AppendTypeAndKey(StringBuilder builder, DataType valueType, string key, bool withSpace) {
            builder.Append(Convertor.DataTypeConvertor.Convert(valueType));
            builder.Append(DataConvertorConsts.KeyBegin);
            if (withSpace) {
                builder.Append(DataConvertorConsts.Space);
            }
            AppendString(builder, key);
            if (withSpace) {
                builder.Append(DataConvertorConsts.Space);
            }
            builder.Append(DataConvertorConsts.ValueBegin);
            if (withSpace) {
                builder.Append(DataConvertorConsts.Space);
            }
        }

        protected virtual void AppendString(StringBuilder builder, string str) {
            if (string.IsNullOrEmpty(str)) {
                builder.Append(WordSplitterConsts.EncloseBeginChar);
                builder.Append(WordSplitterConsts.EncloseEndChar);
                return;
            };

            for (int i = 0; i < str.Length; i++) {
                char ch = str[i];
                if (WordSplitterConsts.IsEmptyChar(ch)
                        || WordSplitterConsts.IsEncloseChar(ch)
                        || DataConvertorConsts.IsWordChar(ch)) {
                    builder.Append(WordSplitterConsts.EscapeChar);
                }
                builder.Append(ch);
            }
        }
    }
}

