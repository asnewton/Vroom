using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vroom.Extentions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();
            SelectListItem i = new SelectListItem()
            {
                Text="----Select----",
                Value="0"
            };
            ItemList.Add(i);
            foreach(var item in Items)
            {
                SelectListItem sli = new SelectListItem()
                {
                    Text = item.GetPropertyValue("Name"),
                    Value = item.GetPropertyValue("Id")
                    //Text = item.GetType().GetProperty("Name").GetValue(item, null).ToString(),
                    //Value = item.GetType().GetProperty("Id").GetValue(item, null).ToString()
                };
                ItemList.Add(sli);
            }
            return ItemList;
        }
    }
}
