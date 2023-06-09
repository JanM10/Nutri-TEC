﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NutriTEC_API_PG.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<AsignacionPlan> AsignacionPlans { get; set; }

    public virtual DbSet<AsociacionCliente> AsociacionClientes { get; set; }

    //Experimental
    public DbSet<PacientePorNutri> PacientePorNutri { get; set; }

    //Experimental
    public DbSet<PacienteSinNutricionista> PacienteSinNutricionista { get; set; }

    //Experimental
    public DbSet<PlanPorPaciente> PlanPorPaciente { get; set; }

    //Experimental
    public DbSet<ReporteCobro> ReporteCobro { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Consumo> Consumos { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<GestionPlane> GestionPlanes { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<Medida> Medidas { get; set; }

    public virtual DbSet<Nutricionistum> Nutricionista { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<TipoPago> TipoPagos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=nutritec-azure.postgres.database.azure.com;Database=postgres;Username=NutriTECAdmin;Password=Postgres12345!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "azure")
            .HasPostgresExtension("pg_catalog", "pgaadauth")
            .HasPostgresExtension("pg_cron");

        //Experimental 
        modelBuilder.Entity<PacientePorNutri>().HasNoKey();

        //Experimental 
        modelBuilder.Entity<PlanPorPaciente>().HasNoKey();

        //Experimental 
        modelBuilder.Entity<ReporteCobro>().HasNoKey();

        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("Administrador_pkey");

            entity.ToTable("Administrador");

            entity.Property(e => e.Cedula)
                .ValueGeneratedNever()
                .HasColumnName("cedula");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(40)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(40)
                .HasColumnName("apellido2");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
            entity.Property(e => e.Usuario)
                .HasMaxLength(40)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<AsignacionPlan>(entity =>
        {
            entity.HasKey(e => new { e.Cedulapaciente, e.Planasignado }).HasName("AsignacionPlan_pkey");

            entity.ToTable("AsignacionPlan");

            entity.Property(e => e.Cedulapaciente).HasColumnName("cedulapaciente");
            entity.Property(e => e.Planasignado).HasColumnName("planasignado");
            entity.Property(e => e.Fechafin)
                .HasMaxLength(10)
                .HasColumnName("fechafin");
            entity.Property(e => e.Fechainicio)
                .HasMaxLength(10)
                .HasColumnName("fechainicio");

            entity.HasOne(d => d.CedulapacienteNavigation).WithMany(p => p.AsignacionPlans)
                .HasForeignKey(d => d.Cedulapaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_asplanxcliente");

            entity.HasOne(d => d.PlanasignadoNavigation).WithMany(p => p.AsignacionPlans)
                .HasForeignKey(d => d.Planasignado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_asplanxplan");
        });

        modelBuilder.Entity<AsociacionCliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AsociacionCliente");

            entity.Property(e => e.Cedulanutricionista).HasColumnName("cedulanutricionista");
            entity.Property(e => e.Cedulapaciente).HasColumnName("cedulapaciente");

            entity.HasOne(d => d.CedulanutricionistaNavigation).WithMany()
                .HasForeignKey(d => d.Cedulanutricionista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_asociacionclientexnutri");

            entity.HasOne(d => d.CedulapacienteNavigation).WithMany()
                .HasForeignKey(d => d.Cedulapaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_asociacionclientexcliente");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clientes_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Altura)
                .HasPrecision(5, 2)
                .HasColumnName("altura");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(40)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(40)
                .HasColumnName("apellido2");
            entity.Property(e => e.Caderas)
                .HasPrecision(5, 2)
                .HasColumnName("caderas");
            entity.Property(e => e.CaloriasMaximas).HasColumnName("calorias_maximas");
            entity.Property(e => e.Cintura)
                .HasPrecision(5, 2)
                .HasColumnName("cintura");
            entity.Property(e => e.Cuello)
                .HasPrecision(5, 2)
                .HasColumnName("cuello");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FechaNacimiento)
                .HasMaxLength(10)
                .HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Imc)
                .HasPrecision(5, 2)
                .HasComputedColumnSql("((peso / altura) * altura)", true)
                .HasColumnName("imc");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
            entity.Property(e => e.PaisResidencia)
                .HasMaxLength(50)
                .HasColumnName("pais_residencia");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
            entity.Property(e => e.Peso)
                .HasPrecision(5, 2)
                .HasColumnName("peso");
            entity.Property(e => e.PorcentajeGrasa)
                .HasPrecision(5, 2)
                .HasColumnName("porcentaje_grasa");
            entity.Property(e => e.PorcentajeMusculo)
                .HasPrecision(5, 2)
                .HasColumnName("porcentaje_musculo");
        });

        modelBuilder.Entity<Consumo>(entity =>
        {
            entity.HasKey(e => new { e.IdCliente, e.Fecha }).HasName("Consumo_pkey");

            entity.ToTable("Consumo");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Fecha)
                .HasMaxLength(10)
                .HasColumnName("fecha");
            entity.Property(e => e.Almuerzo).HasColumnName("almuerzo");
            entity.Property(e => e.Cena).HasColumnName("cena");
            entity.Property(e => e.Desayuno).HasColumnName("desayuno");
            entity.Property(e => e.MeriendaMañana).HasColumnName("merienda_mañana");
            entity.Property(e => e.MeriendaTarde).HasColumnName("merienda_tarde");

            entity.HasOne(d => d.AlmuerzoNavigation).WithMany(p => p.ConsumoAlmuerzoNavigations)
                .HasForeignKey(d => d.Almuerzo)
                .HasConstraintName("fk_consumoxalmuerzo");

            entity.HasOne(d => d.CenaNavigation).WithMany(p => p.ConsumoCenaNavigations)
                .HasForeignKey(d => d.Cena)
                .HasConstraintName("fk_consumoxcena");

            entity.HasOne(d => d.DesayunoNavigation).WithMany(p => p.ConsumoDesayunoNavigations)
                .HasForeignKey(d => d.Desayuno)
                .HasConstraintName("fk_consumoxdesayuno");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Consumos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_clientexconsumo");

            entity.HasOne(d => d.MeriendaMañanaNavigation).WithMany(p => p.ConsumoMeriendaMañanaNavigations)
                .HasForeignKey(d => d.MeriendaMañana)
                .HasConstraintName("fk_consumoxmerienda_mañana");

            entity.HasOne(d => d.MeriendaTardeNavigation).WithMany(p => p.ConsumoMeriendaTardeNavigations)
                .HasForeignKey(d => d.MeriendaTarde)
                .HasConstraintName("fk_consumoxmerienda_tarde");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Direccion_pkey");

            entity.ToTable("Direccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Canton)
                .HasMaxLength(50)
                .HasColumnName("canton");
            entity.Property(e => e.Distrito)
                .HasMaxLength(50)
                .HasColumnName("distrito");
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .HasColumnName("provincia");
        });

        modelBuilder.Entity<GestionPlane>(entity =>
        {
            entity.HasKey(e => e.IdPlan).HasName("GestionPlanes_pkey");

            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.Almuerzo).HasColumnName("almuerzo");
            entity.Property(e => e.Caloriastotales).HasColumnName("caloriastotales");
            entity.Property(e => e.Cedulanutricionista).HasColumnName("cedulanutricionista");
            entity.Property(e => e.Cena).HasColumnName("cena");
            entity.Property(e => e.Desayuno).HasColumnName("desayuno");
            entity.Property(e => e.MeriendaMañana).HasColumnName("merienda_mañana");
            entity.Property(e => e.MeriendaTarde).HasColumnName("merienda_tarde");
            entity.Property(e => e.Nombreplan)
                .HasMaxLength(40)
                .HasColumnName("nombreplan");

            entity.HasOne(d => d.AlmuerzoNavigation).WithMany(p => p.GestionPlaneAlmuerzoNavigations)
                .HasForeignKey(d => d.Almuerzo)
                .HasConstraintName("fk_planxalmuerzo");

            entity.HasOne(d => d.CenaNavigation).WithMany(p => p.GestionPlaneCenaNavigations)
                .HasForeignKey(d => d.Cena)
                .HasConstraintName("fk_planxcena");

            entity.HasOne(d => d.DesayunoNavigation).WithMany(p => p.GestionPlaneDesayunoNavigations)
                .HasForeignKey(d => d.Desayuno)
                .HasConstraintName("fk_planxdesayuno");

            entity.HasOne(d => d.MeriendaMañanaNavigation).WithMany(p => p.GestionPlaneMeriendaMañanaNavigations)
                .HasForeignKey(d => d.MeriendaMañana)
                .HasConstraintName("fk_planxmerienda_mañana");

            entity.HasOne(d => d.MeriendaTardeNavigation).WithMany(p => p.GestionPlaneMeriendaTardeNavigations)
                .HasForeignKey(d => d.MeriendaTarde)
                .HasConstraintName("fk_planxmerienda_tarde");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Idproducto).HasColumnName("idproducto");
            entity.Property(e => e.Idreceta)
                .ValueGeneratedOnAdd()
                .HasColumnName("idreceta");

            entity.HasOne(d => d.IdproductoNavigation).WithMany()
                .HasForeignKey(d => d.Idproducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ingredientesxprducto");

            entity.HasOne(d => d.IdrecetaNavigation).WithMany()
                .HasForeignKey(d => d.Idreceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ingredientesxreceta");
        });

        modelBuilder.Entity<Medida>(entity =>
        {
            entity.HasKey(e => e.IdMedida).HasName("Medidas_pkey");

            entity.Property(e => e.IdMedida).HasColumnName("id_medida");
            entity.Property(e => e.Caderas)
                .HasPrecision(5, 2)
                .HasColumnName("caderas");
            entity.Property(e => e.Cintura)
                .HasPrecision(5, 2)
                .HasColumnName("cintura");
            entity.Property(e => e.Cuello)
                .HasPrecision(5, 2)
                .HasColumnName("cuello");
            entity.Property(e => e.Fecha)
                .HasMaxLength(10)
                .HasColumnName("fecha");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.PorcentajeGrasa)
                .HasPrecision(5, 2)
                .HasColumnName("porcentaje_grasa");
            entity.Property(e => e.PorcentajeMusculo)
                .HasPrecision(5, 2)
                .HasColumnName("porcentaje_musculo");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Medida)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_medidasxcliente");
        });

        modelBuilder.Entity<Nutricionistum>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("Nutricionista_pkey");

            entity.Property(e => e.Cedula)
                .ValueGeneratedNever()
                .HasColumnName("cedula");
            entity.Property(e => e.Altura).HasColumnName("altura");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(40)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(40)
                .HasColumnName("apellido2");
            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FechaNacimiento)
                .HasMaxLength(10)
                .HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Foto)
                .HasMaxLength(200)
                .HasColumnName("foto");
            entity.Property(e => e.Imc)
                .HasPrecision(5, 2)
                .HasComputedColumnSql("((peso / altura) * altura)", true)
                .HasColumnName("imc");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
            entity.Property(e => e.Peso).HasColumnName("peso");
            entity.Property(e => e.Tarjeta)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("tarjeta");
            entity.Property(e => e.Tipocobro).HasColumnName("tipocobro");

            entity.HasOne(d => d.DireccionNavigation).WithMany(p => p.Nutricionista)
                .HasForeignKey(d => d.Direccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_nutrixdire");

            entity.HasOne(d => d.TipocobroNavigation).WithMany(p => p.Nutricionista)
                .HasForeignKey(d => d.Tipocobro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_nutrixpago");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.CodigoBarras).HasName("Producto_pkey");

            entity.ToTable("Producto");

            entity.Property(e => e.CodigoBarras)
                .ValueGeneratedNever()
                .HasColumnName("codigo_barras");
            entity.Property(e => e.Aprobado).HasColumnName("aprobado");
            entity.Property(e => e.Calcio)
                .HasPrecision(5, 2)
                .HasColumnName("calcio");
            entity.Property(e => e.Carbohidratos)
                .HasPrecision(5, 2)
                .HasColumnName("carbohidratos");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.Energia)
                .HasPrecision(5, 2)
                .HasColumnName("energia");
            entity.Property(e => e.Grasa)
                .HasPrecision(5, 2)
                .HasColumnName("grasa");
            entity.Property(e => e.Hierro)
                .HasPrecision(5, 2)
                .HasColumnName("hierro");
            entity.Property(e => e.Proteina)
                .HasPrecision(5, 2)
                .HasColumnName("proteina");
            entity.Property(e => e.Sodio)
                .HasPrecision(5, 2)
                .HasColumnName("sodio");
            entity.Property(e => e.TamanoPorciones)
                .HasPrecision(5, 2)
                .HasColumnName("tamano_porciones");
            entity.Property(e => e.Vitaminas)
                .HasMaxLength(100)
                .HasColumnName("vitaminas");
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Receta_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombrereceta)
                .HasMaxLength(100)
                .HasColumnName("nombrereceta");
        });

        modelBuilder.Entity<TipoPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TipoPago_pkey");

            entity.ToTable("TipoPago");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipo)
                .HasMaxLength(40)
                .HasColumnName("tipo");
        });
        modelBuilder.HasSequence("jobid_seq", "cron");
        modelBuilder.HasSequence("runid_seq", "cron");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
