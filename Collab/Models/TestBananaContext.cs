using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Collab.Models;

public partial class TestBananaContext : DbContext
{
    public TestBananaContext()
    {
    }

    public TestBananaContext(DbContextOptions<TestBananaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Intent> Intents { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<Notebook> Notebooks { get; set; }

    public virtual DbSet<Notify> Notifies { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<ProgramLinkList> ProgramLinkLists { get; set; }

    public virtual DbSet<ProgramMember> ProgramMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=TestBanana;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Intent>(entity =>
        {
            entity.HasKey(e => e.IntentId).HasName("PK__Intent__C3B052A7FD13C3AF");

            entity.ToTable("Intent", tb =>
                {
                    tb.HasTrigger("TR_Intent_Insret");
                    tb.HasTrigger("TR_Intent_Update");
                });

            entity.HasIndex(e => e.IntentName, "UQ__Intent__A65767F5D8CF3CE7").IsUnique();

            entity.Property(e => e.IntentId).HasColumnName("IntentID");
            entity.Property(e => e.IntentName).HasMaxLength(50);
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

            entity.HasOne(d => d.Program).WithMany(p => p.Intents)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__Intent__ProgramI__44FF419A");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Member__0CF04B38F3378C77");

            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MemberAccount).HasMaxLength(30);
            entity.Property(e => e.MemberName).HasMaxLength(20);
            entity.Property(e => e.MemberPassword).HasMaxLength(15);
            entity.Property(e => e.MemberPhoto)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__Mission__66DFB854B02C4DCE");

            entity.ToTable("Mission", tb =>
                {
                    tb.HasTrigger("TR_Mission_Insert");
                    tb.HasTrigger("TR_Mission_Update");
                    tb.HasTrigger("UpdateMissionCountTotal");
                });

            entity.HasIndex(e => e.MissionName, "UQ__Mission__FA0F0F0B715AC3B0").IsUnique();

            entity.Property(e => e.MissionId).HasColumnName("MissionID");
            entity.Property(e => e.IntentId).HasColumnName("IntentID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MisDescribe).HasMaxLength(500);
            entity.Property(e => e.MisFinishTime).HasColumnType("date");
            entity.Property(e => e.MisStartTime).HasColumnType("date");
            entity.Property(e => e.MisState).HasMaxLength(10);
            entity.Property(e => e.MissionName).HasMaxLength(50);

            entity.HasOne(d => d.Intent).WithMany(p => p.Missions)
                .HasForeignKey(d => d.IntentId)
                .HasConstraintName("FK__Mission__IntentI__48CFD27E");

            entity.HasOne(d => d.Member).WithMany(p => p.Missions)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__Mission__MemberI__49C3F6B7");
        });

        modelBuilder.Entity<Notebook>(entity =>
        {
            entity.HasKey(e => e.NotebookId).HasName("PK__Notebook__0CBEE8A41A051395");

            entity.ToTable("Notebook", tb =>
                {
                    tb.HasTrigger("TR_Notebook_Insert");
                    tb.HasTrigger("TR_Notebook_Update");
                });

            entity.Property(e => e.NotebookId).HasColumnName("NotebookID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.NotebooAddDate).HasColumnType("datetime");
            entity.Property(e => e.NotebookContent).HasMaxLength(200);
            entity.Property(e => e.NotebookTitle).HasMaxLength(20);
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

            entity.HasOne(d => d.Member).WithMany(p => p.Notebooks)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__Notebook__Member__6754599E");

            entity.HasOne(d => d.Program).WithMany(p => p.Notebooks)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__Notebook__Progra__66603565");
        });

        modelBuilder.Entity<Notify>(entity =>
        {
            entity.HasKey(e => e.NotifyId).HasName("PK__Notify__AD54A2DC9CA6A61E");

            entity.ToTable("Notify");

            entity.Property(e => e.NotifyId).HasColumnName("NotifyID");
            entity.Property(e => e.ActionName).HasMaxLength(20);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MemberName).HasMaxLength(20);
            entity.Property(e => e.NotifyAction).HasMaxLength(5);
            entity.Property(e => e.NotifyDate).HasColumnType("datetime");
            entity.Property(e => e.NotifyType).HasMaxLength(5);
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

            entity.HasOne(d => d.Member).WithMany(p => p.Notifies)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Notify_Member");

            entity.HasOne(d => d.Program).WithMany(p => p.Notifies)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__Notify__ProgramI__6383C8BA");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__Program__752560386F1A0420");

            entity.ToTable("Program", tb =>
                {
                    tb.HasTrigger("TR_Program_Insert");
                    tb.HasTrigger("TR_Program_Update");
                });

            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.ProgramColor)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProgramName).HasMaxLength(50);
            entity.Property(e => e.ProgramOverview).HasMaxLength(1000);
        });

        modelBuilder.Entity<ProgramLinkList>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__ProgramL__2D12215515EB00A2");

            entity.ToTable("ProgramLinkList");

            entity.Property(e => e.LinkId).HasColumnName("LinkID");
            entity.Property(e => e.LinkTitle)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.LinkUrl)
                .HasMaxLength(50)
                .HasColumnName("LinkURL");
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramLinkLists)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramLi__Progr__4CA06362");
        });

        modelBuilder.Entity<ProgramMember>(entity =>
        {
            entity.HasKey(e => e.ProgramMemberId).HasName("PK__ProgramM__A6D4D87A63907D2B");

            entity.ToTable("ProgramMember", tb =>
                {
                    tb.HasTrigger("TR_ProgramMember_Delete");
                    tb.HasTrigger("TR_ProgramMember_Insert");
                });

            entity.Property(e => e.ProgramMemberId).HasColumnName("ProgramMemberID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MemberState)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

            entity.HasOne(d => d.Member).WithMany(p => p.ProgramMembers)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_ProgramMember_Member");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramMembers)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramMe__Progr__29572725");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
