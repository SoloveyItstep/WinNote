﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinNote.Model.Entity.DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WinEntities : DbContext
    {
        public WinEntities()
            : base("name=WinEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<LogedInUser> LogedInUsers { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<CalendarEventsToLoadOnServer> CalendarEventsToLoadOnServers { get; set; }
        public virtual DbSet<CalendarSharedEventAction> CalendarSharedEventActions { get; set; }
        public virtual DbSet<NotepadCategories> NotepadCategories { get; set; }
        public virtual DbSet<NotepadTextDocuments> NotepadTextDocuments { get; set; }
        public virtual DbSet<NotepadCategoryChanged> NotepadCategoryChangeds { get; set; }
        public virtual DbSet<NotepadTextDocumentsChanged> NotepadTextDocumentsChangeds { get; set; }
    }
}
