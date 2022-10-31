using CW_ToyShopping.Enity.PublicModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace CW_ToyShopping.Common.Helpers
{
    /// <summary>
    /// DataTable转换类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableHelper<T> where T: class
    {
        public static List<T> DataTableConvertToModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<T> modelList = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                T model = (T)Activator.CreateInstance(typeof(T));
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, Convert.ChangeType(dr[i], propertyInfo.PropertyType), null);
                }
                modelList.Add(model);
            }
            return modelList;
        }

        public static DataTable MondelConverToDatable(List<T> model, List<DicModel> dic) {

            if (model.Count <= 0 || model == null) {
                return null;
            }

            DataTable result = new DataTable();

            PropertyInfo[] propertyInfos = model[0].GetType().GetProperties();

            foreach (PropertyInfo pi in propertyInfos) 
            {
                if (dic.Exists(val => val.Dbcol == pi.Name)) 
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
            }
            for (int i = 0; i < model.Count; i++) 
            {
                ArrayList tempList = new ArrayList();
                foreach (PropertyInfo pi in propertyInfos)
                {
                    if (dic.Exists(val => val.Dbcol == pi.Name))
                    {
                        object obj = pi.GetValue(model[i], null);
                        tempList.Add(obj);
                    }
                }
                object[] array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
          
            return result;
        }
    }
}
