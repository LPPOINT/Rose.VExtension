//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rose.VExtension.Server.Models.DbInteraction
{
    using System;
    using System.Collections.Generic;
    
    public partial class ResourceToken
    {
        public string Id { get; set; }
        public string PluginId { get; set; }
        public string ResourcePath { get; set; }
        public System.DateTime Lifetime { get; set; }
    
        public virtual Plugin Plugin { get; set; }
    }
}
