using System;
using System.Collections.Generic;
using MineSweeper.Core.Models;
using MineSweeper.Core.Models.Field;

namespace MineSweeper.Core
{
    public enum FieldType
    {
        Easy,
        Normal,
        Hard
    }

    public static class FieldFactory
    {
        private static readonly Dictionary<FieldType, Type> TypeDict = new Dictionary<FieldType, Type>
        {
            {FieldType.Easy, typeof(EasyBasicField)},
            {FieldType.Normal, typeof(NormalBasicField)},
            {FieldType.Hard, typeof(HardBasicField)},
        };

        public static IField GetPredefinedField(FieldType ft)
        {
            return (IField) Activator.CreateInstance(TypeDict[ft]);
        }

        public static IField GetCustomField(int w, int h, int count)
        {
            return new CustomBasicField(w, h, count);
        }
    }
}