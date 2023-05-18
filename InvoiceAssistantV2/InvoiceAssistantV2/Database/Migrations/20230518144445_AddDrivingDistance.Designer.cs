﻿// <auto-generated />
using System;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(InvoiceAssistantDbContext))]
    [Migration("20230518144445_AddDrivingDistance")]
    partial class AddDrivingDistance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AddressLine1")
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressLine3")
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressLine4")
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressLine5")
                        .HasColumnType("TEXT");

                    b.Property<int>("CompanyDetailsID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DrivingDistanceToAddress")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyDetailsID");

                    b.ToTable("CompanyAddress");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CompanyDetails");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AddressToMakeInvoiceOutToId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfInvoice")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateRecievedMoney")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaymentTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReferenceNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalInvoiceAmmount")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AddressToMakeInvoiceOutToId");

                    b.HasIndex("PaymentTypeID");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.InvoicePaymentBreakDown", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Ammount")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoicesPaymentBreakDown");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PaymentTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Cheque"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Cash"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bank Transfer"
                        });
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.PlacesVisitedForInvoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyAddressId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DrivingDistanceToAddress")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfTimesVisited")
                        .HasColumnType("INTEGER");

                    b.HasKey("InvoiceId", "CompanyAddressId");

                    b.HasIndex("CompanyAddressId");

                    b.ToTable("PlacesVisitedForInvoice");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserDetailsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserDetailsId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.PaymetDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.UserDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UsersName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserDetails");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress", b =>
                {
                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyDetails", "CompanyDetails")
                        .WithMany()
                        .HasForeignKey("CompanyDetailsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyDetails");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.Invoice", b =>
                {
                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress", "AddressToMakeInvoiceOutTo")
                        .WithMany()
                        .HasForeignKey("AddressToMakeInvoiceOutToId");

                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.Invoice.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeID");

                    b.Navigation("AddressToMakeInvoiceOutTo");

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.InvoicePaymentBreakDown", b =>
                {
                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.Invoice.Invoice", null)
                        .WithMany("InvoicePayments")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.PlacesVisitedForInvoice", b =>
                {
                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress", "CompanyAddress")
                        .WithMany("PlacesVisitedForInvoice")
                        .HasForeignKey("CompanyAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.Invoice.Invoice", "Invoice")
                        .WithMany("PlacesVisitedForInvoice")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyAddress");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.PaymentMethod", b =>
                {
                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.User.UserDetails", null)
                        .WithMany("PaymentMethods")
                        .HasForeignKey("UserDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.PaymetDetail", b =>
                {
                    b.HasOne("InvoiceAssistantV2.Shared.Models.Database.User.PaymentMethod", null)
                        .WithMany("PaymetDetails")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress", b =>
                {
                    b.Navigation("PlacesVisitedForInvoice");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.Invoice.Invoice", b =>
                {
                    b.Navigation("InvoicePayments");

                    b.Navigation("PlacesVisitedForInvoice");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.PaymentMethod", b =>
                {
                    b.Navigation("PaymetDetails");
                });

            modelBuilder.Entity("InvoiceAssistantV2.Shared.Models.Database.User.UserDetails", b =>
                {
                    b.Navigation("PaymentMethods");
                });
#pragma warning restore 612, 618
        }
    }
}
