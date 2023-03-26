﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using aplicacion.models;

#nullable disable

namespace aplicacion.Migrations
{
    [DbContext(typeof(EscriturasContext))]
    partial class EscriturasContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("aplicacion.models.Adquirente", b =>
                {
                    b.Property<int>("IdAdquirente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAdquirente"));

                    b.Property<int>("NumAtencion")
                        .HasColumnType("int");

                    b.Property<double>("PorcentajeDerecho")
                        .HasColumnType("float");

                    b.Property<bool>("PorcentajeDerechoNoAcreditado")
                        .HasColumnType("bit");

                    b.Property<string>("RunRut")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("RUN_RUT");

                    b.HasKey("IdAdquirente")
                        .HasName("PK__Adquiren__AAEC4FDB48E843BF");

                    b.HasIndex("NumAtencion");

                    b.ToTable("Adquirente", (string)null);
                });

            modelBuilder.Entity("aplicacion.models.Enajenante", b =>
                {
                    b.Property<int>("IdEnajenante")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEnajenante"));

                    b.Property<int>("NumAtencion")
                        .HasColumnType("int");

                    b.Property<double>("PorcentajeDerecho")
                        .HasColumnType("float");

                    b.Property<bool>("PorcentajeDerechoNoAcreditado")
                        .HasColumnType("bit");

                    b.Property<string>("RunRut")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("RUN_RUT");

                    b.HasKey("IdEnajenante")
                        .HasName("PK__Enajenan__2664FAC4DDAF1CA5");

                    b.HasIndex("NumAtencion");

                    b.ToTable("Enajenante", (string)null);
                });

            modelBuilder.Entity("aplicacion.models.Escritura", b =>
                {
                    b.Property<int>("NumAtencion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NumAtencion"));

                    b.Property<string>("Cne")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("CNE");

                    b.Property<string>("Comuna")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("FechaInscripcion")
                        .HasColumnType("date");

                    b.Property<int>("Fojas")
                        .HasColumnType("int");

                    b.Property<string>("Manzana")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NumeroInscripcion")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Predio")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("NumAtencion")
                        .HasName("PK__Escritur__C0692259767AB5C6");

                    b.ToTable("Escritura", (string)null);
                });

            modelBuilder.Entity("aplicacion.models.Multipropietario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnoInscripcion")
                        .HasColumnType("int");

                    b.Property<int>("AnoVigenciaFinal")
                        .HasColumnType("int");

                    b.Property<int>("AnoVigenciaInicial")
                        .HasColumnType("int");

                    b.Property<string>("Comuna")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("FechaInscripcion")
                        .HasColumnType("date");

                    b.Property<int>("Fojas")
                        .HasColumnType("int");

                    b.Property<int>("Manzana")
                        .HasColumnType("int");

                    b.Property<int>("NumeroInscripcion")
                        .HasColumnType("int");

                    b.Property<double>("PorcentajeDerecho")
                        .HasColumnType("float");

                    b.Property<int>("Predio")
                        .HasColumnType("int");

                    b.Property<string>("RunRut")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("RUN_RUT");

                    b.HasKey("Id")
                        .HasName("PK__MULTIPRO__3214EC07275953BA");

                    b.ToTable("MULTIPROPIETARIO", (string)null);
                });

            modelBuilder.Entity("aplicacion.models.Adquirente", b =>
                {
                    b.HasOne("aplicacion.models.Escritura", "NumAtencionNavigation")
                        .WithMany("Adquirentes")
                        .HasForeignKey("NumAtencion")
                        .IsRequired()
                        .HasConstraintName("FK__Adquirent__NumAt__3C69FB99");

                    b.Navigation("NumAtencionNavigation");
                });

            modelBuilder.Entity("aplicacion.models.Enajenante", b =>
                {
                    b.HasOne("aplicacion.models.Escritura", "NumAtencionNavigation")
                        .WithMany("Enajenantes")
                        .HasForeignKey("NumAtencion")
                        .IsRequired()
                        .HasConstraintName("FK__Enajenant__NumAt__398D8EEE");

                    b.Navigation("NumAtencionNavigation");
                });

            modelBuilder.Entity("aplicacion.models.Escritura", b =>
                {
                    b.Navigation("Adquirentes");

                    b.Navigation("Enajenantes");
                });
#pragma warning restore 612, 618
        }
    }
}
