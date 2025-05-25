using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Infrastructure.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessStep> ProcessSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Workflow configuration
            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
            });

            // WorkflowStep configuration
            modelBuilder.Entity<WorkflowStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Workflow)
                      .WithMany(w => w.Steps)
                      .HasForeignKey(e => e.WorkflowId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Process configuration
            modelBuilder.Entity<Process>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Workflow)
                      .WithMany(w => w.Processes)
                      .HasForeignKey(e => e.WorkflowId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ProcessStep configuration
            modelBuilder.Entity<ProcessStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Process)
                      .WithMany(p => p.ProcessSteps)
                      .HasForeignKey(e => e.ProcessId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.WorkflowStep)
                      .WithMany(ws => ws.ProcessSteps)
                      .HasForeignKey(e => e.WorkflowStepId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
