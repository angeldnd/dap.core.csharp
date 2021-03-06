using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public bool MoveToHead(int index) {
            return MoveToHead(Get(index));
        }

        public bool MoveToTail(int index) {
            return MoveToTail(Get(index));
        }

        public bool Swap(int indexA, int indexB) {
            return Swap(Get(indexA), Get(indexB));
        }

        public bool MoveBefore(int index, int anchorIndex) {
            return MoveBefore(Get(index), Get(anchorIndex));
        }

        public bool MoveAfter(int index, int anchorIndex) {
            return MoveAfter(Get(index), Get(anchorIndex));
        }

        private bool CheckElement(T element) {
            if (element != null) {
                int index = element.Index;
                if (index >= 0 && index < _Elements.Count) {
                    return _Elements[index] == element;
                }
            }
            return false;
        }

        public bool MoveToHead(T element) {
            if (!CheckElement(element)) return false;

            if (element.Index != 0) {
                _Elements.RemoveAt(element.Index);
                _Elements.Insert(0, element);
                UpdateIndexes(0);
            }
            return true;
        }

        public bool MoveToTail(T element) {
            if (!CheckElement(element)) return false;

            if (element.Index != _Elements.Count - 1) {
                _Elements.RemoveAt(element.Index);
                _Elements.Add(element);
                UpdateIndexes(element.Index);
            }
            return true;
        }

        public bool Swap(T elementA, T elementB) {
            if (elementA == elementB) return false;
            if (!CheckElement(elementA)) return false;
            if (!CheckElement(elementB)) return false;

            int aIndex = elementA.Index;
            int bIndex = elementB.Index;

            _Elements[aIndex] = elementB;
            elementB._SetIndex(this, aIndex);

            _Elements[bIndex] = elementA;
            elementA._SetIndex(this, bIndex);

            AdvanceRevision();
            return true;
        }

        public bool MoveBefore(T element, T anchor) {
            if (element == anchor) return false;
            if (!CheckElement(element)) return false;
            if (!CheckElement(anchor)) return false;

            if (element.Index != anchor.Index - 1) {
                _Elements.RemoveAt(element.Index);
                if (element.Index < anchor.Index) {
                    _Elements.Insert(anchor.Index - 2, element);
                    UpdateIndexes(element.Index, anchor.Index - 1);
                } else {
                    _Elements.Insert(anchor.Index - 1, element);
                    UpdateIndexes(anchor.Index - 1, element.Index);
                }
            }
            return true;
        }

        public bool MoveAfter(T element, T anchor) {
            if (element == anchor) return false;
            if (!CheckElement(element)) return false;
            if (!CheckElement(anchor)) return false;

            if (element.Index != anchor.Index + 1) {
                _Elements.RemoveAt(element.Index);
                if (element.Index < anchor.Index) {
                    _Elements.Insert(anchor.Index, element);
                    UpdateIndexes(element.Index, anchor.Index);
                } else {
                    _Elements.Insert(anchor.Index + 1, element);
                    UpdateIndexes(anchor.Index + 1, element.Index);
                }
            }
            return true;
        }
    }
}
