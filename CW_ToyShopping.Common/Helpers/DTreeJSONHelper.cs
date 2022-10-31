using CW_ToyShopping.Enity.AdminModels.MenuModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CW_ToyShopping.Common.Helpers
{
   public class DTreeJSONHelper
    {    
        public DTreeJSONHelper()
        {

        }
        /*public static List<MenuDto> GetTypeOfWorkforTree(List<Menu> treeNodes, List<MenuDto> resps, int ParentId)
        {
            resps = new List<MenuDto>();

            List<Menu> tempList = treeNodes.Where(c => c.PID == ParentId).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                MenuDto node = new MenuDto();
                node.MENUID = tempList[i].MENUID;
                node.PID = tempList[i].PID;
                node.AUTHNAME = tempList[i].AUTHNAME;
                node.PATH = tempList[i].PATH;
                node.RootIntroduction = tempList[i].RootIntroduction;
                node.IsEnble = tempList[i].IsEnble;
                node.CREATEPERSON = tempList[i].CREATEPERSON;
                node.CREATEDATE = tempList[i].CREATEDATE;
                node.UPDATEPERSON = tempList[i].UPDATEPERSON;
                node.UPDATEDATE = tempList[i].UPDATEDATE;
                node.children = GetTypeOfWorkforTree(treeNodes, resps, node.MENUID);
                resps.Add(node);
            }
            return resps;
        }*/


        /// <summary>
        /// 创建树
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="root">根节点</param>
        /// <param name="list">所有数据</param>
        /// <param name="idProperty">节点唯一标识属性表达式</param>
        /// <param name="parentIdProperty">父节点属性表达式</param>
        public static void CreateTree<T>(T root, IList<T> list, string idPropertyName, string parentIdPropertyName) where T : TreeBase<T> 
        {
            root.Children = new List<T>();
            list.Where(e => (string)GetPropertyValue(e, parentIdPropertyName) == (string)GetPropertyValue(root, idPropertyName) && !e.IsLinked).ToList().ForEach(e => { root.Children.Add(e); e.IsLinked = true; });
            foreach (var leaf in root.Children)
            {
                leaf.Parent = root;
                CreateTree<T>(leaf, list, idPropertyName, parentIdPropertyName);
            }
        }
        public static List<T> CreateTree<T>(IList<T> list, Expression<Func<T, object>> idProperty, Expression<Func<T, object>> parentIdProperty) where T : TreeBase<T> 
        {
            //查找父节点不存在的leaf,作伪根节点
            var roots = new List<T>();
            var idPropertyName = GetMemberName(idProperty);
            var parentIdPropertyName = GetMemberName(parentIdProperty);
            list.Where(e => list.Count(item =>
                    (string)GetPropertyValue(item, idPropertyName) == (string)GetPropertyValue(e, parentIdPropertyName)) == 0).ToList().ForEach(e => roots.Add(e));
            foreach (var root in roots)
            {
                CreateTree<T>(root, list, idPropertyName, parentIdPropertyName);
            }
            return roots;

        }
        private static object GetPropertyValue<T>(T t, string propertyName)
        {
            return t.GetType().GetProperty(propertyName).GetValue(t, null);
        }
        private static string GetMemberName<T, TMember>(Expression<Func<T, TMember>> propertySelector)
        {
            var propertyExp = propertySelector.Body as MemberExpression;
            if (propertyExp == null)
            {
                throw new ArgumentException("不合理的表达式!");
            }
            return propertyExp.Member.Name;
        }
    }
}
