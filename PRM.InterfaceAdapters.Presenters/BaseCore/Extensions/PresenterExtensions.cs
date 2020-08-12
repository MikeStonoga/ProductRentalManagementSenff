﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PRM.InterfaceAdapters.Presenters.BaseCore.Extensions
{
    public static class PresenterExtensions
    {
        public static string FormatId(this Guid id)
        {
            return id.ToString().ToUpper();
        }
        
        public static string FormatId(this Guid? id)
        {
            return id.HasValue ? id.ToString().ToUpper() : "";
        }
        
        public static List<string> FormatIds(this IEnumerable<Guid> ids)
        {
            return ids.Select(id => id.FormatId()).ToList();
        }
        
        public static List<string> FormatIds(this IEnumerable<Guid?> ids)
        {
            return ids.Select(id => id.FormatId()).ToList();
        }

        public static string FormatDate(this DateTime date)
        {
            return date.ToShortDateString() + " " + date.ToLongTimeString();
        }
        
        public static string FormatDate(this DateTime? date)
        {
            return date.HasValue? date.Value.FormatDate() : "";
        }
    }
}