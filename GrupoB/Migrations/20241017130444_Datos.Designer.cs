﻿// <auto-generated />
using System;
using CuponesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CuponesApi.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20241017130444_Datos")]
    partial class Datos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CuponesApi.Models.ArticuloModel", b =>
                {
                    b.Property<int>("Id_Articulo")
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion_Articulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre_Articulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Articulo");

                    b.ToTable("Articulos");
                });

            modelBuilder.Entity("CuponesApi.Models.CategoriaModel", b =>
                {
                    b.Property<int>("Id_Categoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Categoria"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Categoria");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("CuponesApi.Models.CuponModel", b =>
                {
                    b.Property<int>("Id_Cupon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Cupon"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Tipo_Cupon")
                        .HasColumnType("int");

                    b.Property<decimal>("ImportePromo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ProcentajeDto")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id_Cupon");

                    b.HasIndex("Id_Tipo_Cupon");

                    b.ToTable("Cupones");
                });

            modelBuilder.Entity("CuponesApi.Models.Cupon_CategoriaModel", b =>
                {
                    b.Property<int>("Id_Cupones_Categorias")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Cupones_Categorias"));

                    b.Property<int>("Id_Categoria")
                        .HasColumnType("int");

                    b.Property<int>("Id_Cupon")
                        .HasColumnType("int");

                    b.HasKey("Id_Cupones_Categorias");

                    b.HasIndex("Id_Categoria");

                    b.HasIndex("Id_Cupon");

                    b.ToTable("Cupones_Categorias");
                });

            modelBuilder.Entity("CuponesApi.Models.Cupon_ClienteModel", b =>
                {
                    b.Property<string>("NroCupon")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaAsignado")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Cupon")
                        .HasColumnType("int");

                    b.HasKey("NroCupon");

                    b.ToTable("Cupones_Clientes");
                });

            modelBuilder.Entity("CuponesApi.Models.Cupones_DetallesModel", b =>
                {
                    b.Property<int>("Id_Cupon")
                        .HasColumnType("int");

                    b.Property<int>("Id_Articulo")
                        .HasColumnType("int");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.HasKey("Id_Cupon", "Id_Articulo");

                    b.ToTable("Cupones_Detalle");
                });

            modelBuilder.Entity("CuponesApi.Models.Cupones_HistorialModel", b =>
                {
                    b.Property<int>("Id_Cupon")
                        .HasColumnType("int");

                    b.Property<string>("NroCupon")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaUso")
                        .HasColumnType("datetime2");

                    b.HasKey("Id_Cupon", "NroCupon");

                    b.ToTable("Cupones_Historial");
                });

            modelBuilder.Entity("CuponesApi.Models.PrecioModel", b =>
                {
                    b.Property<int>("Id_Precio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Precio"));

                    b.Property<int>("Id_Articulo")
                        .HasColumnType("int");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id_Precio");

                    b.ToTable("Precios");
                });

            modelBuilder.Entity("CuponesApi.Models.Tipo_CuponModel", b =>
                {
                    b.Property<int>("Id_Tipo_Cupon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Tipo_Cupon"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Tipo_Cupon");

                    b.ToTable("Tipo_Cupon");
                });

            modelBuilder.Entity("CuponesApi.Models.ArticuloModel", b =>
                {
                    b.HasOne("CuponesApi.Models.PrecioModel", "Precio")
                        .WithMany()
                        .HasForeignKey("Id_Articulo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Precio");
                });

            modelBuilder.Entity("CuponesApi.Models.CuponModel", b =>
                {
                    b.HasOne("CuponesApi.Models.Tipo_CuponModel", "Tipo_Cupon")
                        .WithMany()
                        .HasForeignKey("Id_Tipo_Cupon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tipo_Cupon");
                });

            modelBuilder.Entity("CuponesApi.Models.Cupon_CategoriaModel", b =>
                {
                    b.HasOne("CuponesApi.Models.CategoriaModel", "Categoria")
                        .WithMany("Cupones_Categorias")
                        .HasForeignKey("Id_Categoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CuponesApi.Models.CuponModel", "Cupòn")
                        .WithMany("Cupones_Categorias")
                        .HasForeignKey("Id_Cupon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Cupòn");
                });

            modelBuilder.Entity("CuponesApi.Models.CategoriaModel", b =>
                {
                    b.Navigation("Cupones_Categorias");
                });

            modelBuilder.Entity("CuponesApi.Models.CuponModel", b =>
                {
                    b.Navigation("Cupones_Categorias");
                });
#pragma warning restore 612, 618
        }
    }
}
