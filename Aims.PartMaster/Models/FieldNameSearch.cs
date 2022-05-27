using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Aims.Core.Models
{ 
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FieldNameSearch
    {
        [Description("Itemtype")]
        ItemType,

        [Description("Itemclass")]
        Itemclass,

        [Description("Itemcode")]
        Itemcode,

        [Description("Itemnumber")]
        Itemnumber,

        [Description("Customer")]
        Customer,

        [Description("Foreman")]
        Foreman,

        [Description("Salesperson")]
        Salesperson,

        [Description("Userid")]
        Userid,

        [Description("Uom")]
        Uom
    }
}
