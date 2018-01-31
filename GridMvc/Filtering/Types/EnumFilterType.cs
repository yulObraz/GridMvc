using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using GridMvc.Utility;

namespace GridMvc.Filtering.Types {
    internal sealed class EnumFilterType : FilterTypeBase {
        private Type _type;
        public EnumFilterType(Type type) {
            this._type = type;
        }
        public override Type TargetType {
            get { return _type; }
        }

        public override GridFilterType GetValidType(GridFilterType type) {
            switch(type) {
                case GridFilterType.Equals:
                case GridFilterType.GreaterThan://not supported in ui
                case GridFilterType.LessThan://not supported in ui
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        public override object GetTypedValue(string value) {
            IEnumerable<object> result = null;
            try {
                var values = value.Split(',').ToList().Select(it => it.Trim()).ToList();
                result = values.Where(it => !string.IsNullOrEmpty(it)).Select(it => Enum.Parse(TargetType, it, true));
            } catch { }
            if(result == null || !result.Any()) {
                return null;
            }
            return result;
        }

        public override Expression GetFilterExpression(Expression leftExpr, string value, GridFilterType filterType) {
            //Custom implementation of string filter type. Case insensitive comparison.
            filterType = GetValidType(filterType);

            object typedValues = GetTypedValue(value);

            if(typedValues == null)
                return null; // incorrect filter value;

            Expression current = null;
            foreach(var typedValue in (typedValues as IEnumerable<object>)) {
                Expression valueExpr = Expression.ConvertChecked(Expression.Constant(typedValue), TargetType);
                Expression currentExpr = null;
                switch(filterType) {
                    case GridFilterType.Equals:
                        currentExpr = Expression.Equal(leftExpr, valueExpr);
                        break;
                    case GridFilterType.LessThan:
                        currentExpr = Expression.LessThan(leftExpr, valueExpr);
                        break;
                    case GridFilterType.GreaterThan:
                        currentExpr = Expression.GreaterThan(leftExpr, valueExpr);
                        break;
                }
                if(current == null) {
                    current = currentExpr;
                } else {
                    current = Expression.Or(current, currentExpr);// have no sense for less and greater but doesn't give problems
                }
            }
            return current;
        }
        public override string DefaultWidgetName {
            get { 
                //bool isFlag = TargetType.IsDefined(typeof(FlagsAttribute), inherit: false);
                return "Enum"; 
            }
        }

        public override object DefaultWidgetData {
            get {
                List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
                foreach(var e in Enum.GetValues(TargetType)) {
                    var display = TargetType.GetField(e.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>().FirstOrDefault();
                    var displayName = (display == null ? e.ToString() : display.Name);
                    list.Add(new System.Web.Mvc.SelectListItem() { Value = ((int)e).ToString(), Text = displayName });
                }
                return list;
            }
        }
    }
}

