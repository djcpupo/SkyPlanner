using Microsoft.EntityFrameworkCore;
using SkyPlanner.Entities;
using System;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public DbSet<Account> Account { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderLineItem> OrderLineItem { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public ApplicationDbContext()
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
}