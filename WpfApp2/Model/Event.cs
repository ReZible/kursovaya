//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp2.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.EventUsers = new HashSet<EventUsers>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganizeId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
    
        public virtual EventStatus EventStatus { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventUsers> EventUsers { get; set; }
    }
}