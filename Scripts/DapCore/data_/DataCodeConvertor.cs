using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class DataCodeConvertorConsts {
        public const string NullStr = "null";

        public const string DataBegin = "new Data()";
        public const string TypeBegin = ".";
        public const string KeyBegin = "(";
        public const string ValueBegin = ", ";
        public const string ValueEnd = ")";
        public const string StringBegin = "\"";
        public const string StringEnd = "\"";

        public const string LongEnd = "l";
        public const string FloatEnd = "f";
        public const string DoubleEnd = "d";

        public readonly static char[] WordChars = new char[]{'"'};

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

    public class DataCodeConvertor : Convertor<Data> {
        public void Convert(Data val, string prefix, string suffix, string linePrefix, string indent, Action<string> callback) {
            if (val == null) {
                callback(string.Format("{0}{1}null;", linePrefix, prefix));
                return;
            }

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            Action appendLine = () => {
                callback(builder.ToString());
                builder.Length = 0;
            };
            builder.Append(string.Format("{0}{1}", linePrefix, prefix));
            AppendData(builder, appendLine, linePrefix, indent, 0, val);
            builder.Append(suffix);
            appendLine();
        }

        public override string Convert(Data val) {
            List<string> lines = new List<string>();
            Convert(val, "Data data = ", ";", "", "    ", (string line) => {
                lines.Add(line);
            });
            return string.Join("\n", lines.ToArray());
        }

        public override Data Parse(string str) {
            throw new NotSupportedException("Parse Not Supported!");
        }

        private void AppendIndents(StringBuilder builder, string linePrefix, string indent, int indentLevel) {
            if (!string.IsNullOrEmpty(linePrefix)) {
                builder.Append(linePrefix);
            }

            if (indentLevel <= 0 || indent == null) return;

            for (int i = 0; i < indentLevel; i++) {
                builder.Append(indent);
            }
        }

        private void AppendData(StringBuilder builder, Action appendLine,
                                string linePrefix, string indent, int indentLevel, Data data) {
            if (data == null) {
                builder.Append(DataCodeConvertorConsts.NullStr);
                return;
            }

            builder.Append(DataCodeConvertorConsts.DataBegin);
            if (data.Count > 0) {
                foreach (string key in data.Keys) {
                    appendLine();
                    AppendValue(builder, appendLine, linePrefix, indent, indentLevel + 1, data, key);
                }
            }
        }

        public void AppendValue(StringBuilder builder, Action appendLine,
                                string linePrefix, string indent, int indentLevel, Data data, string key) {
            AppendIndents(builder, linePrefix, indent, indentLevel);

            DataType valueType = data.GetValueType(key);
            builder.Append(DataCodeConvertorConsts.TypeBegin);
            builder.Append(Convertor.DataTypeConvertor.Convert(valueType));

            builder.Append(DataCodeConvertorConsts.KeyBegin);
            builder.Append(DataCodeConvertorConsts.StringBegin);
            AppendString(builder, key);
            builder.Append(DataCodeConvertorConsts.StringEnd);

            builder.Append(DataCodeConvertorConsts.ValueBegin);

            switch (valueType) {
                case DataType.Bool:
                    builder.Append(Convertor.BoolConvertor.Convert(data.GetBool(key)));
                    break;
                case DataType.Int:
                    builder.Append(Convertor.IntConvertor.Convert(data.GetInt(key)));
                    break;
                case DataType.Long:
                    builder.Append(Convertor.LongConvertor.Convert(data.GetLong(key)));
                    builder.Append(DataCodeConvertorConsts.LongEnd);
                    break;
                case DataType.Float:
                    builder.Append(Convertor.FloatConvertor.Convert(data.GetFloat(key)));
                    builder.Append(DataCodeConvertorConsts.FloatEnd);
                    break;
                case DataType.Double:
                    builder.Append(Convertor.DoubleConvertor.Convert(data.GetDouble(key)));
                    builder.Append(DataCodeConvertorConsts.DoubleEnd);
                    break;
                case DataType.String:
                    builder.Append(DataCodeConvertorConsts.StringBegin);
                    AppendString(builder, data.GetString(key));
                    builder.Append(DataCodeConvertorConsts.StringEnd);
                    break;
                case DataType.Data:
                    AppendData(builder, appendLine, linePrefix, indent, indentLevel, data.GetData(key));
                    break;
            }
            builder.Append(DataCodeConvertorConsts.ValueEnd);
        }

        public void AppendString(StringBuilder builder, string str) {
            if (string.IsNullOrEmpty(str)) return;

            for (int i = 0; i < str.Length; i++) {
                char ch = str[i];
                if (ch == '\n') {
                    builder.Append("\\n");
                } else if (ch == '\r') {
                    builder.Append("\\r");
                } else if (ch == '\t') {
                    builder.Append("\\t");
                } else {
                    if (DataCodeConvertorConsts.IsWordChar(ch)) {
                        builder.Append(WordSplitterConsts.EscapeChar);
                    }
                    builder.Append(ch);
                }

            }
        }
    }
}

