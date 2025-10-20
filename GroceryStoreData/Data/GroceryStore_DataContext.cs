using GroceryStoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreData.Data;

public partial class GroceryStore_DataContext : DbContext
{
    public GroceryStore_DataContext()
    {

    }

    public GroceryStore_DataContext(DbContextOptions<GroceryStore_DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreProduct> StoreProducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UvShoppingList> UvShoppingLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UnitOfMeasure).HasMaxLength(10);
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.ToTable("ShoppingList");

            entity.Property(e => e.ShoppingListId).HasColumnName("ShoppingListID");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Title).HasMaxLength(1000);
            entity.Property(e => e.UpdatedBy).HasMaxLength(450);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Store).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingList_Store");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingList_User");
        });

        modelBuilder.Entity<ShoppingListItem>(entity =>
        {
            entity.HasKey(e => e.ShoppingListItemId);

            entity.ToTable("ShoppingListItem");

            entity.Property(e => e.Quantity).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ShoppingListId).HasColumnName("ShoppingListID");
            entity.Property(e => e.StoreProductId).HasColumnName("StoreProductID");

            entity.HasOne(d => d.ShoppingList).WithMany(p => p.ShoppingListItems)
                .HasForeignKey(d => d.ShoppingListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingListItem_ShoppingList");

            entity.HasOne(d => d.StoreProduct).WithMany(p => p.ShoppingListItems)
                .HasForeignKey(d => d.StoreProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingListItem_StoreProduct");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State");

            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Address1).HasMaxLength(255);
            entity.Property(e => e.Address2).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.UpdatedBy).HasMaxLength(450);

            entity.HasOne(d => d.State).WithMany(p => p.Stores)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Store_State");
        });

        modelBuilder.Entity<StoreProduct>(entity =>
        {
            entity.ToTable("StoreProduct");

            entity.HasIndex(e => new { e.StoreId, e.ProductId }, "IX_StoreProduct_StoreID_ProductID").IsUnique();

            entity.Property(e => e.StoreProductId).HasColumnName("StoreProductID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Product).WithMany(p => p.StoreProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreProduct_Product");

            entity.HasOne(d => d.Store).WithMany(p => p.StoreProducts)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreProduct_Store");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(450);
        });

        modelBuilder.Entity<UvShoppingList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uv_ShoppingLists");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.ListTitle).HasMaxLength(1000);
            entity.Property(e => e.Product).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Store).HasMaxLength(255);
            entity.Property(e => e.StoreAddress1).HasMaxLength(255);
            entity.Property(e => e.StoreAddress2).HasMaxLength(255);
            entity.Property(e => e.StoreCity).HasMaxLength(255);
            entity.Property(e => e.StoreState)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UnitOfMeasure).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
