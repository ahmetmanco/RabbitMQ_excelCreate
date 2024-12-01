﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FileCreateWorkerService.Models;

public partial class AdventureWorksLt2022Context : DbContext
{
    public AdventureWorksLt2022Context()
    {
    }

    public AdventureWorksLt2022Context(DbContextOptions<AdventureWorksLt2022Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Product_ProductID");

            entity.ToTable("Product", "SalesLT", tb => tb.HasComment("Products sold or used in the manfacturing of sold products."));

            entity.HasIndex(e => e.Name, "AK_Product_Name").IsUnique();

            entity.HasIndex(e => e.ProductNumber, "AK_Product_ProductNumber").IsUnique();

            entity.HasIndex(e => e.Rowguid, "AK_Product_rowguid").IsUnique();

            entity.Property(e => e.ProductId)
                .HasComment("Primary key for Product records.")
                .HasColumnName("ProductID");
            entity.Property(e => e.Color)
                .HasMaxLength(15)
                .HasComment("Product color.");
            entity.Property(e => e.DiscontinuedDate)
                .HasComment("Date the product was discontinued.")
                .HasColumnType("datetime");
            entity.Property(e => e.ListPrice)
                .HasComment("Selling price.")
                .HasColumnType("money");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("Name of the product.");
            entity.Property(e => e.ProductCategoryId)
                .HasComment("Product is a member of this product category. Foreign key to ProductCategory.ProductCategoryID. ")
                .HasColumnName("ProductCategoryID");
            entity.Property(e => e.ProductModelId)
                .HasComment("Product is a member of this product model. Foreign key to ProductModel.ProductModelID.")
                .HasColumnName("ProductModelID");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(25)
                .HasComment("Unique product identification number.");
            entity.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
            entity.Property(e => e.SellEndDate)
                .HasComment("Date the product was no longer available for sale.")
                .HasColumnType("datetime");
            entity.Property(e => e.SellStartDate)
                .HasComment("Date the product was available for sale.")
                .HasColumnType("datetime");
            entity.Property(e => e.Size)
                .HasMaxLength(5)
                .HasComment("Product size.");
            entity.Property(e => e.StandardCost)
                .HasComment("Standard cost of the product.")
                .HasColumnType("money");
            entity.Property(e => e.ThumbNailPhoto).HasComment("Small image of the product.");
            entity.Property(e => e.ThumbnailPhotoFileName)
                .HasMaxLength(50)
                .HasComment("Small image file name.");
            entity.Property(e => e.Weight)
                .HasComment("Product weight.")
                .HasColumnType("decimal(8, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
