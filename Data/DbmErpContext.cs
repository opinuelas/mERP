using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace mERP.Data;

public partial class DbmErpContext : DbContext
{
    public DbmErpContext(DbContextOptions<DbmErpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coll> Colls { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Param> Params { get; set; }

    public virtual DbSet<Raccess> Raccesses { get; set; }

    public virtual DbSet<Urole> Uroles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coll>(entity =>
        {
            entity.ToTable("colls");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Cname)
                .HasMaxLength(250)
                .HasColumnName("cname");
            entity.Property(e => e.Value)
                .HasMaxLength(250)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientip)
                .HasMaxLength(20)
                .HasColumnName("clientip");
            entity.Property(e => e.Ldate)
                .HasColumnType("datetime")
                .HasColumnName("ldate");
            entity.Property(e => e.Userid)
                .HasMaxLength(30)
                .HasColumnName("userid");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Param>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_parametros");

            entity.ToTable("params");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Alphac)
                .HasMaxLength(30)
                .HasColumnName("alphac");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Descrip)
                .HasMaxLength(100)
                .HasColumnName("descrip");
            entity.Property(e => e.Udate)
                .HasColumnType("datetime")
                .HasColumnName("udate");
            entity.Property(e => e.Userid)
                .HasMaxLength(20)
                .HasColumnName("userid");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Raccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__wsciacc__3213E83F7C9A29A5");

            entity.ToTable("raccess");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accctlr)
                .HasMaxLength(10)
                .HasColumnName("accctlr");
            entity.Property(e => e.Accdesc)
                .HasMaxLength(70)
                .HasColumnName("accdesc");
            entity.Property(e => e.Accicon)
                .HasMaxLength(80)
                .HasColumnName("accicon");
            entity.Property(e => e.Accmnu)
                .HasMaxLength(10)
                .HasColumnName("accmnu");
            entity.Property(e => e.Accord).HasColumnName("accord");
            entity.Property(e => e.Accpag)
                .HasMaxLength(100)
                .HasColumnName("accpag");
            entity.Property(e => e.Acctar)
                .HasMaxLength(15)
                .HasColumnName("acctar");
            entity.Property(e => e.Accval)
                .HasMaxLength(5)
                .HasColumnName("accval");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__wsciacc__ParentI__38996AB5");
        });

        modelBuilder.Entity<Urole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__uroles__3213E83F6D09E18F");

            entity.ToTable("uroles");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Rname)
                .HasMaxLength(55)
                .HasColumnName("rname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Usrcell)
                .HasMaxLength(30)
                .HasColumnName("usrcell");
            entity.Property(e => e.Usremail)
                .HasMaxLength(130)
                .HasColumnName("usremail");
            entity.Property(e => e.Usrid)
                .HasMaxLength(20)
                .HasColumnName("usrid");
            entity.Property(e => e.Usriniop)
                .HasMaxLength(50)
                .HasColumnName("usriniop");
            entity.Property(e => e.Usrname)
                .HasMaxLength(70)
                .HasColumnName("usrname");
            entity.Property(e => e.Usrpar)
                .HasMaxLength(50)
                .HasColumnName("usrpar");
            entity.Property(e => e.Usrpwd)
                .HasMaxLength(100)
                .HasColumnName("usrpwd");
            entity.Property(e => e.Usrrole).HasColumnName("usrrole");
            entity.Property(e => e.Usrsts).HasColumnName("usrsts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
